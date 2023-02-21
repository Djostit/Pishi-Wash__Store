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
            services.AddTransient<SignUpViewModel>();
            services.AddTransient<BrowseProductViewModel>();

            #endregion

            #region Connection

            services.AddDbContext<DataContext>(options =>
            {
                try 
                {
                    var conn = Configuration.GetConnectionString("LocalConnection");
                    options.UseMySql(conn, ServerVersion.AutoDetect(conn));
                }
                catch (MySqlConnector.MySqlException)
                {
                    var conn = Configuration.GetConnectionString("RemoteConnection");
                    options.UseMySql(conn, ServerVersion.AutoDetect(conn));
                }
            }, ServiceLifetime.Transient);

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
    }
}
