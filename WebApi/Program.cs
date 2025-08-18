using Application;
using Application.Users;
using Domain.Common;
using Domain.User;
using Infrastructure;
using Infrastructure.Repositories;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

//TODO dependecy injection através de IServiceCollection pra cada classlib
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();

var app = builder.Build();

app.ApplyMigrations();

app.MapControllers();

app.Run();
