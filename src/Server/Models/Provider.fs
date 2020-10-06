namespace Models

open System.ComponentModel.DataAnnotations

[<CLIMutable>]
type ProviderItem =
    {
        Id : int
        [<Required>]
        Name : string
    }