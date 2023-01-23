module Pages.Index

open Fable.Core
open Feliz
open ApiClient
open TemplateComponent

[<ReactComponent>]
let Index() =
    let (count, setCount) = React.useState(0)
    let (templates, setTemplates) = React.useState([])

    let fetchData () =
        async {
            let! templatesResponse = listTemplates apiBase
            match templatesResponse with
            | HttpError (statusCode, message) -> Browser.Dom.console.error $"Http error {statusCode} - {message}"
            | DecoderError -> Browser.Dom.console.error "Error decoding templates"
            | Success t -> setTemplates t
        }

    React.useEffectOnce <| React.useCallback(fun () -> fetchData() |> ignore)

    Html.div [
        Html.h1 count
        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> setCount(count + 1))
        ]
        Html.p apiBase
        Html.div (List.map TemplateComponent templates)
    ]
