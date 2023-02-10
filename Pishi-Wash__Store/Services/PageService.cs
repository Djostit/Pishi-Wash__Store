using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Pishi_Wash__Store.Services
{
    public class PageService
    {
        public event Action<Page>? onPageChanged;
        public void ChangePage(Page page) => onPageChanged?.Invoke(page);
    }
}
