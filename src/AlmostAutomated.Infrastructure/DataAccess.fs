module AlmostAutomated.Infrastructure.DataAccess

open Dapper.FSharp.PostgreSQL
open AlmostAutomated.Core.Entities
open Npgsql


let openDataSource (connectionString: string) =
    NpgsqlDataSource.Create(connectionString)

let templateTable = table<Template>
let templateDetailsTable = table<TemplateDetails>
