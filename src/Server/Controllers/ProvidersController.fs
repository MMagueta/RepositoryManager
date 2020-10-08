namespace Provider

open Saturn
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive
open Giraffe
open Repository
open Context
open Models
open Repository.UnitOfWork

module Controller =

    let FindPriceRecordsByCPairs cpairs_id =
        match uow.GetPriceRecordsByCPairs(cpairs_id) with
        | Some(record) -> record |> json
        | None -> setStatusCode 404 >=> text "Records not found"