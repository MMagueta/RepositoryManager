namespace Repository

open System
open Microsoft.EntityFrameworkCore
open System.Linq
open System.Linq.Expressions

type Repository(contextIn : DbContext) = 
    let context = contextIn

    member this.All<'T>() = context.Set<'T>().ToArray()
    member this.GetById<'T>(id : int) = context.Set<'T>().Find(id)
    member this.Add<'T>(object : 'T) = context.Set<'T>().Add(object)
    member this.Remove<'T>(object : 'T) = context.Set<'T>().Remove(object)
    member this.Where<'T>(expression : Expression<Func<'T, bool>>) = context.Set<'T>().Where(expression)