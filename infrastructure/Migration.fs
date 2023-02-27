module Migration

open Pulumi.FSharp.Kubernetes.Meta.V1.Inputs
open Pulumi.FSharp.Kubernetes.Batch.V1.Inputs
open Pulumi.FSharp.Kubernetes.Core.V1.Inputs
open Pulumi.FSharp.Kubernetes.Batch.V1
open Utilities


let migrationJob ns dbService dbAuth =
    let job = job {
            name "migration"
            objectMeta {
                ``namespace`` (namespaceName ns)

            }

            jobSpec {
                backoffLimit 1
                podTemplateSpec {
                    podSpec {
                        initContainers [
                            container {
                                name "await-db"
                                image "postgres:15"
                                command [ "/bin/sh" ]
                                args [
                                    "-c";
                                    "until pg_isready -h $(DB_HOST) -p 5432; do echo waiting for database; sleep 2; done;"
                                ]
                                env [
                                    envVar {
                                        name "DB_HOST"
                                        value (serviceName dbService)
                                    }
                                ]
                            }
                        ]
                        containers [
                            container {
                                name "migration"
                                image "almost-migration:latest"
                                imagePullPolicy "IfNotPresent"
                                command [ "dotnet" ]
                                args [ "AlmostAutomated.Migration.dll" ]
                                env [
                                    envVar {
                                        name "DB_HOST"
                                        value (serviceName dbService)
                                    };
                                    envVar {
                                        name "DB_USER"
                                        envVarSource {
                                            secretKeySelector {
                                                key "POSTGRES_USER"
                                                name (secretName dbAuth)
                                            }
                                        }
                                    };
                                    envVar {
                                        name "DB_PASSWORD"
                                        envVarSource {
                                            secretKeySelector {
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

    job
