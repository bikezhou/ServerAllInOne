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
        private ServerTreeViewModel _serverTree;

        public MainWindowViewModel()
        {
            _serverTree = new ServerTreeViewModel();

            _serverTree.Children = [
                new ServerTreeItemViewModel()
                {
                    Title = "Group",
                    IsExpanded = true,
                    Children = new ObservableCollection<ServerTreeItemViewModel>
                    {
                        new ServerTreeItemViewModel()
                        {
                            Title = "Minio Server"
                        },
                        new ServerTreeItemViewModel()
                        {
                            Title = "nginx"
                        },
                        new ServerTreeItemViewModel()
                        {
                            Title = "网盘API"
                        }
                    }
                },
                new ServerTreeItemViewModel(){
                    Title = "Other Group"
                }
            ];
        }

        public ServerTreeViewModel ServerTree
        {
            get => _serverTree;
            set => Set(ref _serverTree, value);
        }

    }
}
