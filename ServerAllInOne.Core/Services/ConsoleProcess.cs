using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerAllInOne.Core.Configs;

namespace ServerAllInOne.Core.Services
{
    internal class ConsoleProcess
    {
        public required ConsoleConfig Config { get; set; }

        public bool Running { get; set; }

    }
}
