namespace Repository

open System
open System.Collections.Generic
open Microsoft.EntityFrameworkCore
open System.Linq
open System.Linq.Expressions

open Context
open Models

type PriceRecordsRepository(contextIn : DbContext) = 
    inherit Repository(contextIn)
    let context = contextIn
    member this.GetByCPairs(cpairs_ids : list<int>) = 
        context.Set<PriceRecordItem>() |> Seq.filter (fun x -> List.contains x.CPair.Id cpairs_ids)