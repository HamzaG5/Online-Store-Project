using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Services.ForumService;
using Infrastructure.Services.OrderService;
using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace OnlineStoreProject
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
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
                    Builder.Configuration["CosmosDb:Account"],
                    Builder.Configuration["CosmosDb:Key"],
                    Builder.Configuration["CosmosDb:DatabaseName"]
                );
            });

            // Repositories
            Services.AddTransient(typeof(IOnlineStoreReadRepository<>), typeof(OnlineStoreReadRepository<>));
            Services.AddTransient(typeof(IOnlineStoreWriteRepository<>), typeof(OnlineStoreWriteRepository<>));

            // Services
            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<IForumService, ForumService>();
        }
    }
}