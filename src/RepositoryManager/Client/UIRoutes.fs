module UIRoutes

open Saturn
open Giraffe
open GiraffeViewEngine
open Repository.UnitOfWork
open Models
open Startup
open Zanaptak.TypedCssClasses
open XPlot.Plotly

Seed(ctx)

//Instance to make access to the style easier
type Bulma = CssClasses<"https://cdnjs.cloudflare.com/ajax/libs/bulma/0.9.1/css/bulma.min.css">

let createPage title subtitle content = 
    html [] [
        head [] [
            link [ _rel "stylesheet"; _href "https://cdnjs.cloudflare.com/ajax/libs/bulma/0.9.1/css/bulma.min.css"]
        ]
        body [] [
            section [ _class (Bulma.hero + " " + Bulma.``is-primary``)] [
                div [ _class Bulma.``hero-body``] [
                    div [ _class Bulma.container] [
                        h1 [ _class Bulma.title ] [ Text title]
                        h2 [ _class Bulma.subtitle ] [ Text subtitle]
                    ]
                ]
            ]
            section [ _class Bulma.section] [
                div [_class Bulma.container] content
            ]
        ]
    ]

//Creates a plot with the data gathered by the id of the provider and currency pair, a line for each category from a provider
let pricesPlotView(provider_id : int, cpair_id : int) = 
    
    createPage "Market Data" ("Provider: " + uow.Providers.GetById<ProviderItem>(provider_id).Name + " Currency: " + uow.CRPairs.GetById<CurrencyPairItem>(cpair_id).Label) [
            a [  _href ("http://localhost:8085/data/table/" + provider_id.ToString() + "/" + cpair_id.ToString())] [
                button [ _class "button is-link columns is-mobile is-centered"; _style "margin-left: 50%" ] [ Text "Table"]
            ]
            
            div [ _style "padding: 5%"; _class "columns is-mobile is-centered" ] [
                match uow.GetMarketData(System.DateTime.Now.AddDays(-1.0), System.DateTime.Now.AddDays(9.0), provider_id, cpair_id) with
                | Some result -> 
                    let plot = 
                        let mutable data = []
                        for entry in (result |> Map.ofList) do
                            entry.Value
                            |> List.map (fun data -> (data.Date, data.Price))
                            |> List.unzip
                            |> (fun (a,b) -> Scatter(x = a, y = b, name = entry.Key))
                            |> (fun x -> data <- data @ [x])
                            |> ignore
                        data
                        |> Chart.Plot
                    plot.GetHtml() |> Text
                | None -> h1 [ _class Bulma.title ] [ Text "No data." ]
            ]
    ]

//Creates a table with the data gathered by the id of the provider and currency pair
let pricesTableView(provider_id : int, cpair_id : int) = 
    createPage "Market Data" ("Provider: " + uow.Providers.GetById<ProviderItem>(provider_id).Name + " Currency: " + uow.CRPairs.GetById<CurrencyPairItem>(cpair_id).Label) [
        a [  _href ("http://localhost:8085/data/plot/" + provider_id.ToString() + "/" + cpair_id.ToString())] [
            button [ _class "button is-link columns is-mobile is-centered"; _style "margin-left: 50%"] [ Text "Plot"]
        ]
        table [ _style "margin-top: 5%"; _class "table is-mobile is-centered is-fullwidth is-hoverable is-hoverable" ] [
            thead [] [
                tr [] [
                    th [] [ Text "Provider" ]
                    th [] [ Text "Currency" ]
                    th [] [ Text "Date" ]
                    th [] [ Text "Price" ]
                    th [] [ Text "Quantity" ]
                    th [] [ Text "Category" ]
                ]
            ]
            tbody [] [
                for row in (((uow.GetPriceRecordsByProviders(provider_id) |> uow.UnpackListSomeOrNone) @ (uow.GetPriceRecordsByCPairs(cpair_id) |> uow.UnpackListSomeOrNone)) |> List.distinct) do
                    tr [] [
                        td [] [ Text (row.Provider.Name.ToString()) ]
                        td [] [ Text (row.CPair.Label.ToString()) ]
                        td [] [ Text (row.Date.ToString()) ]
                        td [] [ Text (row.Price.ToString()) ]
                        td [] [ Text (row.Quantity.ToString()) ]
                        td [] [ Text (row.SubProvider.ToString()) ]
                    ]
            ]
        ]
    ]

let plot(provider_id : int, cpair_id : int) = 
    (htmlView (pricesPlotView(provider_id, cpair_id)))

let table(provider_id : int, cpair_id : int) = 
    (htmlView (pricesTableView(provider_id, cpair_id)))

let uiRouter = router { 
    getf "/plot/%i/%i" plot
    getf "/table/%i/%i" table
}