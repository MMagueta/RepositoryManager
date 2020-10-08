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

        member this.Complete() =
            context.SaveChanges() |> ignore
        member this.Dispose =
            context.Dispose() |> ignore

    let uow = UnitOfWork(ctx)
