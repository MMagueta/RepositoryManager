// module Server

// open Saturn
// open Config

// let endpointPipe = pipeline {
//     plug head
//     plug requestId
// }

// let app = application {
//     pipe_through endpointPipe

//     error_handler (fun ex _ -> pipeline { render_html (InternalError.layout ex) })
//     use_router Router.appRouter
//     url "http://0.0.0.0:8085/"
//     memory_cache
//     use_static "static"
//     use_gzip
//     use_config (fun _ -> {connectionString = "DataSource=database.sqlite"} ) //TODO: Set development time configuration
// }

// [<EntryPoint>]
// let main _ =
//     printfn "Working directory - %s" (System.IO.Directory.GetCurrentDirectory())
//     run app
//     0 // return an integer exit code

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

module Server = 
    
    let app =
        application {
            url "http://0.0.0.0:8085"
            use_router Router.appRouter
            memory_cache
            use_static "public"
            use_json_serializer (Thoth.Json.Giraffe.ThothSerializer())
            use_gzip
        }

    let exitCode = 0

    [<EntryPoint>]
    let main args = 
        run app
        exitCode