using DevExpress.Mvvm;
using Pishi_Wash__Store.Services;
using Pishi_Wash__Store.Views;
using System.Windows.Controls;

namespace Pishi_Wash__Store.ViewModels
{
    public class mWindowViewModel : BindableBase
    {
        private readonly PageService _pageService;
        public Page PageSource { get; set; }
        public mWindowViewModel(PageService pageService)
        {
            _pageService = pageService;

            _pageService.onPageChanged += (page) => PageSource = page;

            _pageService.ChangePage(new SingInPage());
        }
    }
}
