using Azure.Identity;
using IdentityProvider.Endpoints;
using Utilities;

var builder = WebApplication.CreateBuilder(args);

var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
{
    ManagedIdentityClientId = builder.Configuration["AZURE_CLIENT_ID"]
});

builder.AddBlobService(credential);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();


var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "ConfigOriginsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins ?? [])
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();


app.UseRouting();
app.UseCors("ConfigOriginsPolicy");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();

