module RouterComponent

open Feliz
open Feliz.Router
open Index
open Templates
open TemplateEdit
open Page
open Error404

let routes url =
    match url with
    | [] -> Page <| Index ()
    | [ "templates" ] -> Page <| Templates ()
    | [ "templates"; Route.Int64 id ] -> Page <| TemplateEdit (id)
    | [ "templates"; "new" ] -> Html.h1 "New template"
    | _ -> Page <| Error404 ()

[<ReactComponent>]
let RouterComponent () =
    let (currentUrl, updateUrl) = React.useState (Router.currentUrl ())
    React.router [ router.onUrlChanged updateUrl; router.children [ routes currentUrl ] ]
