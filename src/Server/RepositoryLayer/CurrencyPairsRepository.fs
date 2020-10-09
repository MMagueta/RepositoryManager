namespace Repository

open System
open System.Collections.Generic
open Microsoft.EntityFrameworkCore
open System.Linq
open System.Linq.Expressions

open Context
open Models

type CurrencyPairsRepository(contextIn : DbContext) = 
    inherit Repository(contextIn)
    let context = contextIn
    member this.GetByLabel(name : string) = 
        context.Set<CurrencyPairItem>() |> Seq.filter (fun x -> x.Label = name)

    member this.Insert(new_registry : CurrencyPairItem) = 
        context.AddAsync(new_registry)