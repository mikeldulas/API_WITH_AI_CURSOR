var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () => Results.Ok(new
{
    Service = "GatewayApi",
    Description = "Gateway for microservices.",
    Endpoints = new[]
    {
        "/api/services",
        "/proxy/biodata -> BiodataService.Api (configure reverse proxy if needed)"
    }
}));

app.MapGet("/api/services", () => Results.Ok(new[]
{
    new { Name = "GatewayApi", Url = "http://localhost:5000" },
    new { Name = "BiodataService.Api", Url = "http://localhost:5100" }
}));

app.Run();
