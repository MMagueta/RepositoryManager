namespace Models

open System.ComponentModel.DataAnnotations

[<CLIMutable>]
type CurrencyPairItem =
    {
        Id : int
        [<Required>]
        mutable Label : string

    }
    //No other validation needed since all the data is treated in the controllers 
    member this.Errors(update : bool) =
        if this.Id <> 0 && not update then Some "ID must be zero to be registred"
        else None