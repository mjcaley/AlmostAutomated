module Program

open Pulumi.FSharp
open Namespace
open Database
open Migration
open Api



let infra () =
    let ns = ns ()

    let dbAuth, dbConfig, dbService = db ns
    let migrationJob = migrationJob ns dbService dbAuth
    let apiDeployment = api ns dbAuth dbConfig dbService migrationJob

    dict [ ("namespace", ns :> obj); ("apiIngress", apiDeployment.Ingress) ]

[<EntryPoint>]
let main _ = Deployment.run infra
