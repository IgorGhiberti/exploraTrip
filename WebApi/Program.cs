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

// Configuração de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174") // Portas do Vite
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

app.UseExceptionHandler();

// Habilitar CORS
app.UseCors("AllowReactApp");

app.ApplyMigrations();

app.MapControllers();

app.Run();
