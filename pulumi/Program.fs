module Program

open Pulumi.FSharp
open Pulumi.FSharp.Kubernetes.Core.V1
open Pulumi.FSharp.Kubernetes.Meta.V1.Inputs
open Pulumi.FSharp.Kubernetes.Apps.V1.Inputs


let toBase64 (string: string) =
    let bytes = System.Text.Encoding.UTF8.GetBytes(string)
    System.Convert.ToBase64String(bytes)

let ioMetaName (res: Pulumi.Kubernetes.Core.V1.Namespace) =
    io (Outputs.apply (fun (m: Pulumi.Kubernetes.Types.Outputs.Meta.V1.ObjectMeta) -> m.Name) res.Metadata)


let namespaceName (ns: Pulumi.Kubernetes.Core.V1.Namespace) =
    Outputs.apply (fun (m: Pulumi.Kubernetes.Types.Outputs.Meta.V1.ObjectMeta) -> m.Name) ns.Metadata

let configMapName (config: Pulumi.Kubernetes.Core.V1.ConfigMap) =
    Outputs.apply (fun (m: Pulumi.Kubernetes.Types.Outputs.Meta.V1.ObjectMeta) -> m.Name) config.Metadata

let jobName (job: Pulumi.Kubernetes.Batch.V1.Job) =
    Outputs.apply (fun (m: Pulumi.Kubernetes.Types.Outputs.Meta.V1.ObjectMeta) -> m.Name) job.Metadata

let pvcName (pvc: Pulumi.Kubernetes.Core.V1.PersistentVolumeClaim) =
    Outputs.apply (fun (m: Pulumi.Kubernetes.Types.Outputs.Meta.V1.ObjectMeta) -> m.Name) pvc.Metadata

let roleName (role: Pulumi.Kubernetes.Rbac.V1.Role) =
    Outputs.apply (fun (m: Pulumi.Kubernetes.Types.Outputs.Meta.V1.ObjectMeta) -> m.Name) role.Metadata

let secretName (secret: Pulumi.Kubernetes.Core.V1.Secret) =
    Outputs.apply (fun (m: Pulumi.Kubernetes.Types.Outputs.Meta.V1.ObjectMeta) -> m.Name) secret.Metadata

let serviceName (service: Pulumi.Kubernetes.Core.V1.Service) =
    Outputs.apply (fun (m: Pulumi.Kubernetes.Types.Outputs.Meta.V1.ObjectMeta) -> m.Name) service.Metadata


let ns () =
    ``namespace`` {
        name "almost-automated"
    }

