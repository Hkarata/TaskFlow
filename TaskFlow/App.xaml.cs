using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TaskFlow.Data;
using TaskFlow.Repositories;
using TaskFlow.Services;

namespace TaskFlow
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }
        public App()
        {
            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            // Register services
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddDbContext<AppDbContext>();
            services.AddSingleton<ITodoListRepository, TodoListRepository>();
            services.AddSingleton<ITodoRepository, TodoRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
        }
    }

}
