using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketClientAgent
{

    internal class ClientAgent
    {
        public string ServerIp { get; }

        public int ServerPort { get; }

        public string Module { get; set; } = "Equipment";

        public Client? CurrentClient { get; private set; }

        public List<Client> Clients { get; }

        public ClientAgent(string serverIp, int serverPort)
        {
            ServerIp = serverIp;
            ServerPort = serverPort;
            Clients = new List<Client>();
        }

        /// <summary>
        /// 上线客户端
        /// </summary>
        /// <param name="clientName"></param>
        public void OnlineClient(string clientName)
        {
            var client = Clients.FirstOrDefault(c => c.ClientName.Equals(clientName, StringComparison.OrdinalIgnoreCase));
            if (client == null)
            {
                client = new Client(clientName, ServerIp, ServerPort);
                Clients.Add(client);

                Clients.Sort((a, b) => a.ClientName.CompareTo(b.ClientName));
            }
            if (!client.IsConnected)
            {
                client.Connect();

                _ = client.SendAsync(new
                {
                    MsgType = 1,
                    IdentityId = $"{Module}.{clientName}"
                }.ToJson());
            }
            else
            {
                Console.WriteLine("client {0} already online!", clientName);
            }
        }

        /// <summary>
        /// 下线客户端
        /// </summary>
        /// <param name="clientName"></param>
        public void OfflineClient(string clientName)
        {
            var client = GetClient(clientName);
            if (client != null)
            {
                _ = client.SendAsync(new
                {
                    MsgType = 3,
                    IdentityId = $"{Module}.{clientName}"
                }.ToJson());

                client.Disconnect();
            }
        }

        /// <summary>
        /// 显示客户端列表
        /// </summary>
        public void ListClient()
        {
            if (Clients.Count == 0)
            {
                Console.WriteLine("-- empty client --.");
            }
            var i = 0;
            foreach (var c in Clients)
            {
                Console.WriteLine("{0:D2}: {1} {2} {3}", ++i, c.ClientName, c.Status, c.IsConnected ? "online" : "offline");
            }
        }

        /// <summary>
        /// 改变客户端状态
        /// </summary>
        /// <param name="clientName"></param>
        /// <param name="status"></param>
        public void SetClientStatus(string clientName, ClientStatus status)
        {
            var client = GetClient(clientName);
            if (client != null)
            {
                client.SetStatus(status);
            }
        }

        /// <summary>
        /// 设置当前客户端
        /// </summary>
        /// <param name="clientName"></param>
        public void SetCurrentClient(string clientName)
        {
            CurrentClient = GetClient(clientName);
            Console.WriteLine("change current client {0}", CurrentClient != null ? CurrentClient.ClientName : "null");
        }

        public Client? GetClient(string clientName)
        {
            return Clients.Find(c => c.ClientName.Equals(clientName, StringComparison.OrdinalIgnoreCase));
        }
    }

}
