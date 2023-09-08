using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAllInOne.ViewModels
{
    public sealed class MainViewModel : Conductor<Screen>
    {
        private string _title = "Service All In One";
        private MainContentViewModel _mainContent;

        public MainViewModel()
        {
            MainContent = new MainContentViewModel();
        }

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public MainContentViewModel MainContent
        {
            get => _mainContent;
            set => Set(ref _mainContent, value);
        }
    }
}
