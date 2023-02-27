module Utilities

open Pulumi.FSharp


let toBase64 (string: string) =
    let bytes = System.Text.Encoding.UTF8.GetBytes(string)
    System.Convert.ToBase64String(bytes)

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
