using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketClientAgent
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                Console.WriteLine("agent starting...");

                try
                {
                    var ip = args[0];
                    var port = int.Parse(args[1]);

                    var agent = new ClientAgent(ip, port);
                    Client? client = null;

                    Console.WriteLine("agent started.");

                    while (true)
                    {
                        var line = Console.ReadLine() ?? "";
                        try
                        {
                            if (line.StartsWith(":"))
                            {
                                if (line == ":exit")
                                {
                                    break;
                                }
                                else if (line == ":clear")
                                {
                                    Console.Clear();
                                }
                                else if (line == ":list")
                                {
                                    if (agent.Clients.Count == 0)
                                    {
                                        Console.WriteLine("no client.");
                                    }
                                    var i = 0;
                                    foreach (var c in agent.Clients)
                                    {
                                        Console.WriteLine("{0:D2}: {1} {2}", ++i, c.ClientName, c.Status);
                                    }
                                }
                                else if (line.StartsWith(":client "))
                                {
                                    client = agent.GetClient(line.Split(' ')[1]);
                                    if (client != null)
                                    {
                                        Console.WriteLine("change to client {0}", client.ClientName);
                                    }
                                }
                                else if (line.StartsWith(":online "))
                                {
                                    agent.OnlineClient(line.Split(' ')[1]);
                                }
                                else if (line.StartsWith(":offline "))
                                {
                                    agent.OfflineClient(line.Split(' ')[1]);
                                }
                                else if (line.StartsWith(":status "))
                                {
                                    var arg = line.Split(' ');
                                    agent.ChangeClientStatus(arg[1], (ClientStatus)int.Parse(arg[2]));
                                }
                            }
                            else
                            {
                                client?.SendAsync(line);
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("agent command error!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("agent error: {0}", ex.Message);
                }
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }

    internal static class Extensions
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T? ToObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }

    internal class ClientAgent
    {
        public string ServerIp { get; }

        public int ServerPort { get; }

        public List<Client> Clients { get; }

        public ClientAgent(string serverIp, int serverPort)
        {
            ServerIp = serverIp;
            ServerPort = serverPort;
            Clients = new List<Client>();
        }

        public void OnlineClient(string clientName)
        {
            if (Clients.Any(c => c.ClientName.Equals(clientName, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("client {0} already online!", clientName);
                return;
            }

            var client = new Client(clientName, ServerIp, ServerPort);
            client.Connect();

            _ = client.SendAsync(new
            {
                MsgType = 1,
                IdentityId = "Equipment." + clientName
            }.ToJson());

            Clients.Add(client);
        }

        public void OfflineClient(string clientName)
        {
            var client = GetClient(clientName);
            if (client != null)
            {
                _ = client.SendAsync(new
                {
                    MsgType = 3,
                    IdentityId = "Equipment." + clientName
                }.ToJson());

                client.Disconnect();
            }
        }

        public void ChangeClientStatus(string clientName, ClientStatus status)
        {
            var client = GetClient(clientName);
            if (client != null)
            {
                client.Status = status;
            }
        }

        public Client? GetClient(string clientName)
        {
            return Clients.Find(c => c.ClientName.Equals(clientName, StringComparison.OrdinalIgnoreCase));
        }
    }

    internal enum ClientStatus
    {
        Idle,
        Running,
        Pause,
        EmergencyStop,
        Error,
    }

    internal class Client
    {
        private Socket socket;
        private bool isConnected;
        public string ClientName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public ClientStatus Status { get; set; }

        public string ServerIp { get; }

        public int ServerPort { get; }

        public Client(string clientName, string serverIp, int serverPort)
        {
            ClientName = clientName;
            ServerIp = serverIp;
            ServerPort = serverPort;
        }

        public void Connect()
        {
            if (isConnected)
                return;

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(new IPEndPoint(IPAddress.Parse(ServerIp), ServerPort));
                _ = OnReceiveAsync();

                isConnected = true;
            }
            catch (Exception ex)
            {
                _ = WriteErrorAsync($"connect error: {ex.Message}");
                throw;
            }
        }

        public void Disconnect()
        {
            if (!isConnected)
                return;

            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                _ = WriteErrorAsync($"disconnect error: {ex.Message}");
            }

            isConnected = false;
        }

        public async Task SendAsync(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            var data = Encoding.UTF8.GetBytes(message);
            var dataLength = data.Length;
            var packetHeader = BitConverter.GetBytes(dataLength);
            if (!BitConverter.IsLittleEndian)
            {
                _ = packetHeader.Reverse();
            }

            var dataBuffer = new byte[4 + dataLength];
            Array.Copy(packetHeader, 0, dataBuffer, 0, packetHeader.Length);
            Array.Copy(data, 0, dataBuffer, 4, data.Length);

            await WriteInfoAsync($"sending message: {message}");
            await socket.SendAsync(dataBuffer, SocketFlags.None);
        }

        private async Task OnReceiveAsync()
        {
            var buffer = new byte[1024];
            var dataBuffer = new List<byte>();
            while (true)
            {
                try
                {
                    var len = await socket.ReceiveAsync(buffer, SocketFlags.None);
                    if (len == 0)
                        break;

                    dataBuffer.AddRange(buffer.Take(len));
                    if (dataBuffer.Count > 4)
                    {
                        var packetHeader = dataBuffer.Take(4).ToArray();
                        if (!BitConverter.IsLittleEndian)
                        {
                            packetHeader.Reverse();
                        }
                        var dataLength = BitConverter.ToInt32(packetHeader);
                        if (dataBuffer.Count >= dataLength + 4)
                        {
                            var data = dataBuffer.GetRange(4, dataLength).ToArray();
                            dataBuffer.RemoveRange(0, dataLength + 4);

                            await WriteInfoAsync($"receive message: {Encoding.UTF8.GetString(data)}");
                        }
                    }
                }
                catch (SocketException ex)
                {
                    await WriteErrorAsync($"recieve message error: {ex.Message}");
                    throw;
                }

            }
        }

        private async Task WriteInfoAsync(string message)
        {
            await ConsoleWriteAsync(message, ConsoleColor.Gray);
        }

        private async Task WriteErrorAsync(string message)
        {
            await ConsoleWriteAsync(message, ConsoleColor.Red);
        }

        private async Task ConsoleWriteAsync(string message, ConsoleColor color)
        {
            var slimlock = new SemaphoreSlim(1, 1);
            await slimlock.WaitAsync();
            try
            {
                var original = Console.ForegroundColor;
                Console.ForegroundColor = color;
                await Console.Out.WriteLineAsync($"[{DateTime.Now:HH:mm:ss.fff}]{Environment.NewLine}[{ClientName}]: {message}");
                Console.ForegroundColor = original;
            }
            finally
            {
                slimlock.Release();
            }
        }
    }
}