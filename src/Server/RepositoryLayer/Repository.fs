namespace Repository

open System
open System.Collections.Generic
open Microsoft.EntityFrameworkCore
open System.Linq
open System.Linq.Expressions

open Context

type Repository(contextIn : DbContext) = 
    let context = contextIn

    member this.Get<'T>(id : int) = context.Set<'T>().Find(id)
