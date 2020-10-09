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

    let PackListSomeOrNone x = match x with | [] -> None | _ -> Some(x) 
    let UnpackListSomeOrNone x = match x with | None -> [] | Some(x) -> x

    member this.GetByCPairs(cpairs_id : int) = 
        context.Set<PriceRecordItem>() |> Seq.tryFind (fun x -> x.CPair.Id = cpairs_id)

    member this.GetByProviders(providers_id : int) = 
        context.Set<PriceRecordItem>() |> Seq.tryFind (fun x -> x.Provider.Id = providers_id)

    member this.GetByMaxDate(max_date : DateTime) = 
        context.Set<PriceRecordItem>() |> Seq.filter (fun x -> x.Date <= max_date)
        |> List.ofSeq
        |> PackListSomeOrNone

    member this.GetByMinDate(min_date : DateTime) = 
        context.Set<PriceRecordItem>() |> Seq.filter (fun x -> x.Date <= min_date)
        |> List.ofSeq
        |> PackListSomeOrNone

    member this.GetByDateRange(min_date : DateTime, max_date : DateTime) = 
        (this.GetByMinDate(min_date) |> fun (x) -> match x with | None -> [] | Some(x) -> x) @ (this.GetByMaxDate(max_date) |> UnpackListSomeOrNone ) |> List.distinct |> PackListSomeOrNone

    member this.GetByMaxQuantity(max_quantity : int) = 
        context.Set<PriceRecordItem>() |> Seq.filter (fun x -> x.Quantity <= max_quantity) |> List.ofSeq |> PackListSomeOrNone

    member this.GetByMinQuantity(min_quantity : int) = 
        context.Set<PriceRecordItem>() |> Seq.filter (fun x -> x.Quantity >= min_quantity) |> List.ofSeq |> PackListSomeOrNone

    member this.GetAllByDate() = 
        context.Set<PriceRecordItem>()
        |> Seq.groupBy (fun x -> x.SubProvider) //Will add different records under a same subprovider
        |> Seq.map (fun (key, values) -> (key,  values |> Seq.map (fun x -> (x.Date, x.Price) )))
        |> Seq.map (fun (k,t) -> (k, t |> Seq.sortBy (fun (a,b) -> a ) |> List.ofSeq))
        |> List.ofSeq
        |> PackListSomeOrNone