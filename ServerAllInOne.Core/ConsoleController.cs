using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerAllInOne.Core.Models;
using ServerAllInOne.Core.Services;

namespace ServerAllInOne.Core
{
    public class ConsoleController
    {
        private readonly ConsoleProcessService processService;

        public static ConsoleController Default { get; } = new ConsoleController();

        public ConsoleController()
        {
            processService = ConsoleProcessService.Default;
        }

        public List<ConsoleGroupModel> GetGroupList(string groupId = "root")
        {
            return processService.GetGroupList(groupId);
        }

        public List<ConsoleItemModel> GetItemList(string groupId = "root")
        {
            return processService.GetItemList(groupId);
        }
    }
}
