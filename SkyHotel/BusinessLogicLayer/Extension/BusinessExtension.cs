using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DataAccessLayer.Extension;
using BusinessLogicLayer.Services.General;

namespace BusinessLogicLayer.Extension
{
    public static class BusinessExtension
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataServices(configuration);
            // services
            services.AddScoped<ITodoService, TodoService>();
            return services;
        }
    }
}
