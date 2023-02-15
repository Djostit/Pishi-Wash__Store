using DevExpress.Mvvm;
using Pishi_Wash__Store.Services;
using Pishi_Wash__Store.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pishi_Wash__Store.ViewModels
{
    public class SignInViewModel : BindableBase
    {
        private readonly UserService _userService;
        private readonly PageService _pageService;
        public string Username { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public string cAPTCHA { get; set; }
        public SignInViewModel(UserService userService, PageService pageService)
        {
            _userService = userService;
            _pageService = pageService;
        }
        public AsyncCommand SignInCommand => new(async () => 
        {
            if (await _userService.AuthorizationAsync(Username, Password))
            {
                ErrorMessage = string.Empty;
                _pageService.ChangePage(new BrowseProductPage());
            }
            ErrorMessage = "Неверный логин или пароль";
        }, bool() => 
        {
            if(string.IsNullOrWhiteSpace(Username)
                || string.IsNullOrWhiteSpace(Password)) 
            {
                ErrorMessage = "Пустые поля";
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
