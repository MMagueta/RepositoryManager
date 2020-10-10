namespace Repository

open System
open System.Collections.Generic
open System.Linq
open System.Linq.Expressions

type IRepository<'T> = 
    abstract GetById : int -> 'T
    abstract Add : 'T -> unit
    abstract Remove : 'T -> unit
    abstract Where : Expression<Func<'T, bool>> -> IQueryable<'T>