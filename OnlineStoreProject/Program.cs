using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Services.ForumService;
using Infrastructure.Services.OrderService;
using Infrastructure.Services.ProductService;
using Infrastructure.Services.UserService;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineStoreProject.ErrorHandlerMiddleware;
using System;
using System.Threading.Tasks;

namespace OnlineStoreProject
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults((IFunctionsWorkerApplicationBuilder Builder) => {
                    Builder.UseMiddleware<GlobalErrorHandler>();
                })
                .ConfigureServices(Configure)
                .Build();

            host.Run();
        }

        static void Configure(HostBuilderContext Builder, IServiceCollection Services)
        {
            // DB Context
            Services.AddDbContext<OnlineStoreContext>(option =>
            {
                option.UseCosmos(
                    Environment.GetEnvironmentVariable("CosmosDb:Account", EnvironmentVariableTarget.Process), 
                    Environment.GetEnvironmentVariable("CosmosDb:Key", EnvironmentVariableTarget.Process), 
                    Environment.GetEnvironmentVariable("CosmosDb:DatabaseName", EnvironmentVariableTarget.Process)
                );
            });

            // Repositories
            Services.AddTransient(typeof(IOnlineStoreReadRepository<>), typeof(OnlineStoreReadRepository<>));
            Services.AddTransient(typeof(IOnlineStoreWriteRepository<>), typeof(OnlineStoreWriteRepository<>));

            // Services
            Services.AddScoped<IUserService, UserService>();
            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<IForumService, ForumService>();
            Services.AddScoped<IProductService, ProductService>();
        }
    }
}