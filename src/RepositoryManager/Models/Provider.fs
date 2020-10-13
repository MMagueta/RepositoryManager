namespace Models

open System.ComponentModel.DataAnnotations

[<CLIMutable>]
type ProviderItem =
    {
        Id : int
        [<Required>]
        mutable Name : string
    }

    member this.Errors(update : bool) =
        if this.Id <> 0 && not update then Some "ID must be zero to be registred"
        else None