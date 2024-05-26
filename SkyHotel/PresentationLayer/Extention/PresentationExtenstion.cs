using BusinessLogicLayer.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PresentationLayer.Extention
{
    public static class PresentationExtenstion
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBusinessServices(configuration);
            // services
            return services;
        }
    }
}
