namespace Context

open Microsoft.EntityFrameworkCore

type ContextFactory = 

    static member Create(connectionString : string) = 
        let optionsBuilder = new DbContextOptionsBuilder<RepositoryContext>()
        optionsBuilder.UseNpgsql(connectionString) |> ignore

        let context = new RepositoryContext(optionsBuilder.Options)
        context.Database.EnsureDeleted() |> ignore
        context.Database.EnsureCreated() |> ignore

        context