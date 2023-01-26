module AlmostAutomated.Web.ApiClient

open AlmostAutomated.Core.DTO
open Thoth.Json
open Thoth.Fetch

type ApiResponse<'a> =
    | Success of 'a
    | DecodeError
    | HttpError of int * string
    | NetworkErr of exn
    | UnknownError

let internal formatApiUrl apiBase paths = apiBase + String.concat "/" paths

let apiBase = Config.variable "API_ROOT"

let extras = Extra.empty |> Extra.withInt64

let listTemplates () =
    promise {
        let! templateResult =
            Fetch.tryGet<_, array<TemplateDTO>>(formatApiUrl apiBase [ "templates" ], extra=extras)

        return
            match templateResult with
            | Ok t -> Success <| List.ofArray t
            | Error message ->
               match message with
               | DecodingFailed _ -> DecodeError
               | FetchFailed response -> HttpError(response.Status, response.StatusText)
               | NetworkError exc -> NetworkErr exc
               | _ -> UnknownError
    }

let getTemplate id =
    promise {
        let! templateResult =
            Fetch.tryGet<_, TemplateDTO>(formatApiUrl apiBase [ "template"; id ], extra=extras)

        return
            match templateResult with
            | Ok t -> Success <| t
            | Error message ->
               match message with
               | DecodingFailed _ -> DecodeError
               | FetchFailed response -> HttpError(response.Status, response.StatusText)
               | NetworkError exc -> NetworkErr exc
               | _ -> UnknownError
    }
