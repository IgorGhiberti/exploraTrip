using Application;
using Application.Interfaces;
using Infrastructure;
using Microsoft.OpenApi.Models;
using WebApi.Extensions;
using WebApi.GlobalExceptions;
using WebApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

//Dependency Injection
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionsHandler>();
builder.Services.AddScoped<IPasswordCryptography, Cryptography>();
builder.Services.AddScoped<ICache, Cache>();
builder.Services.AddMemoryCache();

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ExploraTrip.API",
        Version = "v1",
        Contact = new OpenApiContact { Email = "igorgh@outlook.com", Name = "Igor Ghiberti" },
        Description = "API feita para o sistema Explora Trip, cujo objetivo é organizar as viagens dos usuários."
    });

    var xmlFile = "WebAPi.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

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



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(); 

// Habilitar CORS
app.UseCors("AllowReactApp");

app.ApplyMigrations();

app.MapControllers();

app.Run();
