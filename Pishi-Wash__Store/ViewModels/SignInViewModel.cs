using System.Threading;
using System.Windows;

namespace Pishi_Wash__Store.ViewModels
{
    public class SignInViewModel : BindableBase
    {
        private readonly UserService _userService;
        private readonly PageService _pageService;
        public string Username { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorMessageButton { get; set; }
        public string cAPTCHA { get; set; }
        public SignInViewModel(UserService userService, PageService pageService)
        {
            _userService = userService;
            _pageService = pageService;
        }
        public AsyncCommand SignInCommand => new(async () => 
        {
            await Task.Run(async () =>
            {
                if (await _userService.AuthorizationAsync(Username, Password))
                {
                    await Application.Current.Dispatcher.InvokeAsync(async () => // Только часть метода должна находиться в потоке пользовательского интерфейса.
                    {
                        ErrorMessageButton = string.Empty;
                        _pageService.ChangePage(new BrowseProductPage());
                    });
                }
                else
                {
                    await Application.Current.Dispatcher.InvokeAsync(async () =>
                    {
                        ErrorMessageButton = "Неверный логин или пароль";
                    });
                }
            });
        }, bool() => 
        {
            if(string.IsNullOrWhiteSpace(Username)
                || string.IsNullOrWhiteSpace(Password)) 
            {
                ErrorMessage = "Пустые поля";
                ErrorMessageButton = string.Empty;
            }
            else 
            {
                ErrorMessage = string.Empty;
            }
            if (ErrorMessage.Equals(string.Empty))
                return true; return false;
        });
        public DelegateCommand SignUpCommand => new(() =>
        {
            _pageService.ChangePage(new SignUpPage());
        });
        public DelegateCommand SignInLaterCommand => new(() => 
        { 
            _pageService.ChangePage(new BrowseProductPage()); 
        });
    }
}
