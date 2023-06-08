using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    client.Connect(new IPEndPoint(IPAddress.Parse(args[0]), int.Parse(args[1])));

                    Task.Run(() =>
                    {
                        var buffer = new byte[1024];
                        var dataBuffer = new List<byte>();
                        while (true)
                        {
                            var len = client.Receive(buffer);
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

                                    Console.WriteLine(Encoding.UTF8.GetString(data));
                                }
                            }
                        }
                    });

                    Console.WriteLine("Connect server success.");
                    while (true)
                    {
                        var message = Console.ReadLine() ?? "";
                        if (message == ":exit")
                            break;

                        if (message.StartsWith(":identity "))
                        {
                            message = JsonConvert.SerializeObject(new
                            {
                                MsgType = 1,
                                IdentityId = "Equipment." + message.Split(' ')[1]
                            });
                        }
                        else if (message.StartsWith(":unknown"))
                        {
                            message = JsonConvert.SerializeObject(new
                            {
                                MsgType = 0,
                                Data = "Hello World!"
                            });
                        }

                        if (string.IsNullOrEmpty(message))
                            continue;

                        var data = Encoding.UTF8.GetBytes(message);
                        var dataLength = data.Length;
                        var packetHeader = BitConverter.GetBytes(dataLength);
                        if (!BitConverter.IsLittleEndian)
                        {
                            packetHeader.Reverse();
                        }

                        var dataBuffer = new byte[4 + dataLength];
                        Array.Copy(packetHeader, 0, dataBuffer, 0, packetHeader.Length);
                        Array.Copy(data, 0, dataBuffer, 4, data.Length);

                        client.Send(dataBuffer);
                    }

                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Connect server error. Exception: {0}", ex.Message);
                }
            }
            Console.WriteLine("Hello, World!");
            Console.ReadKey();
        }
    }
}