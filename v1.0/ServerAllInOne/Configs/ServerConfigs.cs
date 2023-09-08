using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerAllInOne.Configs
{
    public class ServerConfigs
    {
        public static string DefaultConfigJson = "Configs\\ServerConfigs.json";
        public static ServerConfigs Default
        {
            get
            {
                var configs = JsonConvert.DeserializeObject<ServerConfigs>(File.ReadAllText(DefaultConfigJson)) ?? new ServerConfigs();
                configs.configJsonPath = DefaultConfigJson;
                return configs;
            }
        }

        private string configJsonPath;

        public ServerConfigs()
        {

        }
        public ServerConfigs(string configJsonPath)
        {
            this.configJsonPath = configJsonPath;
        }

        public List<Server> Servers { get; set; } = new List<Server>();

        public void Save()
        {
            File.WriteAllText(this.configJsonPath, JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}
