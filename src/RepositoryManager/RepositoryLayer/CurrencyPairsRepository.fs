namespace Repository

open Microsoft.EntityFrameworkCore

open Context
open Models

type CurrencyPairsRepository(contextIn : DbContext) = 
    inherit Repository(contextIn)
    let context = contextIn
    member this.GetByLabel(name : string) = 
        context.Set<CurrencyPairItem>() |> Seq.filter (fun x -> x.Label = name)

    member this.Insert(new_registry : CurrencyPairItem) = 
        context.AddAsync(new_registry)