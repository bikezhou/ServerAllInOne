using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerAllInOne.Core.Models
{
    public class ConsoleItemModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 运行状态，0-停止，1-运行中
        /// </summary>
        public int Status { get; set; }
    }
}
