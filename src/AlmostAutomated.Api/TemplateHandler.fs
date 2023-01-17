namespace AlmostAutomated.Api

module TemplateHandler =
    open Giraffe

    let listTemplates =
        fun next ctx ->
           task {
                return! text "Hello" next ctx
           } 
