using Application;
using Application.Interfaces;
using Infrastructure;
using WebApi.Extensions;
using WebApi.GlobalExceptions;
using WebApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionsHandler>();
builder.Services.AddScoped<IPasswordCryptography, Cryptography>();

var app = builder.Build();

app.UseExceptionHandler(); 

app.ApplyMigrations();

app.MapControllers();

app.Run();
