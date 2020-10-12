namespace Repository

open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Hosting

open Giraffe
open Saturn

open Models
open Context
open PriceRecords.Controller
open CurrencyPair.Controller
open Providers.Controller
open Repository.UnitOfWork

(*
    http://localhost:8085/
*)

module Server = 
     
    let webApp =
        choose [
            GET >=>
                choose [
                    routef "/prices/bycpair/%i" FindPriceRecordsByCPairs
                    routef "/prices/byprovider/%i" GetPriceRecordsByProviders
                    routef "/prices/bydaterange/%s/%s" GetByDateRange
                    routef "/prices/bymaxdate/%s" FilterByMaxDate
                    routef "/prices/bymindate/%s" FilterByMinDate
                    routef "/prices/subprovider/%s/%s"  GetAllPricesInOrderByDate
                    routef "/prices/bymaxquantity/%i" FilterByMaxQuantity
                    routef "/prices/byminquantity/%i" FilterByMinQuantity
                    routef "/prices/marketdata/min_date=%s/max_date=%s/provider=%i/pair=%i" GetMarketData

                    routef "/cpairs/byid/%i" FindCPairById

                    routef "/provider/byid/%i" FindProviderById
                ]
            POST >=>
                choose [
                    route "/prices/allrecords/" >=> FilterAllRecords()
                    route "/prices/" >=> InsertPriceRecord()
                    route "/providers/" >=> InsertProvider()
                    route "/currencypairs/" >=> InsertCurrencyPair()

                ]
            PUT >=>
                choose [
                    route "/prices/" >=> UpdatePriceRecord()
                    route "/providers/" >=> UpdateProvider()
                    route "/currencypairs/" >=> UpdateCurrencyPair()

                ]
            DELETE >=>
                choose [
                    routef "/prices/%i" DeletePriceRecord
                    //routef "/providers/%i" DeleteProvider
                    //routef "/currencypairs/%i" DeleteCurrencyPair

                ]
            setStatusCode 404 >=> text "Not Found" ]
    
    
    let app =
        application {
            url "http://0.0.0.0:8085"
            use_router webApp
            memory_cache
            use_static "public"
            use_json_serializer (Thoth.Json.Giraffe.ThothSerializer())
            use_gzip
        }

    let exitCode = 0

    let init(context : RepositoryContext) = 
        context.Database.EnsureDeleted() |> ignore
        context.Database.EnsureCreated() |> ignore
        
        let tdItems : ProviderItem[] = 
            [|
                { Id = 0; Name = "A" }
                { Id = 0; Name = "B" }
                { Id = 0; Name = "C" }
            |]

        context.Providers.AddRange(tdItems) |> ignore
        let tdItems : CurrencyPairItem[] = 
            [|
                { Id = 0; Label = "A/B" }
                { Id = 0; Label = "Z/H" }
            |]

        context.CPairs.AddRange(tdItems) |> ignore
        context.SaveChanges() |> ignore 
        let prov = ProvidersRepository(context)
        let pairs = CurrencyPairsRepository(context)
        let d1 = System.DateTime.Parse "1-1-2011"
        let d2 = System.DateTime.Parse "1-2-2011"
        let d3 = System.DateTime.Parse "3-2-2011"
        let d4 = System.DateTime.Parse "1-3-2011"
        let tdItems : PriceRecordItem[] = 
            [|
                { Id = 0; Date = d2; Price = 1.4213; Quantity = 1238; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "vwap" }
                { Id = 0; Date = d1; Price = 1.5213; Quantity = 1238; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "vwap" }
                { Id = 0; Date = d4; Price = 1.6213; Quantity = 1238; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "vwap" }
                { Id = 0; Date = d3; Price = 0.9536; Quantity = 1238; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "overall" }
            |]

        context.PriceRecords.AddRange(tdItems) |> ignore
        context.SaveChanges() |> ignore 

    [<EntryPoint>]
    let main args =
        init(ctx)
        run app
        exitCode