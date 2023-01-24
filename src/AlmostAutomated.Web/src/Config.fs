module Config

open System
open Fable.Core

/// Returns the value of a configured variable using its key.
/// Retursn empty string when the value does not exist
[<Emit("process.env[$0] ? process.env[$0] : ''")>]
let variable (key: string) : string = jsNative

/// Tries to find the value of the configured variable if it is defined or returns a given default value otherwise.
let variableOrDefault (key: string) (defaultValue: string) =
    let foundValue = variable key

    if String.IsNullOrWhiteSpace foundValue then
        defaultValue
    else
        foundValue
