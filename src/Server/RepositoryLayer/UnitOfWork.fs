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
        let providers = new ProvidersRepository(context)
        let crpairs = new CurrencyPairsRepository(context)
        let pricerecords = new PriceRecordsRepository(context)

        member this.GetPriceRecordsByCPairs(cpairs_id : int) = 
            pricerecords.GetByCPairs(cpairs_id)

        member this.GetPriceRecordsByProviders(providers_id : int) = 
            pricerecords.GetByProviders(providers_id)

        member this.FilterByMaxDate(max_date : System.DateTime) = 
            max_date |> pricerecords.GetByMaxDate

        member this.FilterByMinDate(min_date : System.DateTime) = 
            min_date |> pricerecords.GetByMinDate

        member this.FilterByMaxQuantity(max_qtd : int) = 
            max_qtd |> pricerecords.GetByMaxQuantity

        member this.FilterByMinQuantity(min_qtd : int) = 
            min_qtd |> pricerecords.GetByMinQuantity

        member this.GetByDateRange(min_date : System.DateTime, max_date : System.DateTime) = 
            (min_date, max_date) |> pricerecords.GetByDateRange

        member this.GetAllPricesInOrderByDate(min_date : System.DateTime, max_date : System.DateTime) = 
            (min_date, max_date) |> pricerecords.GetAllOrderedInRange

        member this.GetMarketData(min_date, max_date, currency_pair_id, provider_id) = 
            (min_date, max_date)
            |> pricerecords.GetAllOrderedInRange
            |> fun data -> match data with | None -> [] | Some(x) -> x
            |> List.map (fun (key, values) -> (key, values |> List.filter (fun data -> data.CPair.Id = currency_pair_id && data.Provider.Id = provider_id )))
            |> fun x -> match x with | [] -> None | _ -> Some(x) 

        member this.InsertPriceRecord(new_registry : PriceRecordItem) = 
            
            pricerecords.Insert(new_registry) |> ignore
            this.Complete() 

        member this.Complete() =
            context.SaveChanges() |> ignore
        member this.Dispose =
            context.Dispose() |> ignore

    let uow = UnitOfWork(ctx)
