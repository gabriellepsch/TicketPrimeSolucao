using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using billet_2.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<Routes>("#app");

await builder.Build().RunAsync();
