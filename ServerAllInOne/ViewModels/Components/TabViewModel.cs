using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace ServerAllInOne.ViewModels.Components
{
    public class TabViewModel : PropertyChangedBase
    {
        private ObservableCollection<TabItemViewModel> tabItems;

        public TabViewModel()
        {
            tabItems = [];
        }

        public ObservableCollection<TabItemViewModel> TabItems
        {
            get => tabItems;
            set => Set(ref tabItems, value);
        }

    }
}
