namespace Models

open System.ComponentModel.DataAnnotations

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
        Provider : ProviderItem
        [<Required>]
        CPair : CurrencyPairItem
        [<Required>]
        SubProvider : string
    }