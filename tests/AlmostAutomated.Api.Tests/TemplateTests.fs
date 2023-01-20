module TemplateTests

open System
open Xunit
open FsUnit.Xunit
open AlmostAutomated.Api.Services
open AlmostAutomated.Core.Entities
open AlmostAutomated.Core.DTO


[<Fact>]
let ``List template returns list of DTO`` () =
    task {
        let testInput: (Template.Select * TemplateDetails.Select) list =
            [ ({ Id = 1L
                 Created = DateTime.UtcNow
                 Deleted = None },
               { Id = 1L
                 Title = "title"
                 Description = "description"
                 ValidFrom = DateTime.UtcNow
                 ValidTo = None
                 TemplateId = 1L })
              ({ Id = 2L
                 Created = DateTime.UtcNow
                 Deleted = None },
               { Id = 2L
                 Title = "title"
                 Description = "description"
                 ValidFrom = DateTime.UtcNow
                 ValidTo = None
                 TemplateId = 2L }) ]

        let expected: TemplateDTO list =
            [ { Id = 1L
                Title = "title"
                Description = "description" }
              { Id = 2L
                Title = "title"
                Description = "description" } ]

        let listRepo = task { return testInput }

        let! templates = listTemplatesService listRepo

        templates |> should equal expected
    }

[<Fact>]
let ``Get template returns DTO`` () =
    task {
        let testInput =
            task {
                let template: (Template.Select * TemplateDetails.Select) option =
                    Some(
                        { Id = 1L
                          Created = DateTime.UtcNow
                          Deleted = None },
                        { Id = 1L
                          Title = "title"
                          Description = "description"
                          ValidFrom = DateTime.UtcNow
                          ValidTo = None
                          TemplateId = 1L }
                    )

                return template
            }

        let expected =
            { Id = 1L
              Title = "title"
              Description = "description" }

        let! result = getTemplateService testInput

        match result with
        | Ok t -> t |> should equal expected
        | _ -> assert false
    }

[<Fact>]
let ``Get template returns none`` () =
    task {
        let testInput = task { return None }

        let! result = getTemplateService testInput

        match result with
        | NotFound -> assert true
        | _ -> assert false
    }

[<Fact>]
let ``Post template returns ID`` () =
    task {
        let createRepo = task { return 42 }

        let! result = createTemplateService createRepo

        result |> should equal 42
    }

[<Fact>]
let ``Delete template service returns entity`` () =
    task {
        let deletedDetails: Template.Select =
            { Id = 42L
              Created = DateTime.UtcNow
              Deleted = Some DateTime.UtcNow }

        let deleteRepo = task { return deletedDetails }

        let! detail = deleteTemplateService deleteRepo

        detail |> should equal deletedDetails
    }
