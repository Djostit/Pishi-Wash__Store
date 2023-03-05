namespace Pishi_Wash__Store
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ViewModelLocator.Init();
            base.OnStartup(e);
        }
        //protected override void OnExit(ExitEventArgs e)
        //{
        //    UserSetting.Default.Save();
        //    base.OnExit(e);
        //}
    }
}
