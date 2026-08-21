using Azure.Identity;
using Commonwealth.Server.Endpoints;
using Commonwealth.Server.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add Blob Service
var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
{
    ManagedIdentityClientId = builder.Configuration["AZURE_CLIENT_ID"]
});
builder.AddBlobService(credential);

builder.AuthInitialize();

// CORS STUFF - Like Paolo GameStore
     // builder.Services.AddCors();
// CORS STUFF - 2 Works with Localhost and with Azure (not using policy.AllowAnyOrigin)
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "blazor", policy =>
    {
        //  policy.AllowAnyOrigin();
        policy.WithOrigins("http://localhost:5257");
        policy.SetIsOriginAllowedToAllowWildcardSubdomains();
        policy.WithMethods("GET", "POST", "PUT", "DELETE");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors("blazor");

app.MapEndpoints();

await app.InitializeServer();

app.Run();
