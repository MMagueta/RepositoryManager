namespace Repository

open System
open System.Collections.Generic
open Microsoft.EntityFrameworkCore
open System.Linq
open System.Linq.Expressions

open Context
open Models

type ProvidersRepository(contextIn : DbContext) = 
    inherit Repository(contextIn)
    let context = contextIn
    member this.GetByName(name : string) = 
        context.Set<ProviderItem>() |> Seq.filter (fun x -> x.Name = name)

    member this.Insert(new_registry : ProviderItem) = 
        context.AddAsync(new_registry)