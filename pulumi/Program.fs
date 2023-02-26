module Program

open Pulumi.FSharp
open Pulumi.Kubernetes.Types.Inputs.Core.V1
open Pulumi.Kubernetes.Types.Inputs.Apps.V1
open Pulumi.Kubernetes.Types.Inputs.Meta.V1
open Pulumi.Kubernetes.Apps.V1
open Pulumi.Kubernetes.Core.V1
open Pulumi.Kubernetes.Types.Outputs.Meta.V1
open Pulumi.FSharp.Kubernetes.Core.V1
open Pulumi.FSharp.Kubernetes.Meta.V1.Inputs
open Pulumi.Kubernetes.Networking.V1
open Pulumi.Kubernetes.Types.Inputs.Networking.V1
open Pulumi.Kubernetes.Batch.V1
open Pulumi.Kubernetes.Types.Inputs.Batch.V1
open Pulumi.Kubernetes.Rbac.V1
open Pulumi.Kubernetes.Types.Inputs.Rbac.V1


let toBase64 (string: string) =
    let bytes = System.Text.Encoding.UTF8.GetBytes(string)
    System.Convert.ToBase64String(bytes)

let ioMetaName (res: Namespace) =
    io (Outputs.apply (fun (m: ObjectMeta) -> m.Name) res.Metadata)


let ns () =
    Namespace("almost-automated")

let db ns =
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
                                    input <| ContainerPortArgs(
                                        Name = "postgres-tcp",
                                        ContainerPortValue = 5432
                                    )
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

    let service = Service("db",
        ServiceArgs(
            Metadata = ObjectMetaArgs(
                Namespace = ioMetaName ns
            ),
            Spec = ServiceSpecArgs(
                Selector = inputMap [
                    dbLabels
                ],
                Ports = ServicePortArgs(
                    Name = "postgres-tcp",
                    Port = 5432,
                    TargetPort = "postgres-tcp"
                )
            )
        )
    )

    auth, config, service

