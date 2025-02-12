using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerAllInOne.Core.Configs
{
    /// <summary>
    /// 控制台程序配置
    /// </summary>
    public class ConsoleConfig
    {
        /// <summary>
        /// 控制台名称
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// 控制台代码页，默认936(GBK)
        /// </summary>
        public int? CodePage { get; set; } = 936;

        /// <summary>
        /// 执行程序路径
        /// </summary>
        public string? BinPath { get; set; }

        /// <summary>
        /// 命令行参数
        /// </summary>
        public string? Arguments { get; set; }

        /// <summary>
        /// 程序工作目录
        /// </summary>
        public string? WorkingDirectory { get; set; }

        /// <summary>
        /// 程序描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 程序停止配置
        /// </summary>
        public ConsoleStopConfig? StopConfig { get; set; }
    }
}
