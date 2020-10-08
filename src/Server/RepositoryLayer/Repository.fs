namespace Repository

open System
open System.Collections.Generic
open Microsoft.EntityFrameworkCore
open System.Linq
open System.Linq.Expressions

open Context

type Repository(contextIn : DbContext) = 
    let context = contextIn

    member this.GetById<'T>(id : int) = context.Set<'T>().Find(id)
    member this.Add<'T>(object : 'T) = context.Set<'T>().Add(object)
    member this.Remove<'T>(object : 'T) = context.Set<'T>().Remove(object)
    