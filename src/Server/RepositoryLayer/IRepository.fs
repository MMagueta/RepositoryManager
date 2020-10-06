namespace Repository

open System
open System.Collections.Generic
open System.Linq.Expressions

type IRepository<'T> = 
    abstract Get : int -> 'T
    // abstract Add : 'T -> unit