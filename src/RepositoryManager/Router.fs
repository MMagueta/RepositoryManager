module Router

open Saturn
open Giraffe

open Providers.Controller
open CurrencyPair.Controller
open PriceRecords.Controller
open UIRoutes

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