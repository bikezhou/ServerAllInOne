using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Caliburn.Micro;
using ServerAllInOne.Core;
using ServerAllInOne.Core.Models;
using ServerAllInOne.Extensions;
using ServerAllInOne.ViewModels.Components;
using ServerAllInOne.ViewModels.Components.Events;

namespace ServerAllInOne.ViewModels
{
    public class MainWindowViewModel : PropertyChangedBase, IHandle<TreeItemChildrenLoadEvent>
    {
        private readonly TreeViewModel serverTree;
        private readonly ConsoleController consoleController;
        private readonly IEventAggregator eventAggregator;

        public MainWindowViewModel()
        {
            serverTree = new TreeViewModel();
            consoleController = ConsoleController.Default;

            eventAggregator = IoC.Get<IEventAggregator>();
            eventAggregator.SubscribeOnBackgroundThread(this);

            _ = LoadAsync();
        }

        public TreeViewModel ServerTree
        {
            get => serverTree;
        }

        private async Task LoadAsync()
        {
            await Task.Run(() =>
            {
                var groups = consoleController.GetGroupList();
                foreach (var item in groups)
                {
                    serverTree.Children.Add(new TreeItemViewModel
                    {
                        Title = item.Name,
                        Tag = item
                    });
                }

                var items = consoleController.GetItemList();
                foreach (var item in items)
                {
                    serverTree.Children.Add(new TreeItemViewModel
                    {
                        Title = item.Name,
                        IsLeaf = true,
                        Icon = new BitmapImage(new Uri("/Assets/Images/console.png", UriKind.RelativeOrAbsolute)),
                        Tag = item
                    });
                }
            });
        }

        public async Task HandleAsync(TreeItemChildrenLoadEvent message, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                var treeItem = message.Item;
                if (treeItem.Tag is ConsoleGroupModel group)
                {
                    if (group.Id != null)
                    {
                        var groups = consoleController.GetGroupList(group.Id);
                        var items = consoleController.GetItemList(group.Id);

                        Execute.OnUIThread(() =>
                        {
                            foreach (var item in groups)
                            {
                                treeItem.Children.Add(new TreeItemViewModel
                                {
                                    Title = item.Name,
                                    Tag = item
                                });
                            }
                            foreach (var item in items)
                            {
                                treeItem.Children.Add(new TreeItemViewModel
                                {
                                    Title = item.Name,
                                    IsLeaf = true,
                                    Icon = new BitmapImage(new Uri("/Assets/Images/console.png", UriKind.RelativeOrAbsolute)),
                                    Tag = item
                                });
                            }
                        });
                    }
                }
            }, cancellationToken);
        }
    }
}
