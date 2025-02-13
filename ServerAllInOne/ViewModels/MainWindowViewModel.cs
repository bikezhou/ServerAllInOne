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
        private readonly IEventAggregator eventAggregator;
        private readonly ProcessManager processManager;

        public MainWindowViewModel(IEventAggregator eventAggregator, ProcessManager processManager)
        {
            serverTree = new TreeViewModel();

            this.processManager = processManager;

            this.eventAggregator = eventAggregator;
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
                var items = processManager.GetNodes();
                foreach (var item in items)
                {
                    serverTree.Children.Add(new TreeItemViewModel
                    {
                        Title = item.Name,
                        IsLeaf = item.NodeType == 0,
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
                if (treeItem.Tag is NodeItem parent)
                {
                    var items = processManager.GetNodes(parent.Id);
                    Execute.OnUIThread(() =>
                    {
                        foreach (var item in items)
                        {
                            treeItem.Children.Add(new TreeItemViewModel
                            {
                                Title = item.Name,
                                IsLeaf = item.NodeType == 0,
                                Tag = item
                            });
                        }
                    });
                }
            }, cancellationToken);
        }
    }
}
