module Namespace

open Pulumi.FSharp.Kubernetes.Core.V1

let ns () =
    ``namespace`` { name "almost-automated" }
