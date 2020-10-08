namespace Provider

open Saturn
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive

module Controller =
    let indexAction (ctx : HttpContext) =
        task {
            return [1;2;3]
        }

    let resource = controller {
      index indexAction
    }