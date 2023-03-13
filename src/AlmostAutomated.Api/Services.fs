module AlmostAutomated.Api.Services

open AlmostAutomated.Core.DTO


type ServiceResult<'a> =
    | Ok of 'a
    | NotFound

type TemplateCreate =
    { Title: string
      Description: string }

type TemplateUpdate =
    { Title: string option
      Description: string option }

let listTemplatesService listRepo deletedRepo deleted =
    task {
        let! templates = if deleted then deletedRepo else listRepo
        return templates |> List.map toTemplateDTO
    }

let getTemplateService getRepo deletedRepo id deleted =
    task {
        try
            let! template = if deleted then deletedRepo id else getRepo id
            return template |> toTemplateDTO |> Ok
        with
        | NoResultsException _ -> return NotFound
    }

let createTemplateService repo (details: TemplateCreate) =
    task {
        return! repo details.Title details.Description
    }

let deleteTemplateService repo id =
    task {
        try
            let! result = repo id
            return Ok result
        with 
        | NoResultsException _ -> return NotFound
    }
