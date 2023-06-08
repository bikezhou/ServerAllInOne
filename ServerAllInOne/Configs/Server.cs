using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerAllInOne.Configs
{
    public class Server
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 程序路径
        /// </summary>
        public string ExePath { get; set; }

        /// <summary>
        /// 执行参数
        /// </summary>
        public string Arguments { get; set; }

        /// <summary>
        /// 支持输入
        /// </summary>
        public bool CanInput { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

    }
}
