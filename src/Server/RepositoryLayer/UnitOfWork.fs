namespace Repository

open Context

type UnitOfWork(contextIn : RepositoryContext) =
    let context = contextIn
    let providers = new Repository(context)
    let crpairs = new Repository(context)
    let pricerecords = new Repository(context)

    member this.Complete() =
        context.SaveChanges() |> ignore
    member this.Dispose =
        context.Dispose() |> ignore