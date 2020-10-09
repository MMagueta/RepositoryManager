namespace Providers

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
    let InsertProvider() = 
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let! new_provider = ctx.BindJsonAsync<ProviderItem>()
                let insertionProcedure(registry) =
                    match uow.InsertProvider(registry) with
                    | Some msg -> RequestErrors.BAD_REQUEST msg
                    | None -> Successful.OK registry
                return! (
                    match new_provider.Errors() with 
                    | Some msg -> RequestErrors.BAD_REQUEST msg
                    | None -> insertionProcedure(new_provider)
                ) next ctx
            }