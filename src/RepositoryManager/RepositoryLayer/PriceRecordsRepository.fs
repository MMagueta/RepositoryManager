namespace Repository

open System
open Microsoft.EntityFrameworkCore

open Context
open Models

type PriceRecordsRepository(contextIn : DbContext) = 
    inherit Repository(contextIn)
    let context = contextIn

    let PackListSomeOrNone x = match x with | [] -> None | _ -> Some(x) 
    let UnpackListSomeOrNone x = match x with | None -> [] | Some(x) -> x

    member this.GetAllOrderedInRange (min_date : DateTime, max_date : DateTime) =
        context.Set<PriceRecordItem>()
        |> Seq.groupBy (fun x -> x.SubProvider) 
        |> Seq.map (fun (key, values) -> (key,  values |> Seq.filter (fun x -> (x.Date >= min_date && x.Date <= max_date)) |> Seq.sortBy (fun x -> x.Date) |> List.ofSeq ))
        |> List.ofSeq
        |> PackListSomeOrNone

    member this.Insert(new_registry : PriceRecordItem) = 
        context.AddAsync(new_registry)
