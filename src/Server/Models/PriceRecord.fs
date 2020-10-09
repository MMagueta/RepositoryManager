namespace Models

open System.ComponentModel.DataAnnotations

open Giraffe

[<CLIMutable>]
type PriceRecordItem =
    {
        Id : int
        [<Required>]
        Date : System.DateTime
        [<Required>]
        Price : float
        [<Required>]
        Quantity : int
        [<Required>]
        mutable Provider : ProviderItem
        [<Required>]
        mutable CPair : CurrencyPairItem
        [<Required>]
        SubProvider : string
    }
    member this.Errors() =
        if this.Id <> 0 then Some "ID must be zero to be registred"
        else None