let db (ns: Pulumi.Kubernetes.Core.V1.Namespace) =
    let config = configMap {
        name "db"

        objectMeta {
            ``namespace`` (namespaceName ns)
        }
        data [ ("POSTGRES_DB", "almostapidb") ]
    }

    let auth = secret {
        name "db"
        objectMeta {
            ``namespace`` (namespaceName ns)
        }

        data [
            ("POSTGRES_USER", toBase64 "almostapidb");
            ("POSTGRES_PASSWORD", toBase64 "password");
        ]
    }

    let pv = persistentVolume {
        name "db"
        objectMeta {
            ``namespace`` (namespaceName ns)
        }

        Pulumi.FSharp.Kubernetes.Core.V1.Inputs.persistentVolumeSpec {
            storageClassName "local-path"
            capacity [ ("storage", "1Gi") ]
            accessModes [ "ReadWriteMany" ]
            Pulumi.FSharp.Kubernetes.Core.V1.Inputs.hostPathVolumeSource {
                path "/data/db"
            }
        }
    }

    let pvc = persistentVolumeClaim {
        name "db"
        objectMeta {
            ``namespace`` (namespaceName ns)
            annotations [ ("pulumi.com/skipAwait", "true") ]
        }

        Pulumi.FSharp.Kubernetes.Core.V1.Inputs.persistentVolumeClaimSpec {
            Pulumi.FSharp.Kubernetes.Core.V1.Inputs.resourceRequirements {
                requests [ "storage", "1Gi"]
            }
            accessModes [ "ReadWriteMany" ]
        }
    }

    let dbLabels = [ ("app", input "db") ]

    let deployment = Pulumi.FSharp.Kubernetes.Apps.V1.deployment {
        name "db"
        objectMeta { ``namespace`` (namespaceName ns) }

        deploymentSpec {
            replicas 1
            labelSelector {
                matchLabels dbLabels
            }
            Pulumi.FSharp.Kubernetes.Core.V1.Inputs.podTemplateSpec {
                objectMeta {
                    ``namespace`` (namespaceName ns)
                    labels dbLabels
                }
                Pulumi.FSharp.Kubernetes.Core.V1.Inputs.podSpec {
                    containers [
                        Pulumi.FSharp.Kubernetes.Core.V1.Inputs.container {
                            name "db"
                            image "postgres:15"
                            imagePullPolicy "IfNotPresent"
                            ports [
                                Pulumi.FSharp.Kubernetes.Core.V1.Inputs.containerPort {
                                    name "postgres-tcp"
                                    containerPortValue 5432
                                }
                            ]
                            envFrom [
                                Pulumi.FSharp.Kubernetes.Core.V1.Inputs.envFromSource {
                                    Pulumi.FSharp.Kubernetes.Core.V1.Inputs.configMapEnvSource {
                                        name (configMapName config)
                                    }
                                };
                                Pulumi.FSharp.Kubernetes.Core.V1.Inputs.envFromSource {
                                    Pulumi.FSharp.Kubernetes.Core.V1.Inputs.secretEnvSource {
                                        name (secretName auth)
                                    }
                                }
                            ]
                            volumeMounts [
                                Pulumi.FSharp.Kubernetes.Core.V1.Inputs.volumeMount {
                                    name "postgresdata"
                                    mountPath "/var/lib/postgresql/data"
                                }
                            ]
                        }
                    ]
                    volumes [
                        Pulumi.FSharp.Kubernetes.Core.V1.Inputs.volume {
                            name "postgresdata"
                            Pulumi.FSharp.Kubernetes.Core.V1.Inputs.persistentVolumeClaimVolumeSource {
                                claimName (pvcName pvc)
                            }
                        }
                    ]
                }
            }
        }
    }

    let service = service {
        name "db"
        objectMeta { ``namespace`` (namespaceName ns) }

        Pulumi.FSharp.Kubernetes.Core.V1.Inputs.serviceSpec {
            selector dbLabels
            ports [
                Pulumi.FSharp.Kubernetes.Core.V1.Inputs.servicePort {
                    name "postgres-tcp"
                    port 5432
                    targetPort "postgres-tcp"
                }
            ]
        }
    }

    auth, config, service

