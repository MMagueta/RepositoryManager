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
    member this.GetByProviders(providers_ids : list<int>) = 
        context.Set<PriceRecordItem>() |> Seq.filter (fun x -> List.contains x.Provider.Id providers_ids)
    member this.GetByMaxDate(max_date : DateTime) = 
        context.Set<PriceRecordItem>() |> Seq.filter (fun x -> x.Date <= max_date)
    member this.GetByMinDate(min_date : DateTime) = 
        context.Set<PriceRecordItem>() |> Seq.filter (fun x -> x.Date >= min_date)
    member this.GetByMaxQuantity(max_quantity : int) = 
        context.Set<PriceRecordItem>() |> Seq.filter (fun x -> x.Quantity <= max_quantity)
    member this.GetByMinQuantity(min_quantity : int) = 
        context.Set<PriceRecordItem>() |> Seq.filter (fun x -> x.Quantity >= min_quantity)
    member this.GetAllByDate = 
        context.Set<PriceRecordItem>()
        |> Seq.groupBy (fun x -> x.SubProvider)
        |> Seq.map (fun (key, values) -> (key, values |> Seq.map (fun x -> x.Date), values |> Seq.map (fun x -> x.Price) ))
        |> Seq.map (fun (key, dates, prices) -> (key, dates |> Seq.sortBy (fun x -> x), prices ))