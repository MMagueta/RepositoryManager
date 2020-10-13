namespace CurrencyPair

open Saturn
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive
open Giraffe
open FSharp.Control.Tasks.V2.ContextInsensitive
open Repository
open Context
open Models
open Repository.UnitOfWork

module Controller =

    let FindCPairById(id : int) = 
        id |> uow.FindCPairById |> json

    //----------------------- POST -----------------------
    let InsertCurrencyPair() = 
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let! new_cpair = ctx.BindJsonAsync<CurrencyPairItem>()
                let insertionProcedure(registry) =
                    match uow.InsertCurrencyPair(registry) with
                    | Some msg -> RequestErrors.BAD_REQUEST msg
                    | None -> Successful.OK registry
                return! (
                    match new_cpair.Errors(false) with 
                    | Some msg -> RequestErrors.BAD_REQUEST msg
                    | None -> insertionProcedure(new_cpair)
                ) next ctx
            }
    //----------------------- PUT -----------------------
    let UpdateCurrencyPair() = 
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let! updated_cpair = ctx.BindJsonAsync<CurrencyPairItem>()
                if not( isNull (box (uow.Providers.GetById<CurrencyPairItem>(updated_cpair.Id)))) then
                    let updateProcedure(registry) =
                        match uow.UpdateCurrencyPair(registry) with
                        | Some msg -> RequestErrors.BAD_REQUEST msg
                        | _ -> Successful.OK "Record updated." 
                    return! (
                        match updated_cpair.Errors(true) with 
                        | Some msg -> RequestErrors.BAD_REQUEST msg
                        | None -> updateProcedure(updated_cpair)
                    ) next ctx
                else
                    return! RequestErrors.BAD_REQUEST "Old record not found." next ctx
            }

    //----------------------- DELETE -----------------------

    let DeleteCurrencyPair(id : int) = 
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let result =
                    try 
                        let crpair = uow.CRPairs.GetById<CurrencyPairItem>(id)
                        match uow.GetPriceRecordsByCPairs(crpair.Id) with
                        | Some price_records-> price_records |> List.iter (fun x -> uow.Pricerecords.Remove<PriceRecordItem>(x) |> ignore)
                        | None -> "" |> ignore //Must verify the condition, but has no use in this case
                        uow.CRPairs.Remove<CurrencyPairItem>(crpair) |> ignore
                        uow.Complete()
                        None
                    with 
                        err -> Some("Error on deleting records.")
                match result with 
                | Some msg -> return! RequestErrors.BAD_REQUEST msg next ctx
                | None -> return! Successful.OK "Record deleted in cascade." next ctx
            }