namespace Repository

open Context

open System.Linq

open Models


type Filters = //Filter type to match the requests, possibly empty
    {
        mutable minDate : string
        mutable maxDate : string
        mutable provider : string
        mutable pairs : list<int>
        mutable maxQuantity : string
        mutable minQuantity : string
    }
    member this.tryParse =
        //Try to cast values and redirects the boolean
        let tryCast(value, parsingFunc) = match (try Some(value |> parsingFunc) with err -> None) with | Some x -> false | None -> true

        //Validation of the input
        if      (tryCast(this.minDate, System.DateTime.Parse) && this.minDate <> "") then (this.minDate <- ""; None)
        else if (tryCast(this.maxDate, System.DateTime.Parse) && this.minDate <> "") then (this.maxDate <- System.DateTime.Now.ToString(); None)
        else if (tryCast(this.provider, int) || this.provider = "") then Some "The Provider ID must be informed."
        else if not (this.pairs |> List.forall (fun x -> not (tryCast(x, int)))) then Some "Error parsing the Currency Pair ID list."
        else if (tryCast(this.minQuantity, int) && this.minQuantity <> "") then (this.minQuantity <- ""; None)
        else if (tryCast(this.maxQuantity, int) && this.maxQuantity <> "") then (this.minQuantity <- ""; None)
        else None


