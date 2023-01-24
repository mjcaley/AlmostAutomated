module Tests

open Xunit
open FsUnit.Xunit
open AlmostAutomated.Web.ApiClient
open AlmostAutomated.Core.DTO
open Thoth.Json

[<Fact>]
let ``Decord of TemplateDTO`` () =
    let expected: TemplateDTO = {Id=42L; Title="title"; Description="description"}
    let decoded = Decode.fromString templateDecoder """{id: 42, title="title", description="description"}"""

    match decoded with
    | Ok template -> template |> should equal expected
    | Error _ -> false |> should be True
