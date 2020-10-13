module Startup
open Context
open Models
open Repository

let Seed(context : RepositoryContext) = 
    
    let records : ProviderItem[] = 
        [|
            { Id = 0; Name = "A" }
            { Id = 0; Name = "B" }
            { Id = 0; Name = "C" }
        |]

    context.Providers.AddRange(records) |> ignore
    let records : CurrencyPairItem[] = 
        [|
            { Id = 0; Label = "USD/BRL" }
            { Id = 0; Label = "EUR/JPY" }
        |]

    context.CPairs.AddRange(records) |> ignore
    context.SaveChanges() |> ignore 
    let prov = ProvidersRepository(context)
    let pairs = CurrencyPairsRepository(context)

    let records : PriceRecordItem[] = 
        [|
            { Id = 0; Date = System.DateTime.Now.AddDays(1.0); Price = 1.4213; Quantity = 1238; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "vwap" }
            { Id = 0; Date = System.DateTime.Now.AddDays(2.0); Price = 1.5213; Quantity = 1235; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "vwap" }
            { Id = 0; Date = System.DateTime.Now.AddDays(3.0); Price = 1.6213; Quantity = 2324; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "vwap" }
            { Id = 0; Date = System.DateTime.Now.AddDays(4.0); Price = 1.4213; Quantity = 1235; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "vwap" }
            { Id = 0; Date = System.DateTime.Now.AddDays(5.0); Price = 1.5213; Quantity = 5321; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "vwap" }
            { Id = 0; Date = System.DateTime.Now.AddDays(6.0); Price = 1.6213; Quantity = 1235; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "vwap" }
            { Id = 0; Date = System.DateTime.Now.AddDays(7.0); Price = 1.4213; Quantity = 4535; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "vwap" }
            { Id = 0; Date = System.DateTime.Now.AddDays(8.0); Price = 1.5213; Quantity = 2354; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "vwap" }
            { Id = 0; Date = System.DateTime.Now.AddDays(9.0); Price = 1.6213; Quantity = 412; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "vwap" }

            { Id = 0; Date = System.DateTime.Now.AddDays(1.0); Price = 1.6433; Quantity = 1238; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "feed" }
            { Id = 0; Date = System.DateTime.Now.AddDays(2.0); Price = 2.4324; Quantity = 1235; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "feed" }
            { Id = 0; Date = System.DateTime.Now.AddDays(3.0); Price = 2.2222; Quantity = 2324; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "feed" }
            { Id = 0; Date = System.DateTime.Now.AddDays(4.0); Price = 2.5676; Quantity = 1235; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "feed" }
            { Id = 0; Date = System.DateTime.Now.AddDays(5.0); Price = 2.4321; Quantity = 5321; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "feed" }
            { Id = 0; Date = System.DateTime.Now.AddDays(6.0); Price = 2.1532; Quantity = 1235; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "feed" }
            { Id = 0; Date = System.DateTime.Now.AddDays(7.0); Price = 2.2134; Quantity = 4535; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "feed" }
            { Id = 0; Date = System.DateTime.Now.AddDays(8.0); Price = 2.1234; Quantity = 2354; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "feed" }
            { Id = 0; Date = System.DateTime.Now.AddDays(9.0); Price = 2.6421; Quantity = 412; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "feed" }

            { Id = 0; Date = System.DateTime.Now.AddDays(1.0); Price = 3.1235; Quantity = 1235; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "overall" }
            { Id = 0; Date = System.DateTime.Now.AddDays(2.0); Price = 0.9545; Quantity = 8454; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "overall" }
            { Id = 0; Date = System.DateTime.Now.AddDays(3.0); Price = 1.2345; Quantity = 2324; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "overall" }
            { Id = 0; Date = System.DateTime.Now.AddDays(4.0); Price = 1.3246; Quantity = 1235; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "overall" }
            { Id = 0; Date = System.DateTime.Now.AddDays(5.0); Price = 1.5213; Quantity = 5321; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "overall" }
            { Id = 0; Date = System.DateTime.Now.AddDays(6.0); Price = 1.654; Quantity = 1235; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "overall" }
            { Id = 0; Date = System.DateTime.Now.AddDays(7.0); Price = 1.5466; Quantity = 4535; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "overall" }
            { Id = 0; Date = System.DateTime.Now.AddDays(8.0); Price = 1.2434; Quantity = 2354; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "overall" }
            { Id = 0; Date = System.DateTime.Now.AddDays(9.0); Price = 4.2345; Quantity = 412; Provider = prov.GetById<ProviderItem>(1); CPair = pairs.GetById<CurrencyPairItem>(1); SubProvider = "overall" }


            { Id = 0; Date = System.DateTime.Now.AddDays(1.0); Price = 1.1234; Quantity = 6433; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "vwap" }
            { Id = 0; Date = System.DateTime.Now.AddDays(2.0); Price = 1.6374; Quantity = 3532; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "vwap" }
            { Id = 0; Date = System.DateTime.Now.AddDays(3.0); Price = 1.6213; Quantity = 2324; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "vwap" }
            { Id = 0; Date = System.DateTime.Now.AddDays(4.0); Price = 1.4213; Quantity = 1235; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "vwap" }
            { Id = 0; Date = System.DateTime.Now.AddDays(5.0); Price = 1.5643; Quantity = 5321; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "vwap" }
            { Id = 0; Date = System.DateTime.Now.AddDays(6.0); Price = 1.6213; Quantity = 1235; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "vwap" }
            { Id = 0; Date = System.DateTime.Now.AddDays(7.0); Price = 1.6547; Quantity = 4535; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "vwap" }
            { Id = 0; Date = System.DateTime.Now.AddDays(8.0); Price = 1.5213; Quantity = 2354; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "vwap" }
            { Id = 0; Date = System.DateTime.Now.AddDays(9.0); Price = 1.543; Quantity = 412; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "vwap" }

            { Id = 0; Date = System.DateTime.Now.AddDays(1.0); Price = 1.3455; Quantity = 1238; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "feed" }
            { Id = 0; Date = System.DateTime.Now.AddDays(2.0); Price = 2.6543; Quantity = 1235; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "feed" }
            { Id = 0; Date = System.DateTime.Now.AddDays(3.0); Price = 2.7543; Quantity = 2324; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "feed" }
            { Id = 0; Date = System.DateTime.Now.AddDays(4.0); Price = 2.5676; Quantity = 1235; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "feed" }
            { Id = 0; Date = System.DateTime.Now.AddDays(5.0); Price = 4.4321; Quantity = 5321; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "feed" }
            { Id = 0; Date = System.DateTime.Now.AddDays(6.0); Price = 1.1532; Quantity = 1235; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "feed" }
            { Id = 0; Date = System.DateTime.Now.AddDays(7.0); Price = 0.2134; Quantity = 4535; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "feed" }
            { Id = 0; Date = System.DateTime.Now.AddDays(8.0); Price = 0.1234; Quantity = 2354; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "feed" }
            { Id = 0; Date = System.DateTime.Now.AddDays(9.0); Price = 0.6421; Quantity = 412; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "feed" }

            { Id = 0; Date = System.DateTime.Now.AddDays(1.0); Price = 3.4345; Quantity = 1235; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "overall" }
            { Id = 0; Date = System.DateTime.Now.AddDays(2.0); Price = 0.1344; Quantity = 8454; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "overall" }
            { Id = 0; Date = System.DateTime.Now.AddDays(3.0); Price = 1.4324; Quantity = 2324; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "overall" }
            { Id = 0; Date = System.DateTime.Now.AddDays(4.0); Price = 2.5432; Quantity = 1235; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "overall" }
            { Id = 0; Date = System.DateTime.Now.AddDays(5.0); Price = 6.4534; Quantity = 5321; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "overall" }
            { Id = 0; Date = System.DateTime.Now.AddDays(6.0); Price = 1.3455; Quantity = 1235; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "overall" }
            { Id = 0; Date = System.DateTime.Now.AddDays(7.0); Price = 1.5466; Quantity = 4535; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "overall" }
            { Id = 0; Date = System.DateTime.Now.AddDays(8.0); Price = 1.2434; Quantity = 2354; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "overall" }
            { Id = 0; Date = System.DateTime.Now.AddDays(9.0); Price = 4.2345; Quantity = 412; Provider = prov.GetById<ProviderItem>(2); CPair = pairs.GetById<CurrencyPairItem>(2); SubProvider = "overall" }
        |]

    context.PriceRecords.AddRange(records) |> ignore
    context.SaveChanges() |> ignore 