module UnitOfWork = 
    //New context from the factory
    let ctx = ContextFactory.Create(Config.configuration.connectionString)

    type UnitOfWork(contextIn : RepositoryContext) =
        let context = contextIn
        member this.Providers = ProvidersRepository(context)
        member this.CRPairs = CurrencyPairsRepository(context)
        member this.Pricerecords = PriceRecordsRepository(context)

        //Auxiliar functions to wrap and unwrap the results
        member this.PackListSomeOrNone x = match x with | [] -> None | _ -> Some(x) 
        member this.UnpackListSomeOrNone x = match x with | None -> [] | Some(x) -> x
        member this.ReturnIQuery (x : IQueryable<'T>) = x.ToArray<'T>() |>  List.ofArray |> this.PackListSomeOrNone

        // CURRENCY PAIRS

        member this.FindCPairById(id : int) = 
            this.CRPairs.GetById<CurrencyPairItem>(id)

        member this.InsertCurrencyPair(new_registry : CurrencyPairItem) = 
            this.CRPairs.Insert(new_registry) |> ignore //Insert into the context
            this.Complete() |> ignore //Calls the context to save the changes
            None // None for matching the return

        member this.UpdateCurrencyPair(update_registry : CurrencyPairItem) =
            try 
                let record = this.CRPairs.GetById<CurrencyPairItem>(update_registry.Id)
                record.Label <- update_registry.Label
                this.Complete()
                None
            with
                err -> Some err

        // PROVIDERS

        member this.FindProviderById(id : int) = 
            this.Providers.GetById<ProviderItem>(id)

        member this.InsertProvider(new_registry : ProviderItem) = 
            this.Providers.Insert(new_registry) |> ignore //Insert into the context
            this.Complete() |> ignore //Calls the context to save the changes
            None // None for matching the return

        member this.UpdateProvider(update_registry : ProviderItem) =
            try 
                let record = this.Pricerecords.GetById<ProviderItem>(update_registry.Id)
                record.Name <- update_registry.Name
                this.Complete()
                None
            with
                err -> Some err

        // PRICE RECORDS

        member this.GetPriceRecordsByCPairs(cpairs_id : int) = 
            this.Pricerecords.Where<PriceRecordItem>((fun (x : PriceRecordItem) -> x.CPair.Id.Equals(cpairs_id)))
            |> this.ReturnIQuery

        member this.GetPriceRecordsByProviders(providers_id : int) = 
            this.Pricerecords.Where<PriceRecordItem>((fun (x : PriceRecordItem) -> x.Provider.Id.Equals(providers_id)))
            |> this.ReturnIQuery

        member this.FilterByMaxDate(max_date : System.DateTime) = 
            this.Pricerecords.Where<PriceRecordItem>((fun (x : PriceRecordItem) -> x.Date <= max_date))
            |> this.ReturnIQuery

        member this.FilterByMinDate(min_date : System.DateTime) = 
            this.Pricerecords.Where<PriceRecordItem>((fun (x : PriceRecordItem) -> x.Date >= min_date))
            |> this.ReturnIQuery

        member this.FilterByMaxQuantity(max_qtd : int) = 
            this.Pricerecords.Where<PriceRecordItem>(fun x -> x.Quantity <= max_qtd)
            |> this.ReturnIQuery

        member this.FilterByMinQuantity(min_qtd : int) = 
            this.Pricerecords.Where<PriceRecordItem>(fun x -> x.Quantity >= min_qtd)
            |> this.ReturnIQuery

        member this.GetByDateRange(min_date : System.DateTime, max_date : System.DateTime) = 
            this.Pricerecords.Where<PriceRecordItem>(fun (x : PriceRecordItem) -> x.Date >= min_date && x.Date <= max_date)
            |> this.ReturnIQuery

        member this.GetAllPricesInOrderByDate(min_date : System.DateTime, max_date : System.DateTime) = 
            (min_date, max_date) |> this.Pricerecords.GetAllOrderedInRange

        member this.GetMarketData(min_date : System.DateTime, max_date : System.DateTime, currency_pair_id : int, provider_id : int) = 
            (min_date, max_date)
            |> this.Pricerecords.GetAllOrderedInRange
            |> this.UnpackListSomeOrNone
            |> List.map (fun (key, values) -> (key, values |> List.filter (fun data -> data.CPair.Id = currency_pair_id && data.Provider.Id = provider_id )))
            |> fun x -> match x with | [] -> None | _ -> Some(x) 

        //Filter the price records with possibly null filters and concat the results of the previous functions
        //hence the individual checking for the binded type
        member this.FilterRecords(filters : Filters) =
            let minDate = if filters.minDate <> "" then 
                            filters.minDate
                            |> System.DateTime.Parse
                            |> this.FilterByMinDate
                            |> this.UnpackListSomeOrNone 
                          else []
            let maxDate = if filters.maxDate <> "" then 
                            filters.maxDate
                            |> System.DateTime.Parse
                            |> this.FilterByMaxDate
                            |> this.UnpackListSomeOrNone 
                          else []
            let maxQuantity = if filters.maxQuantity <> "" then 
                                filters.maxQuantity
                                |> int
                                |> this.FilterByMaxQuantity
                                |> this.UnpackListSomeOrNone 
                              else []
            let minQuantity = if filters.minQuantity <> "" then 
                                filters.minQuantity
                                |> int
                                |> this.FilterByMinQuantity
                                |> this.UnpackListSomeOrNone 
                              else []
            let pairs = if filters.pairs <> [] then 
                            this.Pricerecords.All<PriceRecordItem>()
                            |> List.ofArray
                            |> List.groupBy (fun (elem : PriceRecordItem) -> elem.CPair.Id)
                            |> List.filter (fun (key, value) -> List.contains key filters.pairs)
                            |> List.map (fun (key, value) -> value)
                            |> List.reduce List.append
                        else []
            let provider = this.GetPriceRecordsByProviders(filters.provider |> int) |> this.UnpackListSomeOrNone 

            //Concat the data from each function assuring they are not duplicate in pairs
            (((((pairs @ provider) |> List.distinct) @ minDate |> List.distinct ) @ maxDate |> List.distinct) @ maxQuantity |> List.distinct) @minQuantity |> List.distinct |> this.PackListSomeOrNone


        member this.InsertPriceRecord(new_registry : PriceRecordItem) = 
            let aggregateAndRegister(new_registry : PriceRecordItem) = // Handle the swap of aggregate input and insert
                new_registry.Provider <- this.Providers.GetById<ProviderItem>(new_registry.Provider.Id) //Searches for existing registry and swaps it with the input
                new_registry.CPair <- this.CRPairs.GetById<CurrencyPairItem>(new_registry.CPair.Id)
                this.Pricerecords.Insert(new_registry) |> ignore //Finally insert into the context
                this.Complete() |> ignore //Calls the context to save the changes
                None // None for matching the return
            match (box (this.Providers.GetById<ProviderItem>(new_registry.Provider.Id)), box (this.Providers.GetById<CurrencyPairItem>(new_registry.CPair.Id))) with
            | (null, _) -> Some("Provider not found.")
            | (_, null) -> Some("Currency Pair not found.")
            | (_ , _) -> aggregateAndRegister(new_registry)

        member this.UpdatePriceRecord(update_registry : PriceRecordItem) =
            try 
                let record = this.Pricerecords.GetById<PriceRecordItem>(update_registry.Id)
                record.CPair <-  this.CRPairs.GetById<CurrencyPairItem>(update_registry.CPair.Id)
                record.Date <- update_registry.Date
                record.Price <- update_registry.Price
                record.Provider <- this.Providers.GetById<ProviderItem>(update_registry.Provider.Id)
                record.Quantity <- update_registry.Quantity
                record.SubProvider <- update_registry.SubProvider
                this.Complete()
                None
            with
                err -> Some err
        //Register the results in the context
        member this.Complete() =
            context.SaveChanges() |> ignore
        //Disregard the context, destroying all not saved
        member this.Dispose =
            context.Dispose() |> ignore

    let uow = UnitOfWork(ctx)
