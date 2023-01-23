module ApiClient

open AlmostAutomated.Core.DTO
open Fable.SimpleHttp
open Thoth.Json

type ApiResponse<'a> =
    | Success of 'a
    | DecoderError
    | HttpError of int * string

let internal formatApiUrl apiBase paths =
    apiBase + String.concat "/" paths

let apiBase = Config.variable "API_ROOT"

let templateDecoder =
    Decode.object <| fun fields -> {
        Id=fields.Required.At [ "id" ] Decode.int64;
        Title=fields.Required.At [ "title" ] Decode.string;
        Description=fields.Required.At [ "description" ] Decode.string;
    }

let templatesDecoder =
    Decode.array templateDecoder

let listTemplates apiBase =
    async {
        let! (statusCode, response) = Http.get <| formatApiUrl apiBase ["templates"]
        printfn "I just did an API call!"

        return
            match statusCode with
            | success when success >= 200 && success <= 299 -> 
                match Decode.fromString templatesDecoder response with
                | Ok t -> Success <| List.ofArray t
                | Error msg -> DecoderError
            | userError when userError >= 400 && userError <= 499 -> HttpError (userError, "Could not decode response")
            | serverError when serverError >= 500 && serverError <=599 -> HttpError (serverError, "Server error occurred")
            | _ -> HttpError (statusCode, "Unknown error occurred")
    }