let api ns (dbAuth: Secret) (dbConfig: ConfigMap) (dbService: Service) =
    let nsName = ioMetaName ns

    let role = Role("wait-for",
        RoleArgs(
            Metadata = ObjectMetaArgs(
                Namespace = nsName
            ),
            Rules = inputList [
                input <| PolicyRuleArgs(
                    ApiGroups = inputList [ input "" ],
                    Resources = inputList [ input "services"; input "pods"; input "jobs" ],
                    Verbs = inputList [ input "get"; input "watch"; input "listen" ]
                );
                input <| PolicyRuleArgs(
                    ApiGroups = inputList [ input "batch" ],
                    Resources = inputList [ input "services"; input "pods"; input "jobs" ],
                    Verbs = inputList [ input "get"; input "watch"; input "listen" ]
                )
            ]
        )
    )

    let roleBinding = RoleBinding("wait-for",
        RoleBindingArgs(
            Metadata = ObjectMetaArgs(
                Namespace = nsName
            ),
            Subjects = inputList [
                input <| SubjectArgs(
                    Kind = "ServiceAccount",
                    Name = "default",
                    Namespace = nsName
                )
            ],
            RoleRef = RoleRefArgs(
                Kind = "Role",
                Name = io (Outputs.apply (fun (m: ObjectMeta) -> m.Name) role.Metadata),
                ApiGroup = input "rbac.authorization.k8s.io"
            )
        )
    )

    let job = Job("migration",
        JobArgs(
            Metadata = ObjectMetaArgs(
                Namespace = nsName
            ),
            Spec = JobSpecArgs(
                BackoffLimit = 1,
                Template = PodTemplateSpecArgs(
                    Spec = PodSpecArgs(
                        InitContainers = inputList [
                            input <| ContainerArgs(
                                Name = "await-db",
                                Image = "postgres:15",
                                Command = inputList [ input "/bin/sh" ],
                                Args = inputList [
                                    input "-c";
                                    input "until pg_isready -h $(DB_HOST) -p 5432; do echo waiting for database; sleep 2; done;"
                                ],
                                Env = inputList [
                                    input <| EnvVarArgs(
                                        Name = "DB_HOST",
                                        Value = io (Outputs.apply (fun (m: ObjectMeta) -> m.Name) dbService.Metadata)
                                    );
                                ]
                            )
                        ],
                        Containers = inputList [
                            input <| ContainerArgs(
                                Name = "migration",
                                Image = "almost-migration:latest",
                                ImagePullPolicy = "IfNotPresent",
                                Command = inputList [
                                    input "dotnet"
                                ],
                                Args = inputList [ input "AlmostAutomated.Migration.dll" ],
                                Env = inputList [
                                    input <| EnvVarArgs(
                                        Name = "DB_HOST",
                                        Value = io (Outputs.apply (fun (m: ObjectMeta) -> m.Name) dbService.Metadata)
                                    );
                                    input <| EnvVarArgs(
                                        Name = "DB_USER",
                                        ValueFrom = EnvVarSourceArgs(
                                            SecretKeyRef = SecretKeySelectorArgs(
                                                Name = io (Outputs.apply (fun (m: ObjectMeta) -> m.Name) dbAuth.Metadata),
                                                Key = "POSTGRES_USER"
                                            )
                                        )
                                    );
                                    input <| EnvVarArgs(
                                        Name = "DB_PASSWORD",
                                        ValueFrom = EnvVarSourceArgs(
                                            SecretKeyRef = SecretKeySelectorArgs(
                                                Name = io (Outputs.apply (fun (m: ObjectMeta) -> m.Name) dbAuth.Metadata),
                                                Key = "POSTGRES_PASSWORD"
                                            )
                                        )
                                    )
                                ]
                            )
                        ],
                        RestartPolicy = "Never"
                    )
                )
            )
        )
    )

    let appLabels = ("app", input "api")
    
    let containerPort = 5000
    let portName = "api-tcp"
    let deployment = Deployment("api",
        DeploymentArgs(
            Metadata = ObjectMetaArgs(
                Namespace = nsName
            ),
            Spec = DeploymentSpecArgs(
                Replicas = 1,
                Selector = LabelSelectorArgs(MatchLabels = inputMap [ appLabels ]),
                Template = PodTemplateSpecArgs(
                    Metadata = ObjectMetaArgs(
                        Namespace = nsName,
                        Labels = inputMap [ appLabels ]
                    ),
                    Spec = PodSpecArgs(
                        InitContainers = inputList [
                            input <| ContainerArgs(
                                Name = "await-migration",
                                Image = "groundnuty/k8s-wait-for:v2.0",
                                Args = inputList [
                                    input "job";
                                    io (Outputs.apply (fun (m: ObjectMeta) -> m.Name) job.Metadata)
                                ]
                            )
                        ],
                        Containers = inputList [
                            input <| ContainerArgs(
                                Name = "api",
                                Image = "almost-api:latest",
                                ImagePullPolicy = "IfNotPresent",
                                Ports = inputList [
                                    input <| ContainerPortArgs(
                                        Name = portName,
                                        ContainerPortValue = containerPort
                                    )
                                ],
                                Env = inputList [
                                    input <| EnvVarArgs(
                                        Name = "DB_HOST",
                                        Value = io (Outputs.apply (fun (m: ObjectMeta) -> m.Name) dbService.Metadata)
                                    );
                                    input <| EnvVarArgs(
                                        Name = "DB_USER",
                                        ValueFrom = EnvVarSourceArgs(
                                            SecretKeyRef = SecretKeySelectorArgs(
                                                Name = io (Outputs.apply (fun (m: ObjectMeta) -> m.Name) dbAuth.Metadata),
                                                Key = "POSTGRES_USER"
                                            )
                                        )
                                    );
                                    input <| EnvVarArgs(
                                        Name = "DB_PASSWORD",
                                        ValueFrom = EnvVarSourceArgs(
                                            SecretKeyRef = SecretKeySelectorArgs(
                                                Name = io (Outputs.apply (fun (m: ObjectMeta) -> m.Name) dbAuth.Metadata),
                                                Key = "POSTGRES_PASSWORD"
                                            )
                                        )
                                    )
                                ],
                                LivenessProbe = ProbeArgs(
                                    HttpGet = HTTPGetActionArgs(
                                        Path = "/",
                                        Port = portName
                                    )
                                ),
                                ReadinessProbe = ProbeArgs(
                                    HttpGet = HTTPGetActionArgs(
                                        Path = "/",
                                        Port = portName
                                    )
                                )
                            )
                        ]
                    )
                )
            )
        )
    )

    let service = Service("api",
        ServiceArgs(
            Metadata = ObjectMetaArgs(
                Namespace = nsName
            ),
            Spec = ServiceSpecArgs(
                Selector = inputMap [
                    appLabels
                ],
                Ports = ServicePortArgs(
                    Name = portName,
                    Port = containerPort,
                    TargetPort = portName
                )
            )
        )
    )

    let ingress = Ingress("api",
        IngressArgs(
            Metadata = ObjectMetaArgs(
                Namespace = nsName
            ),
            Spec = IngressSpecArgs(
                Rules = inputList [
                    input <| IngressRuleArgs(
                        Host = "api.almostautomated.local",
                        Http = HTTPIngressRuleValueArgs(
                            Paths = inputList [
                                input <| HTTPIngressPathArgs(
                                    Path = "/api",
                                    PathType = "Prefix",
                                    Backend = IngressBackendArgs(
                                        Service = IngressServiceBackendArgs(
                                            Name = io (Outputs.apply (fun (m: ObjectMeta) -> m.Name) service.Metadata),
                                            Port = ServiceBackendPortArgs(
                                                Number = containerPort
                                            )
                                        )
                                    )
                                )
                            ]
                        )
                    )
                ]
            )
        )
    )

    {| Ingress = ingress :> obj |}


let infra () =
    let ns = ns ()
    let dbAuth, dbConfig, dbService = db ns
    let api = api ns dbAuth dbConfig dbService

    dict [
        ("namespace", ns :> obj)
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


//let infra () =
//  let appLabels = inputMap ["app", input "nginx" ]
  
//  let containers : Pulumi.InputList<ContainerArgs> = inputList [
//    input (ContainerArgs(
//       Name = "nginx",
//       Image = "nginx",
//       Ports = inputList [ input(ContainerPortArgs(ContainerPortValue = 80)) ]
//    ))
//  ]
 
//  let podSpecs = PodSpecArgs(Containers = containers)

//  let deployment = 
//    Deployment("nginx",
//      DeploymentArgs
//        (Spec = DeploymentSpecArgs
//          (Selector = LabelSelectorArgs(MatchLabels = appLabels),
//           Replicas = 1,
//           Template = 
//             PodTemplateSpecArgs
//              (Metadata = ObjectMetaArgs(Labels = appLabels),
//               Spec = podSpecs))))
  
//  let name = 
//    deployment.Metadata
//    |> Outputs.apply(fun metadata -> metadata.Name)

//  dict [("name", name :> obj)]

[<EntryPoint>]
let main _ =
  Deployment.run infra