let api ns (dbAuth: Pulumi.Kubernetes.Core.V1.Secret) (dbConfig: Pulumi.Kubernetes.Core.V1.ConfigMap) (dbService: Pulumi.Kubernetes.Core.V1.Service) =
    let nsName = ioMetaName ns

    let role = Pulumi.FSharp.Kubernetes.Rbac.V1.role {
        name "wait-for"
        objectMeta { ``namespace`` (namespaceName ns) }

        rules [
            Pulumi.FSharp.Kubernetes.Rbac.V1.Inputs.policyRule {
                apiGroups [ "" ]
                resources [ "services"; "pods"; "jobs" ]
                verbs [ "get"; "watch"; "list" ]
            };
            Pulumi.FSharp.Kubernetes.Rbac.V1.Inputs.policyRule {
                apiGroups [ "batch" ]
                resources [ "services"; "pods"; "jobs" ]
                verbs [ "get"; "watch"; "list" ]
            }
        ]
    }

    let roleBinding = Pulumi.FSharp.Kubernetes.Rbac.V1.roleBinding {
        name "wait-for"
        objectMeta { ``namespace`` (namespaceName ns) }

        subjects [
            Pulumi.FSharp.Kubernetes.Rbac.V1.Inputs.subject {
                kind "ServiceAccount"
                name "default"
                ``namespace`` (namespaceName ns)
            }
        ]
        Pulumi.FSharp.Kubernetes.Rbac.V1.Inputs.roleRef {
            kind "Role"
            name (roleName role)
            apiGroup "rbac.authorization.k8s.io"
        }
    }

    let job = Pulumi.FSharp.Kubernetes.Batch.V1.job {
        name "migration"
        objectMeta { ``namespace`` (namespaceName ns) }

        Pulumi.FSharp.Kubernetes.Batch.V1.Inputs.jobSpec {
            backoffLimit 1
            Pulumi.FSharp.Kubernetes.Core.V1.Inputs.podTemplateSpec {
                Pulumi.FSharp.Kubernetes.Core.V1.Inputs.podSpec {
                    initContainers [
                        Pulumi.FSharp.Kubernetes.Core.V1.Inputs.container {
                            name "await-db"
                            image "postgres:15"
                            command [ "/bin/sh" ]
                            args [
                                "-c";
                                "until pg_isready -h $(DB_HOST) -p 5432; do echo waiting for database; sleep 2; done;"
                            ]
                            env [
                                Pulumi.FSharp.Kubernetes.Core.V1.Inputs.envVar {
                                    name "DB_HOST"
                                    value (serviceName dbService)
                                }
                            ]
                        }
                    ]
                    containers [
                        Pulumi.FSharp.Kubernetes.Core.V1.Inputs.container {
                            name "migration"
                            image "almost-migration:latest"
                            imagePullPolicy "IfNotPresent"
                            command [ "dotnet" ]
                            args [ "AlmostAutomated.Migration.dll" ]
                            env [
                                Pulumi.FSharp.Kubernetes.Core.V1.Inputs.envVar {
                                    name "DB_HOST"
                                    value (serviceName dbService)
                                };
                                Pulumi.FSharp.Kubernetes.Core.V1.Inputs.envVar {
                                    name "DB_USER"
                                    Pulumi.FSharp.Kubernetes.Core.V1.Inputs.envVarSource {
                                        Pulumi.FSharp.Kubernetes.Core.V1.Inputs.secretKeySelector {
                                            key "POSTGRES_USER"
                                            name (secretName dbAuth)
                                        }
                                    }
                                };
                                Pulumi.FSharp.Kubernetes.Core.V1.Inputs.envVar {
                                    name "DB_PASSWORD"
                                    Pulumi.FSharp.Kubernetes.Core.V1.Inputs.envVarSource {
                                        Pulumi.FSharp.Kubernetes.Core.V1.Inputs.secretKeySelector {
                                            key "POSTGRES_PASSWORD"
                                            name (secretName dbAuth)
                                        }
                                    }
                                };
                            ]
                        }
                    ]
                    restartPolicy "Never"
                }
            }
        }
    }

    let appLabels = [ ("app", input "api") ]
    let containerPort = 5000
    let portName = "api-tcp"

    let deployment = Pulumi.FSharp.Kubernetes.Apps.V1.deployment {
        name "api"
        objectMeta { ``namespace`` (namespaceName ns) }

        deploymentSpec {
            replicas 1
            labelSelector {
                matchLabels appLabels
            }
            Pulumi.FSharp.Kubernetes.Core.V1.Inputs.podTemplateSpec {
                objectMeta {
                    ``namespace`` (namespaceName ns)
                    labels appLabels
                }
                Pulumi.FSharp.Kubernetes.Core.V1.Inputs.podSpec {
                    serviceAccount "default"
                    initContainers [
                        Pulumi.FSharp.Kubernetes.Core.V1.Inputs.container {
                            name "await-migration"
                            image "groundnuty/k8s-wait-for:v2.0"
                            args [ "job"; "$(JOB)" ]
                            env [
                                Pulumi.FSharp.Kubernetes.Core.V1.Inputs.envVar {
                                    name "JOB"
                                    value (jobName job)
                                }
                            ]
                        }
                    ]
                    containers [
                        Pulumi.FSharp.Kubernetes.Core.V1.Inputs.container {
                            name "api"
                            image "almost-api:latest"
                            imagePullPolicy "IfNotPresent"
                            ports [
                                Pulumi.FSharp.Kubernetes.Core.V1.Inputs.containerPort {
                                    name portName
                                    containerPortValue containerPort
                                }
                            ]
                            env [
                                Pulumi.FSharp.Kubernetes.Core.V1.Inputs.envVar {
                                    name "DB_HOST"
                                    value (serviceName dbService)
                                };
                                Pulumi.FSharp.Kubernetes.Core.V1.Inputs.envVar {
                                    name "DB_USER"
                                    Pulumi.FSharp.Kubernetes.Core.V1.Inputs.envVarSource {
                                        Pulumi.FSharp.Kubernetes.Core.V1.Inputs.secretKeySelector {
                                            key "POSTGRES_USER"
                                            name (secretName dbAuth)
                                        }
                                    }
                                };
                                Pulumi.FSharp.Kubernetes.Core.V1.Inputs.envVar {
                                    name "DB_PASSWORD"
                                    Pulumi.FSharp.Kubernetes.Core.V1.Inputs.envVarSource {
                                        Pulumi.FSharp.Kubernetes.Core.V1.Inputs.secretKeySelector {
                                            key "POSTGRES_PASSWORD"
                                            name (secretName dbAuth)
                                        }
                                    }
                                };
                            ]
                            livenessProbe (Pulumi.Kubernetes.Types.Inputs.Core.V1.ProbeArgs(
                                HttpGet = Pulumi.Kubernetes.Types.Inputs.Core.V1.HTTPGetActionArgs(
                                    Path = "/",
                                    Port = portName
                                )
                            ))
                            readinessProbe (Pulumi.Kubernetes.Types.Inputs.Core.V1.ProbeArgs(
                                HttpGet = Pulumi.Kubernetes.Types.Inputs.Core.V1.HTTPGetActionArgs(
                                    Path = "/",
                                    Port = portName
                                )
                            ))
                        }
                    ]
                }
            }
        }
    }

    let service = service {
        name "api"
        objectMeta { ``namespace`` (namespaceName ns) }

        Pulumi.FSharp.Kubernetes.Core.V1.Inputs.serviceSpec {
            selector appLabels
            ports [
                Pulumi.FSharp.Kubernetes.Core.V1.Inputs.servicePort {
                    name portName
                    port containerPort
                    targetPort portName
                }
            ]
        }
    }

    let ingress = Pulumi.FSharp.Kubernetes.Networking.V1.ingress {
        name "api"
        objectMeta { ``namespace`` (namespaceName ns) }

        Pulumi.FSharp.Kubernetes.Networking.V1.Inputs.ingressSpec {
            rules [
                Pulumi.FSharp.Kubernetes.Networking.V1.Inputs.ingressRule {
                    host "api.almostautomated.local"
                    Pulumi.FSharp.Kubernetes.Networking.V1.Inputs.hTTPIngressRuleValue {
                        paths [
                            Pulumi.FSharp.Kubernetes.Networking.V1.Inputs.hTTPIngressPath {
                                path "/api"
                                pathType "Prefix"
                                Pulumi.FSharp.Kubernetes.Networking.V1.Inputs.ingressBackend {
                                    Pulumi.FSharp.Kubernetes.Networking.V1.Inputs.ingressServiceBackend {
                                        name (serviceName service)
                                        Pulumi.FSharp.Kubernetes.Networking.V1.Inputs.serviceBackendPort {
                                            number containerPort
                                        }
                                    }
                                }
                            }
                        ]
                    }
                }
            ]
        }
    }

    {| Ingress = ingress :> obj |}

let infra () =
    let ns = ns ()

    let dbAuth, dbConfig, dbService = db ns
    let api = api ns dbAuth dbConfig dbService

    dict [
        ("namespace", ns :> obj)
    ]

[<EntryPoint>]
let main _ =
  Deployment.run infra
