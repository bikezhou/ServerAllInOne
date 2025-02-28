﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerAllInOne.Configs
{
    public class Server
    {
        /// <summary>
        /// GUID
        /// </summary>
        public string UUID { get; set; }

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
        /// 工作目录
        /// </summary>
        public string WorkingDirectory { get; set; }

        /// <summary>
        /// 支持输入
        /// </summary>
        public bool CanInput { get; set; }

        /// <summary>
        /// 控制台代码页，默认936（GBK）
        /// </summary>
        public int? CodePage { get; set; } = 936;

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 快速菜单
        /// </summary>
        public ContextMenu[] ContextMenu { get; set; }

        /// <summary>
        /// 配置文件
        /// </summary>
        public ServerConfig[] Configs { get; set; }

        /// <summary>
        /// 服务停止配置
        /// </summary>
        public ServerStopConfig StopConfig { get; set; }

    }

    public class ServerConfig
    {
        public string Name { get; set; }

        public string Path { get; set; }
    }

    public class ContextMenu
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 菜单命令
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public ContextMenu[] Items { get; set; }
    }

    public class ServerStopConfig
    {
        /// <summary>
        /// 服务停止方法：event(如ctrl+c)，input(如exit)，command(如nginx.exe -s quit)，kill
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 服务停止指令
        /// </summary>
        public string Command { get; set; }

        public int Timeout { get; set; } = 1000;
    }
}
