/*
     
            ______  __   ____      ______  _____________________
           / __ ) \/ /  / __ \    / / __ \/ ___/_  __/  _/_  __/
          / __  |\  /  / / / /_  / / / / /\__ \ / /  / /  / /   
         / /_/ / / /  / /_/ / /_/ / /_/ /___/ // / _/ /  / /    
        /_____/ /_/  /_____/\____/\____//____//_/ /___/ /_/     
                                                        

*/
namespace Pishi_Wash__Store
{
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
                switch (Environment.MachineName)
                {
                    case "DJOSTIT":
                        var connr = _configuration.GetConnectionString("RemoteConnection");
                        options.UseMySql(connr, ServerVersion.AutoDetect(connr));
                        break;
                    case "LAPTOPDJOSTIT":
                        var connl = _configuration.GetConnectionString("LocalConnection");
                        options.UseMySql(connl, ServerVersion.AutoDetect(connl));
                        break;
                    default:
                        MessageBox.Show("Я вас не знаю.", "Неизвестный компьютер", MessageBoxButton.OK, MessageBoxImage.Error);
                        Process.GetCurrentProcess().Kill();
                        break;
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
