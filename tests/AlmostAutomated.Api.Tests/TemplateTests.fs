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
        let testInput: Template.Select list =
            [ { Id = 1L
                Created = DateTime.UtcNow
                Deleted = None
                Title = "title"
                Description = "description" }
              { Id = 2L
                Created = DateTime.UtcNow
                Deleted = None
                Title = "title"
                Description = "description"} ]

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
                let template: Template.Select =
                    { Id = 1L
                      Created = DateTime.UtcNow
                      Deleted = None
                      Title = "title"
                      Description = "description" }

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
        let testInput =
            task {
                raise <| NoResultsException ""
                let result: Template.Select =
                    { Id = 1L
                      Created = DateTime.UtcNow
                      Deleted = None;
                      Title = "title"
                      Description = "description" }
                return result
            }

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
            { Id = 1L
              Created = DateTime.UtcNow
              Deleted = Some DateTime.UtcNow;
              Title = "title"
              Description = "description" }

        let deleteRepo = task { return deletedDetails }

        let! detail = deleteTemplateService deleteRepo

        detail |> should equal <| Ok deletedDetails
    }
