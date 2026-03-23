using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tcc.Application.Interfaces.IRepositories;
using Tcc.Infrastructure.Repositories;

namespace Tcc.Infrastructure.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserQueryRepository, UserQueryRepository>();
        services.AddScoped<IUserCommandRepository, UserCommandRepository>();
        services.AddScoped<IPersonQueryRepository, PersonQueryRepository>();
        services.AddScoped<IPersonCommandRepository, PersonCommandRepository>();
        services.AddScoped<IApprovalQueryRepository, ApprovalQueryRepository>();
        services.AddScoped<IApprovalCommandRepository, ApprovalCommandRepository>();


        return services;
    }
}