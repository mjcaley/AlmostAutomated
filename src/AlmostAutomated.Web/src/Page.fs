module Page

open Feliz
open Feliz.Bulma
open BulmaExtensions
open Header
open Sidebar

[<ReactComponent>]
let Page (content: ReactElement) =
    Html.div [
        Header ()

        Bulma.columns [
            Bulma.column [
                prop.classes [ "is-one-fifth" ]
                prop.children [
                    Sidebar ()
                ]
            ]
            Bulma.content [
                content
            ]
        ]

        
            
        //]
    ]
