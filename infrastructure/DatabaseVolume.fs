module DatabaseVolume

open Pulumi.FSharp.Kubernetes.Meta.V1.Inputs
open Pulumi.FSharp.Kubernetes.Core.V1.Inputs
open Pulumi.FSharp.Kubernetes.Core.V1
open Utilities


let dbVolume ns =
    let pv =
        persistentVolume {
            name "db"
            objectMeta { ``namespace`` (namespaceName ns) }

            persistentVolumeSpec {
                storageClassName "local-path"
                capacity [ ("storage", "1Gi") ]
                accessModes [ "ReadWriteMany" ]
                hostPathVolumeSource { path "/data/db" }
            }
        }

    let pvc =
        persistentVolumeClaim {
            name "db"

            objectMeta {
                ``namespace`` (namespaceName ns)
                annotations [ ("pulumi.com/skipAwait", "true") ]
            }

            persistentVolumeClaimSpec {
                resourceRequirements { requests [ "storage", "1Gi" ] }
                accessModes [ "ReadWriteMany" ]
            }
        }

    pvc
