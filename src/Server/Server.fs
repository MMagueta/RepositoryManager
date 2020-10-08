namespace Repository

open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Hosting

open Giraffe
open Saturn

open Shared

(*
    http://localhost:8085/
*)
module Server = 

    let webApp =
        router {
            get "/" (["A";"B";"C"] |> json)
            forward "/providers" Provider.Controller.resource
        }

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

    [<EntryPoint>]
    let main args =
        run app
        exitCode