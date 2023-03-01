module Program

open Microsoft.AspNetCore.Components.WebAssembly.Hosting
open Microsoft.Extensions.DependencyInjection
open System
open System.Net.Http


[<EntryPoint>]
let main args =
    let builder = WebAssemblyHostBuilder.CreateDefault(args)
    builder.RootComponents.Add<Main.App>("#main")
    
    builder.Services.AddScoped<HttpClient>(fun _ ->
        let apiBaseUrl = builder.Configuration["ApiBase"]
        new HttpClient(BaseAddress = Uri apiBaseUrl)
    )
    |> ignore

    builder.Build().RunAsync() |> ignore
    0
