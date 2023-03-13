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
        let testInput: Templates list =
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
        let deletedRepo = task { return [] }

        let! templates = listTemplatesService listRepo deletedRepo false

        templates |> should equal expected
    }

[<Fact>]
let ``Get template returns DTO`` () =
    task {
        let get _ =
            task {
                let template: Templates =
                    { Id = 1L
                      Created = DateTime.UtcNow
                      Deleted = None
                      Title = "title"
                      Description = "description" }

                return template
            }

        let deleted _ =
            task {
                raise <| NoResultsException ""
                return 
                    { Id = 1L
                      Created = DateTime.UtcNow
                      Deleted = None
                      Title = "title"
                      Description = "description" }
            }

        let expected =
            { Id = 1L
              Title = "title"
              Description = "description" }

        let! result = getTemplateService get deleted 1L false

        match result with
        | Ok t -> t |> should equal expected
        | _ -> assert false
    }

[<Fact>]
let ``Get template returns none`` () =
    task {
        let get _ =
            task {
                raise <| NoResultsException ""
                let result: Templates =
                    { Id = 1L
                      Created = DateTime.UtcNow
                      Deleted = None;
                      Title = "title"
                      Description = "description" }
                return result
            }

        let deleted _ =
            task {
                raise <| NoResultsException ""
                return 
                    { Id = 1L
                      Created = DateTime.UtcNow
                      Deleted = None
                      Title = "title"
                      Description = "description" }
            }

        let! result = getTemplateService get deleted 1L false

        match result with
        | NotFound -> assert true
        | _ -> assert false
    }

[<Fact>]
let ``Post template returns ID`` () =
    task {
        let expectedTitle = "title"
        let expectedDescription = "description"
        let createRepo title description = task { return {| Title=title; Description=description |} }

        let! result = createTemplateService createRepo { Title=expectedTitle; Description=expectedDescription }

        result |> should equal {| Title=expectedTitle; Description=expectedDescription |}
    }

[<Fact>]
let ``Delete template service returns entity`` () =
    task {
        let deletedDetails: Templates =
            { Id = 1L
              Created = DateTime.UtcNow
              Deleted = Some DateTime.UtcNow;
              Title = "title"
              Description = "description" }

        let deleteRepo _ = task { return deletedDetails }

        let! detail = deleteTemplateService deleteRepo 1L

        detail |> should equal <| Ok deletedDetails
    }
