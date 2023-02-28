namespace Pishi_Wash__Store.ViewModels
{
    public class SignUpViewModel : BindableBase
    {
        private readonly PageService _pageService;
        public SignUpViewModel(PageService pageService)
        {
            _pageService = pageService;
        }
        public AsyncCommand SignUpCommand => new(async () =>
        {
            await Task.Delay(1440);
            Debug.WriteLine("Проверка регистрации");
        });
        public DelegateCommand SignInCommand => new(() =>
        {
            _pageService.ChangePage(new SingInPage());
        });
    }
}
