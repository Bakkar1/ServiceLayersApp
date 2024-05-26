using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DataAccessLayer.Model;

namespace DataAccessLayer.Extension
{
    public static class DataExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            string conString = "server=(localdb)\\MSSQLLocalDB;database=testDb;Trusted_Connection=true;MultipleActiveResultSets = true";
            services.AddDbContextPool<AppDbContext>(
                        options => options.UseSqlServer(conString)//configuration.GetConnectionString("YsserConnString"))
                    );



            services.AddIdentityCore<DotNetUser>(
                options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 4;
                    options.User.RequireUniqueEmail = true;
                }
             )
            .AddEntityFrameworkStores<AppDbContext>();
            //.AddDefaultTokenProviders();

            return services;
        }
    }
}
