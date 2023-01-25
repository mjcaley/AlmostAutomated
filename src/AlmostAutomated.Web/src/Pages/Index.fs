﻿module Pages.Index

open Feliz
open Feliz.Bulma
open AlmostAutomated.Web.ApiClient
open Sidebar
open TemplateComponent

[<ReactComponent>]
let Index () =
    let (count, setCount) = React.useState (0)
    let (templates, setTemplates) = React.useState ([])

    let fetchData () =
        promise {
            let! templatesResponse = listTemplates apiBase

            match templatesResponse with
            | HttpError(statusCode, message) -> Browser.Dom.console.error $"Http error {statusCode} - {message}"
            | DecodeError -> Browser.Dom.console.error "Error decoding templates"
            | Success t -> setTemplates t
            | NetworkErr exc -> Browser.Dom.console.error exc.Message
        }

    React.useEffectOnce <| React.useCallback (fun () -> fetchData () |> ignore)

    Html.div
        [ Sidebar ()
          Html.h1 count
          Html.button [ prop.text "Increment"; prop.onClick (fun _ -> setCount (count + 1)) ]
          Html.p apiBase
          Html.div (List.map TemplateComponent templates)
          Bulma.button.a [
            color.isWarning
            prop.onClick (fun _ -> Fable.Core.JS.eval "alert('Hello Feliz.Bulma')" |> ignore)
            prop.text "Amazing button, ain't it?" ]
        ]
