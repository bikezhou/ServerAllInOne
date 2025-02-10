using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace ServerAllInOne.ViewModels.Components
{
    public class ServerTreeItemViewModel : PropertyChangedBase
    {
        private string? _title;
        private bool _isExpanded;
        private ObservableCollection<ServerTreeItemViewModel> _children;

        public ServerTreeItemViewModel()
        {
            _children = [];
        }

        public string? Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set => Set(ref _isExpanded, value);
        }

        public ObservableCollection<ServerTreeItemViewModel> Children
        {
            get => _children;
            set => Set(ref _children, value ?? []);
        }


        public void ExpandButton_Click()
        {
            IsExpanded = !IsExpanded;
        }
    }
}
