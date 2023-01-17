namespace AlmostAutomated.Infrastructure

open DataAccess
open Dapper.FSharp.PostgreSQL
open Dapper.FSharp.PostgreSQL.IDbConnection

module TemplateRepository =
    let getAll (db : Dapper.FSharp.PostgreSQL.IDbConnection) =
        select {
            for template in templateTable do
            innerJoin details in templateDetailsTable on (template.TemplateDetailsId = details.Id)
            orderBy template.Id
        } |> db.SelectAsync<TemplateDetails>
        
    //let get (dbContext) (id: int) =

    //let update dbContext id: int =

    //let delete dbContext id: int =
