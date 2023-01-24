module Tests

open Fable.Mocha
open AlmostAutomated.Web.ApiClient
open AlmostAutomated.Core.DTO
open Thoth.Json

let apiTests =
    testList "API tests" [
        test "converts string to TemplateDTO" {
            let expected: TemplateDTO = {Id=42L; Title="title"; Description="description"}
            let decoded = Decode.fromString templateDecoder """{id: 42, title="title", description="description"}"""

            Expect.isOk decoded "ok"
            match decoded with
            | Ok t -> Expect.equal t expected "equal"
            | Error message -> Expect.isTrue false "error"
        }

    ]

Mocha.runTests apiTests
