namespace Models

open System.ComponentModel.DataAnnotations

[<CLIMutable>]
type CurrencyPairItem =
    {
        Id : int
        [<Required>]
        Label : string

    }

    member this.Errors() =
        if this.Id <> 0 then Some "ID must be zero to be registred"
        else None