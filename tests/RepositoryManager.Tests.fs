module RepositoryManager.Tests

open Expecto
open Repository.UnitOfWork

let repositoryLayer = testList "Repository Layer Controller" [
    testCase "Adding a valid " <| fun _ ->
        Expect.isSome (uow.Pricerecords.GetAllOrderedInRange(System.DateTime.Now.AddDays(-1.0), System.DateTime.Now.AddDays(0.0))) "Result should be empty but Some"
        Expect.isSome (uow.GetMarketData(System.DateTime.Now.AddDays(-1.0), System.DateTime.Now.AddDays(0.0), 1, 1)) "Result should be empty but Some"
        Expect.hasLength (uow.GetAllPricesInOrderByDate(System.DateTime.Now.AddDays(-1.0), System.DateTime.Now.AddDays(1.0)) |> uow.UnpackListSomeOrNone|>Seq.ofList) 3 "Only 3 subcategories inserted, they should group"
        
        //I could keep going, but since I did not follow the TDD logic, a depth test would be the best for this scenario (to evaluate the multiple components scattered in the app), however I don't really have time at the moment and testing JSON results this way is time consuming
        //Regardless, this library is interesting, it has functions I wasn't even aware of in other libraries, such as Expect.sequenceContainsOrder
]

let all =
    testList "All"
        [
            repositoryLayer
        ]

[<EntryPoint>]
let main _ = runTests defaultConfig all