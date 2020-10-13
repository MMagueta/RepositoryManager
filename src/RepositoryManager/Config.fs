module Config

type Config = {
    connectionString : string
}

let configuration = {connectionString="Host=database-3.cnnri9trsf5s.us-east-2.rds.amazonaws.com;Port=5432;Pooling=true;Database=teste_pg;User Id=postgres;Password=159753123"}