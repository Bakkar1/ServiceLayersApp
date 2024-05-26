using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DataAccessLayer.Extension;
using BusinessLogicLayer.Repositories.General;
using MediatR;
using BusinessLogicLayer.Features.Commands.Add;

namespace BusinessLogicLayer.Extension
{
    public static class BusinessExtension
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataServices(configuration);
            // services
            services.AddScoped<ITodoService, TodoService>();

            //services.AddTransient<IValidator<AddItemCommand>, AddItemValidator>();
            // Register your command handler
            services.AddTransient<IRequestHandler<AddTodoCommand, bool>, AddTodoCommandHandler>();

            return services;
        }
    }
}
