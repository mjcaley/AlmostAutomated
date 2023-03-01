module Web

open Pulumi.FSharp
open Pulumi.Kubernetes.Core.V1
open Pulumi.FSharp.Kubernetes.Meta.V1.Inputs
open Pulumi.FSharp.Kubernetes.Apps.V1.Inputs
open Pulumi.FSharp.Kubernetes.Core.V1.Inputs
open Pulumi.FSharp.Kubernetes.Rbac.V1.Inputs
open Pulumi.FSharp.Kubernetes.Networking.V1.Inputs
open Pulumi.Kubernetes.Types.Inputs.Core.V1
open Pulumi.FSharp.Kubernetes.Core.V1
open Pulumi.FSharp.Kubernetes.Apps.V1
open Pulumi.FSharp.Kubernetes.Rbac.V1
open Pulumi.FSharp.Kubernetes.Networking.V1
open Utilities


let web ns  =
    let appLabels = [ ("app", input "web") ]
    let webPort = 8000
    let webPortName = "web-tcp"

    let deployment =
        deployment {
            name "web"
            objectMeta { ``namespace`` (namespaceName ns) }

            deploymentSpec {
                replicas 1
                labelSelector { matchLabels appLabels }

                podTemplateSpec {
                    objectMeta {
                        ``namespace`` (namespaceName ns)
                        labels appLabels
                    }

                    podSpec {
                        containers
                            [ container {
                                  name "web"
                                  image "almost-web:latest"
                                  imagePullPolicy "IfNotPresent"

                                  ports
                                      [ containerPort {
                                            name webPortName
                                            containerPortValue webPort
                                        } ]
                              } ]
                    }
                }
            }
        }

    let service =
        service {
            name "web"
            objectMeta { ``namespace`` (namespaceName ns) }

            serviceSpec {
                selector appLabels

                ports
                    [ servicePort {
                          name webPortName
                          port webPort
                          targetPort webPortName
                      } ]
            }
        }

    let ingress =
        ingress {
            name "web"
            objectMeta { ``namespace`` (namespaceName ns) }

            ingressSpec {
                rules
                    [ ingressRule {
                          host "almostautomated.localhost"

                          hTTPIngressRuleValue {
                              paths
                                  [ hTTPIngressPath {
                                        path "/"
                                        pathType "Prefix"

                                        ingressBackend {
                                            ingressServiceBackend {
                                                name (serviceName service)
                                                serviceBackendPort { number webPort }
                                            }
                                        }
                                    } ]
                          }
                      } ]
            }
        }

    {| Ingress = ingress :> obj |}
