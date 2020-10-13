### How to build application

1. Make sure you have installed version of .Net SDK defined in `global.json`
2. Change your directory to src/RepositoryManager/
2. Run `dotnet tool restore` to restore all necessary tools
3. Run `dotnet run` and make sure to allow the host in the firewall, if using Windows

### Database

The database is by default drop and create with a seed (Startup.fs), in order to operate with migrations using EFCore you have to compile this library in your machine and install it in the dependencies, then change the startup to add the migrations support to the desired database, as well as change the drop and create functions in the Context Factory to Migrations, in order to make a runtime migration.

Library mentioned: https://github.com/efcore/EFCore.FSharp (Not available as a nuget yet, hence the compilation)

#### Saturn migration

`dotnet saturn` is an alternative, but currently only seem to work with SQLite and does not accept FK models, so the default database (PostgreSQL must be changed).

* `gen NAME NAMES COLUMN:TYPE COLUMN:TYPE COLUMN:TYPE ...` - creates model, database layer, views and controller returning HTML views
* `gen.json NAME NAMES COLUMN:TYPE COLUMN:TYPE COLUMN:TYPE ...` - creates model, database layer and JSON API controller
* `gen.model NAME NAMES COLUMN:TYPE COLUMN:TYPE COLUMN:TYPE ...` - creates model and database layer
* `migration` - runs all migration scripts for the database

#### HOST

* The host will receive PUT, GET, POST and DELETE requests at localhost:8085/
* For checking the simple client side, go to localhost:8085/data/plot/<ID OF A PROVIDER>/<ID OF A CURRENCY PAIR> or localhost:8085/data/table/ID_OF_A_PROVIDER/ID_OF_A_CURRENCY PAIR - For example, localhost:8085/data/table/1/1
