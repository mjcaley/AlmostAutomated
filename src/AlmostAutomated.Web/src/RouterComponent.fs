module RouterComponent

open Feliz
open Feliz.Router
open Pages.Index
open Page
open Error404

let routes url =
    match url with
    | [] -> Page <| Index()
    //| [ "templates" ] -> Templates
    | [ "users" ] -> Html.h1 "Users page"
    | [ "users"; Route.Int userId ] -> Html.h1 (sprintf "User ID %d" userId)
    | _ -> Page <| Error404()

[<ReactComponent>]
let RouterComponent () =
    let (currentUrl, updateUrl) = React.useState (Router.currentUrl ())
    React.router [ router.onUrlChanged updateUrl; router.children [ routes currentUrl ] ]
