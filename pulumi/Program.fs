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

let ioMetaName (res: Namespace) =
    io (Outputs.apply (fun (m: ObjectMeta) -> m.Name) res.Metadata)

let db () =
    let ns = Namespace("almost-automated")

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
                Namespace = ioMetaName ns
            ),
            Data = inputMap [
                ("POSTGRES_DB", input "almostapidb")
            ]
        )
    )

    let auth = Secret("db",
        SecretArgs(
            Metadata = ObjectMetaArgs(
                Namespace = ioMetaName ns
            ),
            Data = inputMap [
                ("POSTGRES_USER", input <| toBase64 "almostapidb");
                ("POSTGRES_PASSWORD", input <| toBase64 "password")
            ]
        )
    )

    let persistentVolume = PersistentVolume("db",
        PersistentVolumeArgs(
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
                Namespace = ioMetaName ns,
                Annotations = inputMap [
                    ("pulumi.com/skipAwait", input "true")
                ]
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
                Namespace = ioMetaName ns
            ),
            Spec = DeploymentSpecArgs(
                Replicas = 1,
                Selector = LabelSelectorArgs(MatchLabels = inputMap [ dbLabels ]),
                Template = PodTemplateSpecArgs(
                    Metadata = ObjectMetaArgs(
                        Namespace = ioMetaName ns,
                        Labels = inputMap [ dbLabels ]
                    ),
                    Spec = PodSpecArgs(
                        Containers = inputList [
                            input <| ContainerArgs(
                                Name = "db",
                                Image = "postgres:15",
                                ImagePullPolicy = "IfNotPresent",
                                Ports = inputList [
                                    input <| ContainerPortArgs(ContainerPortValue = 5432)
                                ],
                                EnvFrom = inputList [
                                    input <| EnvFromSourceArgs(
                                        ConfigMapRef = ConfigMapEnvSourceArgs(
                                            Name = io (Outputs.apply (fun (m: ObjectMeta) -> m.Name) config.Metadata)
                                        )
                                    );
                                    input <| EnvFromSourceArgs(
                                        SecretRef = SecretEnvSourceArgs(
                                            Name = io (Outputs.apply (fun (m: ObjectMeta) -> m.Name) auth.Metadata)
                                        )
                                    );
                                ],
                                VolumeMounts = inputList [
                                    input <| VolumeMountArgs(
                                        MountPath = "/var/lib/postgresql/data",
                                        Name = "postgresdata"
                                    )
                                ]
                            )
                        ],
                        Volumes = inputList [
                            input <| VolumeArgs(
                                Name = "postgresdata",
                                PersistentVolumeClaim = (input <| PersistentVolumeClaimVolumeSourceArgs(
                                        ClaimName = io (Outputs.apply (fun (m: ObjectMeta) -> m.Name) persistentVolumeClaim.Metadata)
                                ))
                            )
                        ]
                    )
                )
            )
        )
    )

    dict [
        ("auth", auth :> obj);
        ("config", config :> obj)
    ]

  //let podSpecs = PodSpecArgs(Containers = containers)

  //let deployment = Deployment("postgres",
  //  DeploymentArgs(
  //      Metadata = ObjectMetaArgs(
  //          Labels = appLabels,
  //          Namespace = ioMetaName ns
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
