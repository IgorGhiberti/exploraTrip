using Application.Interfaces;
using Application.Services;
using Application.Users;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserServices, UserServices>();
        services.AddScoped<ITripServices, TripServices>();
        services.AddScoped<ILocalService, LocalServices>();
        return services;
    }
}