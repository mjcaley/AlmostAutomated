module App

open Feliz
open RouterComponent



open Browser.Dom

let root = ReactDOM.createRoot (document.getElementById "root")
root.render (RouterComponent())
