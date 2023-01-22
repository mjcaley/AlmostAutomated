module AlmostAutomated.Infrastructure.TemplateRepository

open AlmostAutomated.Core.Entities
open Dapper.FSharp.PostgreSQL
open System.Data
open System

let templateTable = table'<Template.Select> "Template"
let templateTable' = table'<Template.Insert> "Template"
let templateDetailsTable = table'<TemplateDetails.Select> "TemplateDetails"
let templateDetailsTable' = table'<TemplateDetails.Insert> "TemplateDetails"

let internal isTemplateDeleted showDeleted (template: Template.Select * TemplateDetails.Select) =
    let t = fst template
    t.Deleted.IsSome = showDeleted

let listTemplates (dbConn: IDbConnection) isDeleted =
    task {
        let query =
            select {
                for template in templateTable do
                    innerJoin details in templateDetailsTable on (template.Id = details.TemplateId)
                    orderBy template.Id
            }

        let! results = query |> dbConn.SelectAsync<Template.Select, TemplateDetails.Select>
        return results |> List.ofSeq |> List.filter (isTemplateDeleted isDeleted)
    }

let getTemplateById (dbConn: IDbConnection) (id: int64) isDeleted =
    task {
        let query =
            select {
                for template in templateTable do
                    innerJoin details in templateDetailsTable on (template.Id = details.TemplateId)
                    where (template.Id = id)
            }

        let! result = query |> dbConn.SelectAsync<Template.Select, TemplateDetails.Select>

        return
            result
            |> List.ofSeq
            |> List.filter (isTemplateDeleted isDeleted)
            |> List.tryHead
    }

let createTemplate (dbConn: IDbConnection) (details: TemplateDetails.Insert') =
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

let deleteTemplate (dbConn: IDbConnection) (id: int64) =
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
