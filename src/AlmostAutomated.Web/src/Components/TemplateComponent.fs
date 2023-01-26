module TemplateComponent

open Feliz
open Feliz.Bulma
open AlmostAutomated.Core.DTO

[<ReactComponent>]
let TemplateComponent (template: TemplateDTO) =
    Bulma.card [ 
      prop.id ("template-" + template.Id.ToString())
      size.isSize7
      spacing.mx4
      spacing.my4
      prop.children [
        Bulma.cardContent [
          Bulma.media [
              Bulma.mediaContent [
                  Bulma.title.p [
                      title.is4
                      prop.text template.Title
                  ]
              ]
          ]
          Bulma.content template.Description
        ]
      ]
    ]
