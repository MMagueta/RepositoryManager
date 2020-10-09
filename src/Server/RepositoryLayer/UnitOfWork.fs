namespace Repository

open Context

open System.Runtime.Serialization
open System.Runtime.Serialization.Formatters.Binary
open System.Runtime.Serialization.Json
open System.IO

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

        member this.Complete() =
            context.SaveChanges() |> ignore
        member this.Dispose =
            context.Dispose() |> ignore

    let uow = UnitOfWork(ctx)
