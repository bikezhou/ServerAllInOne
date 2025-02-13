using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerAllInOne.Core.Models
{
    public class NodeItem
    {
        public required string Id { get; set; }

        public string? ParentId { get; set; }

        /// <summary>
        /// 节点类型：0-进程，1-分组
        /// </summary>
        public int NodeType { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        /// <summary>
        /// 状态：0-停止，1-运行中
        /// </summary>
        public int Status { get; set; }
    }
}
