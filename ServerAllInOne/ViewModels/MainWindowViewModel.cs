using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using ServerAllInOne.ViewModels.Components;

namespace ServerAllInOne.ViewModels
{
    public class MainWindowViewModel : PropertyChangedBase
    {
        private TreeViewModel _serverTree;

        public MainWindowViewModel()
        {
            _serverTree = new TreeViewModel();

            _serverTree.Children = [
                new TreeItemViewModel()
                {
                    Title = "Group",
                    IsExpanded = true,
                    Children = new ObservableCollection<TreeItemViewModel>
                    {
                        new TreeItemViewModel()
                        {
                            Title = "Minio Server"
                        },
                        new TreeItemViewModel()
                        {
                            Title = "nginx"
                        },
                        new TreeItemViewModel()
                        {
                            Title = "网盘API"
                        }
                    }
                },
                new TreeItemViewModel(){
                    Title = "Other Group"
                }
            ];
        }

        public TreeViewModel ServerTree
        {
            get => _serverTree;
            set => Set(ref _serverTree, value);
        }

    }
}
