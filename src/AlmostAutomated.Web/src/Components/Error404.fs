module Error404

open Feliz
open Feliz.Bulma

[<ReactComponent>]
let Error404 () =
    Html.div [ Html.h2 "404 Not Found" ]
