namespace CurrencyPair

open Saturn
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive
open FSharp.Collections
open Giraffe
open FSharp.Control.Tasks.V2.ContextInsensitive
open Repository
open Context
open Models
open Repository.UnitOfWork

module Controller =


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
                    match new_cpair.Errors() with 
                    | Some msg -> RequestErrors.BAD_REQUEST msg
                    | None -> insertionProcedure(new_cpair)
                ) next ctx
            }