namespace AlmostAutomated.Infrastructure

open Dapper.FSharp.PostgreSQL
open AlmostAutomated.Core.Entities

module DataAccess =

    let templateTable = table<Template>
    let templateDetailsTable = table<TemplateDetails>
