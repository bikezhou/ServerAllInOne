using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerAllInOne.Core.Models;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using ServerAllInOne.Core.Configs;

namespace ServerAllInOne.Core.Services
{
    internal class ConsoleProcessService
    {
        public static ConsoleProcessService Default { get; } = new ConsoleProcessService();

        private readonly Dictionary<string, string> groupIdMapping = [];
        private readonly Dictionary<string, string> itemIdMapping = [];

        private readonly Dictionary<string, ConsoleProcess> processMapping = [];

        private readonly IDeserializer deserializer;

        private ConsoleProcessService()
        {
            deserializer = new DeserializerBuilder()
               .WithNamingConvention(CamelCaseNamingConvention.Instance) // 使用驼峰命名法
               .Build();
        }

        internal List<ConsoleGroupModel> GetGroupList(string groupId = "root")
        {
            string configDir = Path.GetFullPath(Consts.RootConfigDir);
            var result = new List<ConsoleGroupModel>();
            if (Directory.Exists(configDir))
            {
                var targetDir = configDir;
                if (!string.IsNullOrEmpty(groupId) && !groupId.Equals("root", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var pair = groupIdMapping.Where(a => a.Value == groupId).First();
                        targetDir = Path.Combine(configDir, pair.Key);
                    }
                    catch (InvalidOperationException)
                    {
                        return result;
                    }
                }

                foreach (var dir in Directory.GetDirectories(targetDir))
                {
                    var relative = Path.GetRelativePath(configDir, dir);
                    if (!groupIdMapping.ContainsKey(relative))
                    {
                        groupIdMapping[relative] = Guid.NewGuid().ToString("N");
                    }

                    result.Add(new ConsoleGroupModel
                    {
                        Id = groupIdMapping[relative],
                        Name = Path.GetFileName(dir)
                    });
                }
            }
            return result;
        }

        internal List<ConsoleItemModel> GetItemList(string groupId)
        {
            string configDir = Path.GetFullPath(Consts.RootConfigDir);
            var result = new List<ConsoleItemModel>();
            if (Directory.Exists(configDir))
            {
                var targetDir = configDir;
                if (!string.IsNullOrEmpty(groupId) && !groupId.Equals("root", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var pair = groupIdMapping.Where(a => a.Value == groupId).First();
                        targetDir = Path.Combine(configDir, pair.Key);
                    }
                    catch (InvalidOperationException)
                    {
                        return result;
                    }
                }

                foreach (var file in Directory.GetFiles(targetDir, "*.yaml"))
                {
                    var relative = Path.GetRelativePath(configDir, file);
                    if (!itemIdMapping.ContainsKey(relative))
                    {
                        itemIdMapping[relative] = Guid.NewGuid().ToString("N");
                    }

                    ConsoleConfig config;

                    using (var stream = File.OpenRead(file))
                    {
                        using var reader = new StreamReader(stream);
                        config = deserializer.Deserialize<ConsoleConfig>(reader);
                    }

                    var id = itemIdMapping[relative];
                    var item = new ConsoleItemModel
                    {
                        Id = id,
                        Name = config.Name,
                        Description = config.Description,
                    };

                    if (processMapping.TryGetValue(id, out var process))
                    {
                        process.Config = config;
                        item.Status = process.Running ? 1 : 0;
                    }

                    result.Add(item);
                }
            }

            return result;
        }
    }
}
