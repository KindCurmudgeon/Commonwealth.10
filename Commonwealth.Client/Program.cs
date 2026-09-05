using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Commonwealth.Client;
using Commonwealth.Client.Services;
using Identity.Client;
using Identity.Client.Service;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<ApiService>(client =>
{
     client.BaseAddress = new Uri("http://localhost:5082/");
     client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddHttpClient<IdentityService>(client =>
{
     client.BaseAddress = new Uri("http://localhost:5029/");
     client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddScoped<SharedDataService>();

await builder.Build().RunAsync();
