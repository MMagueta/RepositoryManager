namespace Repository

open System
open System.Collections.Generic
open Microsoft.EntityFrameworkCore
open System.Linq
open System.Linq.Expressions

open Context

type ProvidersRepository(contextIn : DbContext) = 
    let context = contextIn

    member this.GetById<'T>(id : int) = context.Set<'T>().Find(id)
