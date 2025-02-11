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
        private string? _title;
        private ObservableCollection<TreeItemViewModel> _children;

        public TreeViewModel()
        {
            _children = [];
        }

        public string? Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public ObservableCollection<TreeItemViewModel> Children
        {
            get => _children;
            set => Set(ref _children, value ?? []);
        }

    }
}
