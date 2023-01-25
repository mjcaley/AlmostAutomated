module TemplateComponent

open Feliz
open Feliz.Bulma
open AlmostAutomated.Core.DTO

[<ReactComponent>]
let TemplateComponent (template: TemplateDTO) =
    Html.div [
      Bulma.card [ 
        prop.id (int template.Id)
        prop.children [
          Bulma.cardContent [
            Bulma.media [
                Bulma.mediaContent [
                    Bulma.title.p [
                        Bulma.title.is4
                        prop.text template.Title
                    ]
                ]
            ]
            Bulma.content template.Description
          ]
        ]
      ]
    ]
