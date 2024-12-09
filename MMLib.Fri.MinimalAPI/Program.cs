using MMLib.Fri.MinimalAPI;
using MMLib.Fri.MinimalAPI.Features.Contacts;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddContacts();

var app = builder.Build();

app.UseCors("AllowAll");

app.UseRequestTiming();

app.UseOutputCache();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseRateLimiter();

app.MapContacts();

app.Run();