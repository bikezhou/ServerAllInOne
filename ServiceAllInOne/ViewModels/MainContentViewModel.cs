using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAllInOne.ViewModels
{
    public sealed class MainContentViewModel : Screen
    {
        public ObservableCollection<ServicePanelViewModel> ServicePanels { get; private set; }

        public MainContentViewModel()
        {
            ServicePanels = new ObservableCollection<ServicePanelViewModel>();
#if DEBUG
            for (int i = 0; i < 20; i++)
            {
                ServicePanels.Add(new ServicePanelViewModel
                {
                    Name = "Visual Studio Code + " + i,
                });
            }
#endif
        }
    }
}
