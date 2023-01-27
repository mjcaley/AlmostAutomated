﻿module Templates

open Feliz
open Feliz.Bulma
open Feliz.Router
open AlmostAutomated.Web.ApiClient
open TemplateComponent

[<ReactComponent>]
let Templates () =
    let (templates, setTemplates) = React.useState ([])

    let fetchData () =
        promise {
            let! templatesResponse = listTemplates ()

            match templatesResponse with
            | HttpError(statusCode, message) -> Browser.Dom.console.error $"Http error {statusCode} - {message}"
            | DecodeError -> Browser.Dom.console.error "Error decoding templates"
            | Success t -> setTemplates t
            | NetworkErr exc -> Browser.Dom.console.error ("Some kind of network error", [ exc.Message ])
        }

    React.useEffectOnce <| React.useCallback (fun () -> fetchData () |> ignore)

    Bulma.content [
        Bulma.container [
            Bulma.button.a (Router.formatPath "/templates/new")
        ]
        
        Bulma.block [
            Bulma.tile (List.map TemplateComponent templates)
        ]
    ]

