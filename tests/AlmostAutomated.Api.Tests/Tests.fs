module Tests

open System
open Xunit
open FsUnit.Xunit
open AlmostAutomated.Api.Services
open AlmostAutomated.Core.Entities


let deletedDetails: Template.Select = { Id=42L; Created=DateTime.UtcNow; Deleted=Some DateTime.UtcNow; }

[<Fact>]
let ``Delete template service returns entity`` () =
    task {
        let deleteRepo =
            task {
                return deletedDetails
            }

        let! detail = deleteTemplateService deleteRepo

        detail |> should equal deletedDetails
    }
