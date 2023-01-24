module App

open Feliz
open Feliz.Router
open Pages.Index
open Error404


let routes url =
    match url with
    | [] -> Index()
    | [ "users" ] -> Html.h1 "Users page"
    | [ "users"; Route.Int userId ] -> Html.h1 (sprintf "User ID %d" userId)
    | _ -> Error404()

[<ReactComponent>]
let Router () =
    let (currentUrl, updateUrl) = React.useState (Router.currentUrl ())
    React.router [ router.onUrlChanged updateUrl; router.children [ routes currentUrl ] ]

open Browser.Dom

let root = ReactDOM.createRoot (document.getElementById "root")
root.render (Router())
