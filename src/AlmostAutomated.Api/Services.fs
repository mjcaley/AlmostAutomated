module AlmostAutomated.Api.Services

open AlmostAutomated.Core.DTO
open AlmostAutomated.Infrastructure.TemplateRepository


let listTemplatesService dbConn =
    task {
        let! templates = getAll dbConn
        return templates |> List.map toTemplateAndDetails'
    }


let getTemplateService dbConn id =
    task {
        let! template = get dbConn id
        return template |> toTemplateAndDetails'
    }


let createTemplateService dbConn details = task { return! create dbConn details }


let deleteTemplateService dbConn id = task { return! delete dbConn id }
