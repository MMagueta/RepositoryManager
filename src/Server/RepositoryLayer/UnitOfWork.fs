namespace Repository

open Context

open System.Runtime.Serialization
open System.Runtime.Serialization.Formatters.Binary
open System.Runtime.Serialization.Json
open System.IO
open System
open System.Linq
open System.Linq.Expressions

open Models

module UnitOfWork = 
    let ctx = ContextFactory.Create("Host=database-3.cnnri9trsf5s.us-east-2.rds.amazonaws.com;Port=5432;Pooling=true;Database=teste_pg;User Id=postgres;Password=159753123")
    type UnitOfWork(contextIn : RepositoryContext) =
        let context = contextIn
        member this.Providers = ProvidersRepository(context)
        member this.CRPairs = CurrencyPairsRepository(context)
        member this.Pricerecords = PriceRecordsRepository(context)

        member this.PackListSomeOrNone x = match x with | [] -> None | _ -> Some(x) 
        member this.UnpackListSomeOrNone x = match x with | None -> [] | Some(x) -> x
        member this.ReturnIQuery (x : IQueryable<'T>) = x.ToArray<'T>() |>  List.ofArray |> this.PackListSomeOrNone

        member this.GetPriceRecordsByCPairs(cpairs_id : int) = 
            this.Pricerecords.Where<PriceRecordItem>((fun (x : PriceRecordItem) -> x.CPair.Id.Equals(cpairs_id)))
            |> this.ReturnIQuery

        member this.GetPriceRecordsByProviders(providers_id : int) = 
            this.Pricerecords.Where<PriceRecordItem>((fun (x : PriceRecordItem) -> x.Provider.Id.Equals(providers_id)))
            |> this.ReturnIQuery

        member this.FilterByMaxDate(max_date : System.DateTime) = 
            this.Pricerecords.Where<PriceRecordItem>((fun (x : PriceRecordItem) -> x.Date <= max_date))
            |> this.ReturnIQuery

        member this.FilterByMinDate(min_date : System.DateTime) = 
            this.Pricerecords.Where<PriceRecordItem>((fun (x : PriceRecordItem) -> x.Date >= min_date))
            |> this.ReturnIQuery

        member this.FilterByMaxQuantity(max_qtd : int) = 
            this.Pricerecords.Where<PriceRecordItem>(fun x -> x.Quantity <= max_qtd)
            |> this.ReturnIQuery

        member this.FilterByMinQuantity(min_qtd : int) = 
            this.Pricerecords.Where<PriceRecordItem>(fun x -> x.Quantity >= min_qtd)
            |> this.ReturnIQuery

        member this.GetByDateRange(min_date : System.DateTime, max_date : System.DateTime) = 
            this.Pricerecords.Where<PriceRecordItem>(fun (x : PriceRecordItem) -> x.Date >= min_date && x.Date <= max_date)
            |> this.ReturnIQuery

        member this.GetAllPricesInOrderByDate(min_date : System.DateTime, max_date : System.DateTime) = 
            (min_date, max_date) |> this.Pricerecords.GetAllOrderedInRange

        member this.GetMarketData(min_date, max_date, currency_pair_id, provider_id) = 
            (min_date, max_date)
            |> this.Pricerecords.GetAllOrderedInRange
            |> fun data -> match data with | None -> [] | Some(x) -> x
            |> List.map (fun (key, values) -> (key, values |> List.filter (fun data -> data.CPair.Id = currency_pair_id && data.Provider.Id = provider_id )))
            |> fun x -> match x with | [] -> None | _ -> Some(x) 

        member this.InsertPriceRecord(new_registry : PriceRecordItem) = 
            let aggregateAndRegister(new_registry : PriceRecordItem) = // Handle the swap of aggregate input and insert
                new_registry.Provider <- this.Providers.GetById<ProviderItem>(new_registry.Provider.Id) //Searches for existing registry and swaps it with the input
                new_registry.CPair <- this.CRPairs.GetById<CurrencyPairItem>(new_registry.CPair.Id)
                this.Pricerecords.Insert(new_registry) |> ignore //Finally insert into the context
                this.Complete() |> ignore //Calls the context to save the changes
                None // None for matching the return
            match (box (this.Providers.GetById<ProviderItem>(new_registry.Provider.Id)), box (this.Providers.GetById<CurrencyPairItem>(new_registry.CPair.Id))) with
            | (null, _) -> Some("Provider not found.")
            | (_, null) -> Some("Currency Pair not found.")
            | (_ , _) -> aggregateAndRegister(new_registry)

        member this.InsertProvider(new_registry : ProviderItem) = 
            this.Providers.Insert(new_registry) |> ignore //Insert into the context
            this.Complete() |> ignore //Calls the context to save the changes
            None // None for matching the return
            //TODO add validation for the complete and insert function returns

        member this.InsertCurrencyPair(new_registry : CurrencyPairItem) = 
            this.CRPairs.Insert(new_registry) |> ignore //Insert into the context
            this.Complete() |> ignore //Calls the context to save the changes
            None // None for matching the return
            //TODO add validation for the complete and insert function returns

        member this.Complete() =
            context.SaveChanges() |> ignore
        member this.Dispose =
            context.Dispose() |> ignore

    let uow = UnitOfWork(ctx)
