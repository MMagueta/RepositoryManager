namespace Provider

open Saturn
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive
open FSharp.Collections
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

    let FilterByMaxQuantity (max_qtd) = 
        max_qtd |> uow.FilterByMaxQuantity |> match_pattern

    let FilterByMinQuantity (min_qtd) = 
        min_qtd |> uow.FilterByMinQuantity |> match_pattern

    let FilterByMaxDate (max_date) = 
        max_date |> System.DateTime.Parse |> uow.FilterByMaxDate |> match_pattern

    let GetAllPricesInOrderByDate (min_date, max_date) = 
        (uow.GetAllPricesInOrderByDate (min_date |> System.DateTime.Parse, max_date |> System.DateTime.Parse)) |> match_pattern

    let GetMarketData(min_date, max_date, currency_pair_id, provider_id) = 
        (uow.GetMarketData (min_date |> System.DateTime.Parse, max_date |> System.DateTime.Parse, provider_id, currency_pair_id)) |> match_pattern