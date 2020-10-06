namespace Repository

open System
open System.Collections.Generic
open System.Linq.Expressions

type IRepository = 
    abstract member Get : id : int -> 'a
    abstract member Add : model : 'a -> unit