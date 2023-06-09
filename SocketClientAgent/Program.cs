using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketClientAgent
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                Console.WriteLine("socket agent starting...");

                try
                {
                    var ip = args[0];
                    var port = int.Parse(args[1]);

                    var agent = new ClientAgent(ip, port);

                    Console.WriteLine("socket agent started.");

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
                                    agent.ListClient();
                                }
                                else if (line.StartsWith(":client "))
                                {
                                    agent.SetCurrentClient(line.Split(' ')[1]);
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
                                    agent.SetClientStatus(arg[1], (ClientStatus)int.Parse(arg[2]));
                                }
                            }
                            else
                            {
                                agent.CurrentClient?.SendAsync(line);
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
}