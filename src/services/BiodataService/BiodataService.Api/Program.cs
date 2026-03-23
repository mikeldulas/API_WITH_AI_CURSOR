using System.Threading.RateLimiting;
using BiodataService.Application;
using BiodataService.Application.Contracts;
using BiodataService.Application.Models;
using BiodataService.Infrastructure;
using BiodataService.Infrastructure.Persistence;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddFixedWindowLimiter("fixed", limiter =>
    {
        limiter.Window = TimeSpan.FromMinutes(1);
        limiter.PermitLimit = 100;
        limiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiter.QueueLimit = 0;
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRateLimiter();
app.UseHttpsRedirection();

app.MapGet("/api/biodata", async (IBiodataService service, CancellationToken ct) =>
    Results.Ok(await service.GetAllAsync(ct)))
    .RequireRateLimiting("fixed");

app.MapGet("/api/biodata/{id:guid}", async (Guid id, IBiodataService service, CancellationToken ct) =>
{
    var result = await service.GetByIdAsync(id, ct);
    return result is null ? Results.NotFound() : Results.Ok(result);
}).RequireRateLimiting("fixed");

app.MapPost("/api/biodata", async (BiodataRequest request, IBiodataService service, CancellationToken ct) =>
{
    var created = await service.CreateAsync(request, ct);
    return Results.Created($"/api/biodata/{created.Id}", created);
}).RequireRateLimiting("fixed");

app.MapPut("/api/biodata/{id:guid}", async (Guid id, BiodataRequest request, IBiodataService service, CancellationToken ct) =>
{
    var updated = await service.UpdateAsync(id, request, ct);
    return updated ? Results.NoContent() : Results.NotFound();
}).RequireRateLimiting("fixed");

app.MapDelete("/api/biodata/{id:guid}", async (Guid id, IBiodataService service, CancellationToken ct) =>
{
    var deleted = await service.DeleteAsync(id, ct);
    return deleted ? Results.NoContent() : Results.NotFound();
}).RequireRateLimiting("fixed");

app.MapPost("/api/biodata/db-init", async (BiodataDbContext dbContext, CancellationToken ct) =>
{
    await dbContext.Database.MigrateAsync(ct);
    return Results.Ok(new { Message = "Database migrated." });
});

app.Run();
