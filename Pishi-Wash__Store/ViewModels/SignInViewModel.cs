using DevExpress.Mvvm;
using Pishi_Wash__Store.Services;
using Pishi_Wash__Store.Views;
using System;
using System.Collections.Generic;
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
        public SignInViewModel(UserService userService, PageService pageService)
        {
            _userService = userService;
            _pageService = pageService;
        }
        public AsyncCommand AuthCommand => new(async () => 
        {
            await _userService.Auth(Username, Password);
        });
        public DelegateCommand SignInLater => new(() => { _pageService.ChangePage(new BrowseProductPage()); });
    }
}
