using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerAllInOne.Core.Configs
{
    /// <summary>
    /// 控制台程序停止配置
    /// </summary>
    public class ConsoleStopConfig
    {
        /// <summary>
        /// 停止方法：event(事件，如ctrl+c)，input(控制台输入，如exit)，command(执行命令行，如nginx -s quit)，kill(直接kill进程)
        /// </summary>
        public required string Method { get; set; } = "event";

        /// <summary>
        /// 停止命令
        /// </summary>
        public string? Command { get; set; } = "ctrl+c";

        /// <summary>
        /// 停止超时时间，毫秒
        /// </summary>
        public int Timeout { get; set; }
    }
}
