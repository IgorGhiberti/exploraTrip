using Application.Users;
using Domain.Common;
using Domain.User;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
    
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
