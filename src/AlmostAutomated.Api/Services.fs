module AlmostAutomated.Api.Services

open AlmostAutomated.Core.DTO


type ServiceResult<'a> =
    | Ok of 'a
    | NotFound


let listTemplatesService repo =
    task {
        let! templates = repo
        return templates |> List.map toTemplateDTO
    }


let getTemplateService repo =
    task {
        try
            let! template = repo
            return template |> toTemplateDTO |> Ok
        with
        | NoResultsException _ -> return NotFound
    }


let createTemplateService repo = task { return! repo }


let deleteTemplateService repo =
    task {
        try
            let! result = repo
            return Ok result
        with 
        | NoResultsException _ -> return NotFound
    }
