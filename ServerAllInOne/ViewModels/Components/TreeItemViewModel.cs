using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Caliburn.Micro;
using ServerAllInOne.ViewModels.Components.Events;

namespace ServerAllInOne.ViewModels.Components
{
    public class TreeItemViewModel : PropertyChangedBase
    {
        private string? title;
        private ImageSource? icon;
        private bool isExpanded;
        private bool isSelected;
        private bool isLeaf;
        private object? tag;
        private readonly ObservableCollection<TreeItemViewModel> children;

        private bool isLoadedChildren;

        private readonly IEventAggregator eventAggregator;

        public TreeItemViewModel()
        {
            eventAggregator = IoC.Get<IEventAggregator>();
            children = [];
        }

        public string? Title
        {
            get => title;
            set => Set(ref title, value);
        }

        public ImageSource? Icon
        {
            get => icon;
            set => Set(ref icon, value);
        }

        public bool IsExpanded
        {
            get => isExpanded;
            set => Set(ref isExpanded, value);
        }

        public bool IsSelected
        {
            get => isSelected;
            set => Set(ref isSelected, value);
        }

        public bool IsLeaf
        {
            get => isLeaf;
            set => Set(ref isLeaf, value);
        }

        public object? Tag
        {
            get => tag;
            set => Set(ref tag, value);
        }

        public ObservableCollection<TreeItemViewModel> Children
        {
            get => children;
        }


        public void ToggleExpand()
        {
            if (!IsExpanded && !isLoadedChildren)
            {
                Task.Run(async () =>
                {
                    await eventAggregator.PublishOnBackgroundThreadAsync(new TreeItemChildrenLoadEvent() { Item = this });
                    isLoadedChildren = true;
                });
            }

            IsExpanded = !IsExpanded;
        }
    }
}
