using PresentationLayer.Extention;
using DataAccessLayer.Extension;
using BusinessLogicLayer.Extension;

namespace SkyHotelApp.Extenstion
{
    public static class ExtensionServices
    {
        public static IServiceCollection AddExtensionServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPresentationServices(configuration);

            return services;
        }
    }
}
