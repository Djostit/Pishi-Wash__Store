using DevExpress.Mvvm.POCO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pishi_Wash__Store.Data;
using Pishi_Wash__Store.Services;
using Pishi_Wash__Store.ViewModels;
using System.IO;

namespace Pishi_Wash__Store
{
    internal class ViewModelLocator
    {
        private static ServiceProvider _provider;
        public static IConfiguration Configuration { get; private set; }
        public static void Init()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            var services = new ServiceCollection();

            #region ViewModel

            services.AddTransient<mWindowViewModel>();
            services.AddTransient<SignInViewModel>();
            services.AddTransient<BrowseProductViewModel>();

            #endregion

            #region Connection

            services.AddDbContext<DataContext>(options =>
            {
                var conn = Configuration.GetConnectionString("DefaultConnection");
                options.UseMySql(conn, ServerVersion.AutoDetect(conn));
            });

            #endregion

            #region Services

            services.AddSingleton<PageService>();
            services.AddSingleton<UserService>();

            #endregion
            
            _provider = services.BuildServiceProvider();
            foreach (var service in services)
            {
                _provider.GetRequiredService(service.ServiceType);
            }
        }
        public mWindowViewModel mWindowViewModel => _provider.GetRequiredService<mWindowViewModel>();
        public SignInViewModel SignInViewModel => _provider.GetRequiredService<SignInViewModel>();
        public BrowseProductViewModel BrowseProductViewModel => _provider.GetRequiredService<BrowseProductViewModel>();
    }
}
