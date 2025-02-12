using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using ServerAllInOne.ViewModels;

namespace ServerAllInOne
{
    public sealed class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer? container;

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            container = new SimpleContainer();

            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();

            container.PerRequest<MainWindowViewModel>();
        }

        protected override object? GetInstance(Type service, string key)
        {
            return container?.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container?.GetAllInstances(service) ?? [];
        }

        protected override void BuildUp(object instance)
        {
            container?.BuildUp(instance);
        }
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewForAsync<MainWindowViewModel>();
        }
    }
}
