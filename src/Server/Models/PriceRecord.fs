namespace Models

open System.ComponentModel.DataAnnotations

open Giraffe

[<CLIMutable>]
type PriceRecordItem =
    {
        Id : int
        [<Required>]
        mutable Date : System.DateTime
        [<Required>]
        mutable Price : float
        [<Required>]
        mutable Quantity : int
        [<Required>]
        mutable Provider : ProviderItem
        [<Required>]
        mutable CPair : CurrencyPairItem
        [<Required>]
        mutable SubProvider : string
    }
    member this.Errors(update : bool) =
        if this.Id <> 0 && not update then Some("ID must be zero to be registred")
        else None

