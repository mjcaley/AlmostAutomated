namespace AlmostAutomated.Infrastructure

module TemplateRepository =

    open DataAccess
    open AlmostAutomated.Core.Entities
    open Dapper.FSharp.PostgreSQL
    open System.Data

    let getAll (db: IDbConnection) =
        select {
            for template in templateTable do
                innerJoin details in templateDetailsTable on (template.Id = details.TemplateId)
                orderBy template.Id
        }
        |> db.SelectAsync<Template, TemplateDetails>

//let get (dbContext) (id: int) =

//let update dbContext id: int =

//let delete dbContext id: int =
