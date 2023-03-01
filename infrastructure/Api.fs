module Api

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


let api ns dbAuth dbConfig dbService migrationJob =
    let role =
        role {
            name "wait-for"
            objectMeta { ``namespace`` (namespaceName ns) }

            rules
                [ policyRule {
                      apiGroups [ "" ]
                      resources [ "services"; "pods"; "jobs" ]
                      verbs [ "get"; "watch"; "list" ]
                  }
                  policyRule {
                      apiGroups [ "batch" ]
                      resources [ "services"; "pods"; "jobs" ]
                      verbs [ "get"; "watch"; "list" ]
                  } ]
        }

    let roleBinding =
        roleBinding {
            name "wait-for"
            objectMeta { ``namespace`` (namespaceName ns) }

            subjects
                [ subject {
                      kind "ServiceAccount"
                      name "default"
                      ``namespace`` (namespaceName ns)
                  } ]

            roleRef {
                kind "Role"
                name (roleName role)
                apiGroup "rbac.authorization.k8s.io"
            }
        }

    let appLabels = [ ("app", input "api") ]
    let apiPort = 5000
    let apiPortName = "api-tcp"

    let deployment =
        deployment {
            name "api"
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
                        serviceAccount "default"

                        initContainers
                            [ container {
                                  name "await-migration"
                                  image "groundnuty/k8s-wait-for:v2.0"
                                  args [ "job"; "$(JOB)" ]

                                  env
                                      [ envVar {
                                            name "JOB"
                                            value (jobName migrationJob)
                                        } ]
                              } ]

                        containers
                            [ container {
                                  name "api"
                                  image "almost-api:latest"
                                  imagePullPolicy "IfNotPresent"

                                  ports
                                      [ containerPort {
                                            name apiPortName
                                            containerPortValue apiPort
                                        } ]

                                  env
                                      [ envVar {
                                            name "DB_HOST"
                                            value (serviceName dbService)
                                        }
                                        envVar {
                                            name "DB_USER"

                                            envVarSource {
                                                secretKeySelector {
                                                    key "POSTGRES_USER"
                                                    name (secretName dbAuth)
                                                }
                                            }
                                        }
                                        envVar {
                                            name "DB_PASSWORD"

                                            envVarSource {
                                                secretKeySelector {
                                                    key "POSTGRES_PASSWORD"
                                                    name (secretName dbAuth)
                                                }
                                            }
                                        } ]

                                  livenessProbe (ProbeArgs(HttpGet = HTTPGetActionArgs(Path = "/", Port = apiPortName)))

                                  readinessProbe (
                                      ProbeArgs(HttpGet = HTTPGetActionArgs(Path = "/", Port = apiPortName))
                                  )
                              } ]
                    }
                }
            }
        }

    let service =
        service {
            name "api"
            objectMeta { ``namespace`` (namespaceName ns) }

            serviceSpec {
                selector appLabels

                ports
                    [ servicePort {
                          name apiPortName
                          port apiPort
                          targetPort apiPortName
                      } ]
            }
        }

    let ingress =
        ingress {
            name "api"
            objectMeta { ``namespace`` (namespaceName ns) }

            ingressSpec {
                rules
                    [ ingressRule {
                          host "api.almostautomated.localhost"

                          hTTPIngressRuleValue {
                              paths
                                  [ hTTPIngressPath {
                                        path "/api"
                                        pathType "Prefix"

                                        ingressBackend {
                                            ingressServiceBackend {
                                                name (serviceName service)
                                                serviceBackendPort { number apiPort }
                                            }
                                        }
                                    } ]
                          }
                      } ]
            }
        }

    {| Ingress = ingress :> obj |}
