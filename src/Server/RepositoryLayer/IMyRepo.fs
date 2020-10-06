namespace Repository

open System
open System.Collections.Generic
open System.Linq.Expressions

open Models

type IMyRepo = 
    inherit IRepository<ProviderItem>