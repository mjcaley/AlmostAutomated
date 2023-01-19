module AlmostAutomated.Infrastructure.DataAccess

open Dapper.FSharp.PostgreSQL
open AlmostAutomated.Core.Entities
open Npgsql


let openDataSource (connectionString: string) =
    NpgsqlDataSource.Create(connectionString)

let templateTable = table'<Template.Select> "Template"
let templateTable' = table'<Template.Insert> "Template"
let templateDetailsTable = table'<TemplateDetails.Select> "TemplateDetails"
let templateDetailsTable' = table'<TemplateDetails.Insert> "TemplateDetails"
