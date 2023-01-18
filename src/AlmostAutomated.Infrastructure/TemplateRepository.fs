module AlmostAutomated.Infrastructure.TemplateRepository

open DataAccess
open AlmostAutomated.Core.Entities
open Dapper.FSharp.PostgreSQL
open System.Data

let getAll (dbConn: IDbConnection) =
    task {
        let query =
            select {
                for template in templateTable do
                    innerJoin details in templateDetailsTable on (template.Id = details.TemplateId)
                    orderBy template.Id
            }

        let! results = query |> dbConn.SelectAsync<Template, TemplateDetails>
        return results |> List.ofSeq
    }

let get (dbConn: IDbConnection) (id: int) =
    task {
        let query =
            select {
                for template in templateTable do
                    innerJoin details in templateDetailsTable on (template.Id = details.TemplateId)
                    where (template.Id = id)
            }

        let! result = query |> dbConn.SelectAsync<Template, TemplateDetails>
        return result |> Seq.head
    }

//let update dbContext id: int =

//let delete dbContext id: int =
