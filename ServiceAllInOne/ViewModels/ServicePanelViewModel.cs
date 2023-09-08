using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAllInOne.ViewModels
{
    public sealed class ServicePanelViewModel : Screen
    {
        private string _name;

        /// <summary>
        /// 服务名称
        /// </summary>
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }
    }
}
