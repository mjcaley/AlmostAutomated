module AlmostAutomated.Infrastructure.TemplateRepository

open AlmostAutomated.Core.Entities
open Dapper.FSharp.PostgreSQL
open System.Data
open System

let templateTable = table'<Template.Select> "Template"
let templateTable' = table'<Template.Insert> "Template"
let templateDetailsTable = table'<TemplateDetails.Select> "TemplateDetails"
let templateDetailsTable' = table'<TemplateDetails.Insert> "TemplateDetails"

let getAll (dbConn: IDbConnection) =
    task {
        let query =
            select {
                for template in templateTable do
                    innerJoin details in templateDetailsTable on (template.Id = details.TemplateId)
                    where (template.Deleted = None)
                    orderBy template.Id
            }

        let! results = query |> dbConn.SelectAsync<Template.Select, TemplateDetails.Select>
        return results |> List.ofSeq
    }

let get (dbConn: IDbConnection) (id: int64) =
    task {
        let query =
            select {
                for template in templateTable do
                    innerJoin details in templateDetailsTable on (template.Id = details.TemplateId)
                    where (template.Deleted = None && template.Id = id)
            }

        let! result = query |> dbConn.SelectAsync<Template.Select, TemplateDetails.Select>

        return result |> Seq.tryHead
    }

let create (dbConn: IDbConnection) (details: TemplateDetails.Insert') =
    task {
        let template: Template.Insert = { Created = DateTime.UtcNow }

        let transaction = dbConn.BeginTransaction()

        let! insertedTemplates =
            insert {
                into templateTable'
                value template
            }
            |> dbConn.InsertOutputAsync<Template.Insert, Template.Select>

        let insertedTemplate = insertedTemplates |> Seq.head

        let! _ =
            insert {
                into templateDetailsTable'

                value
                    { Title = details.Title
                      Description = details.Description
                      TemplateId = insertedTemplate.Id }
            }
            |> dbConn.InsertAsync

        transaction.Commit()

        return insertedTemplate.Id
    }

let delete (dbConn: IDbConnection) (id: int64) =
    task {
        let! result =
            update {
                for t in templateTable do
                    setColumn t.Deleted (Some DateTime.UtcNow)
                    where (t.Id = id && t.Deleted = None)
            }
            |> dbConn.UpdateOutputAsync<Template.Select, Template.Select>

        return result |> Seq.tryHead
    }
