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
        private TreeViewModel serverTree;

        public MainWindowViewModel()
        {
            serverTree = new TreeViewModel();

            serverTree.Children = [
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
            get => serverTree;
            set => Set(ref serverTree, value);
        }

    }
}
