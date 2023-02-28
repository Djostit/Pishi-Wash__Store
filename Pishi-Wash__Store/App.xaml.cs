namespace Pishi_Wash__Store
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ViewModelLocator.Init();
        }
        //protected override void OnExit(ExitEventArgs e)
        //{
        //    UserSetting.Default.Save();
        //    base.OnExit(e);
        //}
    }
}
