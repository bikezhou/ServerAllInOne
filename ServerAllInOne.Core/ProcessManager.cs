using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ServerAllInOne.Core.Configs;
using ServerAllInOne.Core.Models;

namespace ServerAllInOne.Core
{
    public class ProcessManager
    {
        private JsonSerializerOptions serializerOptions;

        public ProcessManager()
        {
            serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
            };
        }

        public static string ConfigRootDir => Path.GetFullPath("./Config/Process");

        private string GetIdFromPath(string path)
        {
            return "/" + Path.GetRelativePath(ConfigRootDir, path).Replace("\\", "/").Trim('.');
        }

        public List<NodeItem> GetNodes() => GetNodes("/");

        public List<NodeItem> GetNodes(string parentId)
        {
            var results = new List<NodeItem>();
            var configDir = ConfigRootDir;
            var targetDir = Path.Combine(ConfigRootDir, parentId.TrimStart('/'));
            if (Directory.Exists(targetDir))
            {
                foreach (var item in Directory.GetDirectories(targetDir))
                {
                    results.Add(new NodeItem
                    {
                        Id = GetIdFromPath(item),
                        Name = Path.GetFileName(item),
                        NodeType = 1,
                        ParentId = parentId,
                        Status = 0
                    });
                }

                foreach (var item in Directory.GetFiles(targetDir, "*.json"))
                {
                    var config = default(ProcessConfig);
                    using (var stream = File.OpenRead(item))
                    {
                        config = JsonSerializer.Deserialize<ProcessConfig>(stream, serializerOptions);
                    }

                    if (config != null)
                    {
                        results.Add(new NodeItem
                        {
                            Id = GetIdFromPath(item),
                            Name = config.Name,
                            NodeType = 0,
                            ParentId = parentId,
                            Description = config.Description,
                            Status = 0
                        });
                    }
                }
            }
            return results;
        }
    }
}
