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
    let match_pattern (matcher)= 
        match matcher with
        | Some(record) -> record |> json
        | None -> setStatusCode 404 >=> text "Records not found"

    let FindPriceRecordsByCPairs cpairs_id =
        cpairs_id |> uow.GetPriceRecordsByCPairs |> match_pattern