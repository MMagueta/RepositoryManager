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

    let GetPriceRecordsByProviders providers_id =
        providers_id |> uow.GetPriceRecordsByProviders |> match_pattern

    let GetByDateRange (min_date, max_date) = 
        (uow.GetByDateRange (min_date |> System.DateTime.Parse, max_date |> System.DateTime.Parse)) |> match_pattern

    let FilterByMinDate (min_date) = 
        min_date |> System.DateTime.Parse |> uow.FilterByMinDate |> match_pattern

    let FilterByMaxDate (max_date) = 
        max_date |> System.DateTime.Parse |> uow.FilterByMaxDate |> match_pattern