namespace Pishi_Wash__Store
{
    /*
     
            ______  __   ____      ______  _____________________
           / __ ) \/ /  / __ \    / / __ \/ ___/_  __/  _/_  __/
          / __  |\  /  / / / /_  / / / / /\__ \ / /  / /  / /   
         / /_/ / / /  / /_/ / /_/ / /_/ /___/ // / _/ /  / /    
        /_____/ /_/  /_____/\____/\____//____//_/ /___/ /_/     
                                                        

     */
    internal class ViewModelLocator
    {
        private static ServiceProvider? _provider;
        private static IConfiguration? _configuration;
        public static void Init()
        {
            _configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();

            var services = new ServiceCollection();

            #region ViewModel

            services.AddTransient<mWindowViewModel>();
            services.AddTransient<SignInViewModel>();
            services.AddTransient<SignUpViewModel>();
            services.AddTransient<BrowseProductViewModel>();
            services.AddTransient<CartViewModel>();
            services.AddTransient<BrowseAdminViewModel>();
            services.AddTransient<HelpAdminViewModel>();

            #endregion

            #region Connection

            services.AddDbContext<TradeContext>(options =>
            {
                try
                {
                    var conn = _configuration.GetConnectionString(/*"RemoteConnection"*/ "LocalConnection");
                    options.UseMySql(conn, ServerVersion.AutoDetect(conn));
                }
                catch (MySqlConnector.MySqlException ex1)
                {
                    if (MessageBox.Show(ex1.Message + "\nПопробовать другой вариант?", "Нет подключения к бд", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            var conn = _configuration.GetConnectionString("RemoteConnection");
                            options.UseMySql(conn, ServerVersion.AutoDetect(conn));
                        }
                        catch (MySqlConnector.MySqlException ex2)
                        {
                            MessageBox.Show(ex2.Message, "Нет подключения к бд", MessageBoxButton.OK, MessageBoxImage.Error);
                            Process.GetCurrentProcess().Kill();
                        }
                    }
                    else
                        Process.GetCurrentProcess().Kill();
                }
            }, ServiceLifetime.Singleton);

            #endregion

            #region Services

            services.AddSingleton<PageService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<ProductService>();
            services.AddSingleton<DocumentService>();

            #endregion

            _provider = services.BuildServiceProvider();
            //foreach (var service in services)
            //{
            //    _provider.GetRequiredService(service.ServiceType);
            //}
        }
        public mWindowViewModel? mWindowViewModel => _provider?.GetRequiredService<mWindowViewModel>();
        public SignInViewModel? SignInViewModel => _provider?.GetRequiredService<SignInViewModel>();
        public SignUpViewModel? SignUpViewModel => _provider?.GetRequiredService<SignUpViewModel>();
        public BrowseProductViewModel? BrowseProductViewModel => _provider?.GetRequiredService<BrowseProductViewModel>();
        public CartViewModel? CartViewModel => _provider?.GetRequiredService<CartViewModel>();
        public BrowseAdminViewModel? BrowseAdminViewModel => _provider?.GetRequiredService<BrowseAdminViewModel>();
        public HelpAdminViewModel? HelpAdminViewModel => _provider?.GetRequiredService<HelpAdminViewModel>();
    }
}
