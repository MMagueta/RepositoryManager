module Router

open Saturn
open Giraffe
open Giraffe.Core
open Giraffe.ResponseWriters

open Providers.Controller
open CurrencyPair.Controller
open PriceRecords.Controller
open Repository.UnitOfWork
open UIRoutes


// let browser = pipeline {
//     plug acceptHtml
//     plug putSecureBrowserHeaders
//     plug fetchSession
//     set_header "x-pipeline-type" "Browser"
// }

// let defaultView = router {
//     get "/" (htmlView Index.layout)
//     get "/index.html" (redirectTo false "/")
//     get "/default.html" (redirectTo false "/")
// }

// let browserRouter = router {
//     not_found_handler (htmlView NotFound.layout) //Use the default 404 webpage
//     pipe_through browser //Use the default browser pipeline

//     forward "" defaultView //Use the default view
// }

//Other scopes may use different pipelines and error handlers

// let api = pipeline {
//     plug acceptJson
//     set_header "x-pipeline-type" "Api"
// }

// let apiRouter = router {
//     not_found_handler (text "Api 404")
//     pipe_through api
//
//     forward "/someApi" someScopeOrController
// }


let webApp =
    choose [
        GET >=>
            choose [
                routef "prices/bycpair/%i" FindPriceRecordsByCPairs
                routef "prices/byprovider/%i" GetPriceRecordsByProviders
                routef "prices/bydaterange/%s/%s" GetByDateRange
                routef "prices/bymaxdate/%s" FilterByMaxDate
                routef "prices/bymindate/%s" FilterByMinDate
                routef "prices/subprovider/%s/%s"  GetAllPricesInOrderByDate
                routef "prices/bymaxquantity/%i" FilterByMaxQuantity
                routef "prices/byminquantity/%i" FilterByMinQuantity
                routef "prices/marketdata/min_date=%s/max_date=%s/provider=%i/pair=%i" GetMarketData

                routef "cpairs/byid/%i" FindCPairById

                routef "provider/byid/%i" FindProviderById
            ]
        POST >=>
            choose [
                route "prices/allrecords/" >=> FilterAllRecords()
                route "prices/" >=> InsertPriceRecord()
                route "providers/" >=> InsertProvider()
                route "currencypairs/" >=> InsertCurrencyPair()

            ]
        PUT >=>
            choose [
                route "prices/" >=> UpdatePriceRecord()
                route "providers/" >=> UpdateProvider()
                route "currencypairs/" >=> UpdateCurrencyPair()

            ]
        DELETE >=>
            choose [
                routef "prices/%i" DeletePriceRecord
                routef "providers/%i" DeleteProvider
                routef "currencypairs/%i" DeleteCurrencyPair

            ]
        setStatusCode 404 >=> text "Not Found" ]

let appRouter = router {
    forward "/data" uiRouter
    forward "/" webApp
}