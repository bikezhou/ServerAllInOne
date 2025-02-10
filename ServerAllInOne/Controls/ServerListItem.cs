using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerAllInOne.Controls
{
    public class ServerListItem
    {
        /// <summary>
        /// 服务GUID
        /// </summary>
        public string? UUID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 运行状态
        /// </summary>
        public bool Running { get; set; }

        /// <summary>
        /// 进程ID
        /// </summary>
        public int ProcessId { get; set; }
    }
}
