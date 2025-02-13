using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public void Refresh(string id)
        {
            if (Servers.Count(s => s.UUID == id) == 0)
                return;

            using (var stream = File.OpenRead(this.configJsonPath))
            {
                using (var tr = new StreamReader(stream))
                {
                    using (var jr = new JsonTextReader(tr))
                    {
                        var obj = JObject.Load(jr);
                        var token = obj.SelectToken("Servers")?.First(token => token.SelectToken("UUID")?.Value<string>() == id);
                        if (token != null)
                        {
                            var server = token.ToObject<Server>();
                            if (server != null)
                            {
                                var index = Servers.FindIndex(s => s.UUID == id);
                                if (index != -1)
                                {
                                    Servers[index] = server;
                                }
                            }
                        }
                    }
                }
            }

        }
    }
}
