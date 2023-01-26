module App

open Feliz
//open Pages.Templates
open Pages.Index
open Error404
open RouterComponent



open Browser.Dom

let root = ReactDOM.createRoot (document.getElementById "root")
root.render (RouterComponent())
