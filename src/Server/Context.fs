namespace Context

open Microsoft.EntityFrameworkCore
open Models

type RepositoryContext(options : DbContextOptions<RepositoryContext>) = 
    inherit DbContext(options)
    
    [<DefaultValue>]
    val mutable Providers : DbSet<ProviderItem>
    member public this._Providers          with    get()      = this.Providers 
                                           and     set value  = this.Providers <- value 

    [<DefaultValue>]
    val mutable CPairs : DbSet<CurrencyPairItem>
    member public this._CPairs             with    get()      = this.CPairs 
                                           and     set value  = this.CPairs <- value 

    [<DefaultValue>]
    val mutable PriceRecords : DbSet<PriceRecordItem>
    member public this._PriceRecords       with    get()      = this.PriceRecords
                                           and     set value  = this.PriceRecords <- value 

    member this.GetProvider (id:int) = this.Providers.Find(id)
    member this.GetCPair (id:int) = this.CPairs.Find(id)
    member this.GetPriceRecord (id:int) = this.PriceRecords.Find(id)
