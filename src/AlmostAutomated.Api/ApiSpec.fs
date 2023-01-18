namespace AlmostAutomated.Api

open OpenApi
open Microsoft.OpenApi.Models
open System.Net

module ApiSpec =

    let apiSpec =
        apiDocument {
            info (
                apiInfo {
                    version "0.1.0"
                    title "Almost Automated API"
                }
            )

            servers [ apiServer { url "https://localhost:4000/api" } ]

            paths
                [ "/templates",
                  apiPathItem {
                      operations
                          [ OperationType.Get,
                            apiOperation {
                                description "List all templates."
                                responses [ HttpStatusCode.OK, apiResponse { description "OK" } ]
                            } ]
                  } ]
        }
