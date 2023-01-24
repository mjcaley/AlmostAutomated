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

let templateDecoder =
    Decode.object
    <| fun get ->
        { Id = get.Required.Field "id" Decode.int64
          Title = get.Required.Field "title" Decode.string
          Description = get.Required.Field "description" Decode.string }

let templatesDecoder = Decode.array templateDecoder

let extras = Extra.empty |> Extra.withInt64

let listTemplates apiBase =
    promise {
        let! templateResult =
            Fetch.tryGet (formatApiUrl apiBase [ "templates" ], extra = extras, decoder = templatesDecoder)

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
