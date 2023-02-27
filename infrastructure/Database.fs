module Database

open Pulumi.FSharp
open Pulumi.Kubernetes.Core.V1
open Pulumi.FSharp.Kubernetes.Meta.V1.Inputs
open Pulumi.FSharp.Kubernetes.Apps.V1.Inputs
open Pulumi.FSharp.Kubernetes.Core.V1.Inputs
open Pulumi.FSharp.Kubernetes.Core.V1
open Pulumi.FSharp.Kubernetes.Apps.V1
open Utilities
open DatabaseVolume


let db (ns: Namespace) =
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

    let pvc = dbVolume ns

    let dbLabels = [ ("app", input "db") ]

    let deployment = deployment {
        name "db"
        objectMeta { ``namespace`` (namespaceName ns) }

        deploymentSpec {
            replicas 1
            labelSelector {
                matchLabels dbLabels
            }
            podTemplateSpec {
                objectMeta {
                    ``namespace`` (namespaceName ns)
                    labels dbLabels
                }
                podSpec {
                    containers [
                        container {
                            name "db"
                            image "postgres:15"
                            imagePullPolicy "IfNotPresent"
                            ports [
                                containerPort {
                                    name "postgres-tcp"
                                    containerPortValue 5432
                                }
                            ]
                            envFrom [
                                envFromSource {
                                    Pulumi.FSharp.Kubernetes.Core.V1.Inputs.configMapEnvSource {
                                        name (configMapName config)
                                    }
                                };
                                envFromSource {
                                    Pulumi.FSharp.Kubernetes.Core.V1.Inputs.secretEnvSource {
                                        name (secretName auth)
                                    }
                                }
                            ]
                            volumeMounts [
                                volumeMount {
                                    name "postgresdata"
                                    mountPath "/var/lib/postgresql/data"
                                }
                            ]
                        }
                    ]
                    volumes [
                        volume {
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

        serviceSpec {
            selector dbLabels
            ports [
                servicePort {
                    name "postgres-tcp"
                    port 5432
                    targetPort "postgres-tcp"
                }
            ]
        }
    }

    auth, config, service

