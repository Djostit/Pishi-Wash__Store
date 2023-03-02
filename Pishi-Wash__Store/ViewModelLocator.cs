using Pishi_Wash__Store.Data.Db;

namespace Pishi_Wash__Store
{
    internal class ViewModelLocator
    {
        private static ServiceProvider _provider;
        public static IConfiguration _configuration;
        public static void Init()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            _configuration = builder.Build();

            var services = new ServiceCollection();

            #region ViewModel

            services.AddTransient<mWindowViewModel>();
            services.AddTransient<SignInViewModel>();
            services.AddTransient<SignUpViewModel>();
            services.AddTransient<BrowseProductViewModel>();
            services.AddTransient<CartViewModel>();

            #endregion

            #region Connection

            //services.AddDbContext<DataContext>(options =>
            //{
            //    try
            //    {
            //        var conn = _configuration.GetConnectionString("LocalConnection");
            //        options.UseMySql(conn, ServerVersion.AutoDetect(conn));
            //    }
            //    catch (MySqlConnector.MySqlException)
            //    {
            //        try
            //        {
            //            var conn = _configuration.GetConnectionString("RemoteConnection");
            //            options.UseMySql(conn, ServerVersion.AutoDetect(conn));
            //        }
            //        catch (MySqlConnector.MySqlException)
            //        {
            //            Debug.WriteLine("Ошибка.");
            //        }

            //    }
            //}, ServiceLifetime.Transient);

            services.AddDbContext<TradeContext>(options =>
            {
                try
                {
                    var conn = _configuration.GetConnectionString("LocalConnection");
                    options.UseMySql(conn, ServerVersion.AutoDetect(conn));
                }
                catch (MySqlConnector.MySqlException)
                {
                    try
                    {
                        var conn = _configuration.GetConnectionString("RemoteConnection");
                        options.UseMySql(conn, ServerVersion.AutoDetect(conn));
                    }
                    catch (MySqlConnector.MySqlException)
                    {
                        Debug.WriteLine("Ошибка.");
                        Application.Current.Shutdown();
                    }

                }
            }, ServiceLifetime.Singleton);

            #endregion

            #region Services

            services.AddSingleton<PageService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<ProductService>();

            #endregion

            _provider = services.BuildServiceProvider();
            foreach (var service in services)
            {
                _provider.GetRequiredService(service.ServiceType);
            }
        }
        public mWindowViewModel mWindowViewModel => _provider.GetRequiredService<mWindowViewModel>();
        public SignInViewModel SignInViewModel => _provider.GetRequiredService<SignInViewModel>();
        public SignUpViewModel SignUpViewModel => _provider.GetRequiredService<SignUpViewModel>();
        public BrowseProductViewModel BrowseProductViewModel => _provider.GetRequiredService<BrowseProductViewModel>();
        public CartViewModel CartViewModel => _provider.GetRequiredService<CartViewModel>();
    }
}
