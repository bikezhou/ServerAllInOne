using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerAllInOne.Configs
{
    public class AppConfig
    {
        public static string DefaultConfigJson = "Configs\\AppConfig.json";

        public static AppConfig Default
        {
            get
            {
                return JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(DefaultConfigJson)) ?? new AppConfig();
            }
        }

        /// <summary>
        /// 程序名称
        /// </summary>
        public string Name { get; set; } = "服务一键启动";

        /// <summary>
        /// 图标位置
        /// </summary>
        public string IconPath { get; set; } = "/Icons/server_windows.ico";
    }
}
