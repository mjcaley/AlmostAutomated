module Program

open Pulumi.FSharp
open Pulumi.Kubernetes.Types.Inputs.Core.V1
open Pulumi.Kubernetes.Types.Inputs.Apps.V1
open Pulumi.Kubernetes.Types.Inputs.Meta.V1
open Pulumi.Kubernetes.Apps.V1
open Pulumi.Kubernetes.Core.V1
open Pulumi.Kubernetes.Types.Outputs.Meta.V1


let toBase64 (string: string) =
    let bytes = System.Text.Encoding.UTF8.GetBytes(string)
    System.Convert.ToBase64String(bytes)

[<Literal>]
let namespaceName = "almost-automated"

let db () =
    let appLabels = inputMap [ "app", input "almost-automated" ]

    let containers = inputList [
        input (ContainerArgs(
            Name = "almost-automated-db",
            Image = "postgres:15",
            Ports = inputList [ input(ContainerPortArgs(ContainerPortValue = 5432)) ]
        ))
    ]

    let config = ConfigMap("db",
        ConfigMapArgs(
            Metadata = ObjectMetaArgs(
                Namespace = namespaceName
            ),
            Data = inputMap [
                ("name", input "almostapidb")
            ]
        )
    )

    let auth = Secret("db",
        SecretArgs(
            Metadata = ObjectMetaArgs(
                Namespace = namespaceName
            ),
            Data = inputMap [
                ("username", input <| toBase64 "almostapidb");
                ("password", input <| toBase64 "password")
            ]
        )
    )

    let persistentVolume = PersistentVolume("db",
        PersistentVolumeArgs(
            Metadata = ObjectMetaArgs(
                Namespace = namespaceName
            ),
            Spec = PersistentVolumeSpecArgs(
                StorageClassName = "local-path",
                Capacity = inputMap [
                    "storage", input "1Gi"
                ],
                AccessModes = inputList [input "ReadWriteMany"],
                HostPath = HostPathVolumeSourceArgs(
                    Path = input "/data/db"
                )
            )
        )
    )

    let persistentVolumeClaim = PersistentVolumeClaim("db",
        PersistentVolumeClaimArgs(
            Metadata = ObjectMetaArgs(
                Namespace = namespaceName
            ),
            Spec = PersistentVolumeClaimSpecArgs(
                Resources = ResourceRequirementsArgs(
                    Requests = inputMap [ ("storage", input "1Gi") ]
                ),
                AccessModes = inputList [input "ReadWriteMany"]
            )
        )
    )

    let dbLabels = ("app", input "db")

    let deployment = Deployment("db",
        DeploymentArgs(
            Metadata = ObjectMetaArgs(
                Namespace = namespaceName
            ),
            Spec = DeploymentSpecArgs(
                Replicas = 1,
                Selector = LabelSelectorArgs(MatchLabels = inputMap [ dbLabels ])
            )
        )
    )

    dict []

  //let podSpecs = PodSpecArgs(Containers = containers)

  //let deployment = Deployment("postgres",
  //  DeploymentArgs(
  //      Metadata = ObjectMetaArgs(
  //          Labels = appLabels,
  //          Namespace = ns.GetResourceName()
  //      )
  //  ))

  //dict [("db", deployment :> obj)]


let infra () =
  let appLabels = inputMap ["app", input "nginx" ]
  
  let containers : Pulumi.InputList<ContainerArgs> = inputList [
    input (ContainerArgs(
       Name = "nginx",
       Image = "nginx",
       Ports = inputList [ input(ContainerPortArgs(ContainerPortValue = 80)) ]
    ))
  ]
 
  let podSpecs = PodSpecArgs(Containers = containers)

  let deployment = 
    Deployment("nginx",
      DeploymentArgs
        (Spec = DeploymentSpecArgs
          (Selector = LabelSelectorArgs(MatchLabels = appLabels),
           Replicas = 1,
           Template = 
             PodTemplateSpecArgs
              (Metadata = ObjectMetaArgs(Labels = appLabels),
               Spec = podSpecs))))
  
  let name = 
    deployment.Metadata
    |> Outputs.apply(fun metadata -> metadata.Name)

  dict [("name", name :> obj)]

[<EntryPoint>]
let main _ =
  Deployment.run db
