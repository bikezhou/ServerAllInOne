using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace ServerAllInOne.ViewModels.Components
{
    public class TreeViewModel : PropertyChangedBase
    {
        private string? title;
        private ObservableCollection<TreeItemViewModel> children;

        public TreeViewModel()
        {
            children = [];
        }

        public string? Title
        {
            get => title;
            set => Set(ref title, value);
        }

        public ObservableCollection<TreeItemViewModel> Children
        {
            get => children;
            set => Set(ref children, value ?? []);
        }

    }
}
