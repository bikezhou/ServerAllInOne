using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace ServerAllInOne.ViewModels.Components
{
    public class TreeItemViewModel : PropertyChangedBase
    {
        private string? title;
        private bool isExpanded;
        private ObservableCollection<TreeItemViewModel> children;

        public TreeItemViewModel()
        {
            children = [];
        }

        public string? Title
        {
            get => title;
            set => Set(ref title, value);
        }

        public bool IsExpanded
        {
            get => isExpanded;
            set => Set(ref isExpanded, value);
        }

        public ObservableCollection<TreeItemViewModel> Children
        {
            get => children;
            set => Set(ref children, value ?? []);
        }


        public void ExpandButton_Click()
        {
            IsExpanded = !IsExpanded;
        }
    }
}
