namespace Repository

open Context

open System.Runtime.Serialization
open System.Runtime.Serialization.Formatters.Binary
open System.Runtime.Serialization.Json
open System.IO

open Models

module UnitOfWork = 
    let ctx = ContextFactory.Create("Host=database-3.cnnri9trsf5s.us-east-2.rds.amazonaws.com;Port=5432;Pooling=true;Database=teste_pg;User Id=postgres;Password=159753123")
    type UnitOfWork(contextIn : RepositoryContext) =
        let context = contextIn
        member this.Providers = ProvidersRepository(context)
        member this.CRPairs = CurrencyPairsRepository(context)
        member this.Pricerecords = PriceRecordsRepository(context)

        member this.GetPriceRecordsByCPairs(cpairs_id : int) = 
            this.Pricerecords.GetByCPairs(cpairs_id)

        member this.GetPriceRecordsByProviders(providers_id : int) = 
            this.Pricerecords.GetByProviders(providers_id)

        member this.FilterByMaxDate(max_date : System.DateTime) = 
            max_date |> this.Pricerecords.GetByMaxDate

        member this.FilterByMinDate(min_date : System.DateTime) = 
            min_date |> this.Pricerecords.GetByMinDate

        member this.FilterByMaxQuantity(max_qtd : int) = 
            max_qtd |> this.Pricerecords.GetByMaxQuantity

        member this.FilterByMinQuantity(min_qtd : int) = 
            min_qtd |> this.Pricerecords.GetByMinQuantity

        member this.GetByDateRange(min_date : System.DateTime, max_date : System.DateTime) = 
            (min_date, max_date) |> this.Pricerecords.GetByDateRange

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

        member this.Complete() =
            context.SaveChanges() |> ignore
        member this.Dispose =
            context.Dispose() |> ignore

    let uow = UnitOfWork(ctx)
