namespace PriceRecords

open Microsoft.AspNetCore.Http
open Giraffe
open FSharp.Control.Tasks.V2.ContextInsensitive
open Repository
open Models
open Repository.UnitOfWork

module Controller =
    let MatchPattern(matcher)= 
        match matcher with
        | Some(record) -> record |> json
        | None -> setStatusCode 404 >=> text "Records not found"

    let FindPriceRecordsByCPairs(cpairs_id : int) =
        cpairs_id |> uow.GetPriceRecordsByCPairs |> MatchPattern

    let GetPriceRecordsByProviders(providers_id : int) =
        providers_id |> uow.GetPriceRecordsByProviders |> MatchPattern

    let GetByDateRange(min_date : string, max_date : string) = 
        (uow.GetByDateRange (min_date |> System.DateTime.Parse, max_date |> System.DateTime.Parse)) |> MatchPattern

    let FilterByMinDate(min_date : string) = 
        min_date |> System.DateTime.Parse |> uow.FilterByMinDate |> MatchPattern

    let FilterByMaxQuantity(max_qtd : int) = 
        max_qtd |> uow.FilterByMaxQuantity |> MatchPattern

    let FilterByMinQuantity(min_qtd : int) = 
        min_qtd |> uow.FilterByMinQuantity |> MatchPattern

    let FilterByMaxDate(max_date : string) = 
        max_date |> System.DateTime.Parse |> uow.FilterByMaxDate |> MatchPattern

    let GetAllPricesInOrderByDate(min_date : string, max_date : string) = 
        (uow.GetAllPricesInOrderByDate (min_date |> System.DateTime.Parse, max_date |> System.DateTime.Parse)) |> MatchPattern

    let GetMarketData(min_date : string, max_date : string, currency_pair_id : int, provider_id :int) = 
        (uow.GetMarketData (min_date |> System.DateTime.Parse, max_date |> System.DateTime.Parse, provider_id, currency_pair_id)) |> MatchPattern

    let FilterAllRecords() = 
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let! filters = ctx.BindJsonAsync<Filters>()

                let filterProcedure(registry) =
                    match uow.FilterRecords(filters) with 
                    | None -> RequestErrors.BAD_REQUEST "No records found."
                    | Some result -> Successful.OK result
                return! (
                    match filters.tryParse with 
                    | Some msg -> RequestErrors.BAD_REQUEST msg
                    | None -> filterProcedure(filters)
                ) next ctx
            }


    //----------------------- POST -----------------------

    let InsertPriceRecord() = 
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let! new_price_record = ctx.BindJsonAsync<PriceRecordItem>()
                let insertionProcedure(registry) =
                    match uow.InsertPriceRecord(registry) with
                    | Some msg -> RequestErrors.BAD_REQUEST msg
                    | None -> Successful.OK registry
                return! (
                    match new_price_record.Errors(false) with 
                    | Some msg -> RequestErrors.BAD_REQUEST msg
                    | None -> insertionProcedure(new_price_record)
                ) next ctx
            }

    //----------------------- PUT -----------------------

    let UpdatePriceRecord() = 
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let! updated_price_record = ctx.BindJsonAsync<PriceRecordItem>()
                if not( isNull (box (uow.Pricerecords.GetById<PriceRecordItem>(updated_price_record.Id)))) then
                    let updateProcedure(registry) =
                        match uow.UpdatePriceRecord(registry) with
                        | Some msg -> RequestErrors.BAD_REQUEST msg
                        | _ -> Successful.OK "Record updated." 
                    return! (
                        match updated_price_record.Errors(true) with 
                        | Some msg -> RequestErrors.BAD_REQUEST msg
                        | None -> updateProcedure(updated_price_record)
                    ) next ctx
                else
                    return! RequestErrors.BAD_REQUEST "Old record not found." next ctx
            }

    //----------------------- DELETE -----------------------

    let DeletePriceRecord(id : int) = 
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let result =
                    try 
                        uow.Pricerecords.Remove<PriceRecordItem>(uow.Pricerecords.GetById<PriceRecordItem>(id)) |> ignore
                        uow.Complete()
                        None
                    with 
                        err -> Some("Error on deleting record.")
                match result with 
                | Some msg -> return! RequestErrors.BAD_REQUEST msg next ctx
                | None -> return! Successful.OK "Record deleted." next ctx
            }