module AlmostAutomated.Api.Services

open AlmostAutomated.Core.DTO


let listTemplatesService repo =
    task {
        let! templates = repo
        return templates |> List.map toTemplateAndDetails'
    }


let getTemplateService repo =
    task {
        let! template = repo

        return
            match template with
            | Some t -> t |> toTemplateAndDetails' |> Some
            | None -> None
    }


let createTemplateService repo = task { return! repo }


let deleteTemplateService repo = task { return! repo }
