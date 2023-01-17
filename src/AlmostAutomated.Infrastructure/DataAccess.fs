namespace AlmostAutomated.Infrastructure

open Dapper.FSharp.PostgreSQL
open AlmostAutomated.Core.Entities
open Npgsql

module DataAccess =

    let openDataSource (connectionString : string) =
        NpgsqlDataSource.Create(connectionString)

    let templateTable = table<Template>
    let templateDetailsTable = table<TemplateDetails>
