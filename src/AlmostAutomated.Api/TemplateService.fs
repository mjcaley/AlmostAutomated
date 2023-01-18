module AlmostAutomated.Api.TemplateService

open AlmostAutomated.Infrastructure.TemplateRepository
open AlmostAutomated.Core.DTO

let listTemplates dbConn =
    task {
        let! templates = getAll dbConn
        return templates |> List.ofSeq |> List.map toTemplateAndDetails' |> Map
    }
