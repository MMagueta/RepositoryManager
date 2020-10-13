namespace Repository

open Saturn

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