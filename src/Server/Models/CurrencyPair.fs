namespace Models

open System.ComponentModel.DataAnnotations

[<CLIMutable>]
type CurrencyPairItem =
    {
        Id : int
        [<Required>]
        Label : string

    }