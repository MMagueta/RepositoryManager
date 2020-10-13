namespace Providers

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

    let FindProviderById(id : int) = 
        id |> uow.FindProviderById |> json

    //----------------------- POST -----------------------
    let InsertProvider() = 
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let! new_provider = ctx.BindJsonAsync<ProviderItem>()
                let insertionProcedure(registry) =
                    match uow.InsertProvider(registry) with
                    | Some msg -> RequestErrors.BAD_REQUEST msg
                    | None -> Successful.OK registry
                return! (
                    match new_provider.Errors(false) with 
                    | Some msg -> RequestErrors.BAD_REQUEST msg
                    | None -> insertionProcedure(new_provider)
                ) next ctx
            }
    //----------------------- PUT -----------------------
    let UpdateProvider() = 
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let! updated_provider = ctx.BindJsonAsync<ProviderItem>()
                if not( isNull (box (uow.Providers.GetById<ProviderItem>(updated_provider.Id)))) then
                    let updateProcedure(registry) =
                        match uow.UpdateProvider(registry) with
                        | Some msg -> RequestErrors.BAD_REQUEST msg
                        | _ -> Successful.OK "Record updated." 
                    return! (
                        match updated_provider.Errors(true) with 
                        | Some msg -> RequestErrors.BAD_REQUEST msg
                        | None -> updateProcedure(updated_provider)
                    ) next ctx
                else
                    return! RequestErrors.BAD_REQUEST "Old record not found." next ctx
            }

    //----------------------- DELETE -----------------------

    let DeleteProvider(id : int) = 
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let result =
                    try 
                        let provider = uow.Providers.GetById<ProviderItem>(id)
                        match uow.GetPriceRecordsByProviders(provider.Id) with
                        | Some price_records-> price_records |> List.iter (fun x -> uow.Pricerecords.Remove<PriceRecordItem>(x) |> ignore)
                        | None -> "" |> ignore //Must verify the condition, but has no use in this case
                        uow.Providers.Remove<ProviderItem>(provider) |> ignore
                        uow.Complete()
                        None
                    with 
                        err -> Some("Error on deleting records.")
                match result with 
                | Some msg -> return! RequestErrors.BAD_REQUEST msg next ctx
                | None -> return! Successful.OK "Record deleted in cascade." next ctx
            }