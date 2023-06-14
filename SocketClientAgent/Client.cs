﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace SocketClientAgent
{
    internal enum ClientStatus
    {
        Available,
        Wait,
        Running,
        Pause,
        EmergencyStop,
        Error,
    }

    internal class Client
    {
        private Socket socket;
        private bool isConnected;
        private bool isManualDisconnect = false;

        public string Module { get; set; } = "Equipment";

        public string ClientName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public ClientStatus Status { get; private set; }

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IsConnected
        {
            get => isConnected;
        }

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

            socket?.Dispose();

            isManualDisconnect = false;

            _ = WriteInfoAsync($"connecting... -> {ServerIp}:{ServerPort}");

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(new IPEndPoint(IPAddress.Parse(ServerIp), ServerPort));
                Task.Run(async () =>
                {
                    try
                    {
                        await OnReceiveAsync();
                    }
                    catch (Exception ex)
                    {
                        isConnected = false;
                        await WriteInfoAsync($"receive message error:: {ex.Message}");
                    }
                    finally
                    {
                        if (!isManualDisconnect)
                        {
                            await Task.Run(async () =>
                            {
                                await Task.Delay(3000);
                                Connect();
                            });
                        }
                    }
                });

                isConnected = true;
                _ = SendIdentityAsync();
            }
            catch (Exception ex)
            {
                _ = WriteErrorAsync($"connect error: {ex.Message}");

                Task.Run(async () =>
                {
                    await Task.Delay(3000);
                    Connect();
                });
            }
        }

        public void Disconnect()
        {
            if (!isConnected)
                return;

            isManualDisconnect = true;

            _ = SendOfflineAsync();

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

        public void SetStatus(ClientStatus status)
        {
            Status = status;
            _ = SendStatusAsync();
        }

        public async Task SendAsync(string message)
        {
            if (string.IsNullOrEmpty(message) || !isConnected)
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

            await WriteInfoAsync($"-> {message}");
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

                            var message = Encoding.UTF8.GetString(data);
                            await WriteInfoAsync($"<- {message}");

                            try
                            {
                                var jObj = JObject.Parse(message);

                                if (jObj.TryGetValue("msgtype", out int type))
                                {
                                    switch (type)
                                    {
                                        case 1:
                                            await SendIdentityAsync();
                                            break;
                                        case 4:
                                            if (jObj.TryGetValue("command", out string? command))
                                            {
                                                if (!string.IsNullOrEmpty(command))
                                                {
                                                    switch (command.ToLower())
                                                    {
                                                        case "start":
                                                            SetStatus(ClientStatus.Running);
                                                            break;
                                                        case "pause":
                                                            SetStatus(ClientStatus.Pause);
                                                            break;
                                                        case "abort":
                                                            SetStatus(ClientStatus.EmergencyStop);
                                                            break;
                                                        case "status": // 状态上报
                                                            await SendStatusAsync();
                                                            break;
                                                    }
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                await WriteErrorAsync("parse message error!");
                            }
                        }
                    }
                }
                catch (SocketException ex)
                {
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
                await Console.Out.WriteLineAsync($"[{DateTime.Now:HH:mm:ss.fff}] [{ClientName}]: {message}");
                Console.ForegroundColor = original;
            }
            finally
            {
                slimlock.Release();
            }
        }

        public async Task SendIdentityAsync()
        {
            await SendAsync(new
            {
                MsgType = 1,
                IdentityId = $"{Module}.{ClientName}"
            }.ToJson());
        }

        public async Task SendOnlineAsync()
        {
            await SendAsync(new
            {
                MsgType = 2,
                IdentityId = $"{Module}.{ClientName}"
            }.ToJson());
        }

        public async Task SendOfflineAsync()
        {
            await SendAsync(new
            {
                MsgType = 3,
                IdentityId = $"{Module}.{ClientName}"
            }.ToJson());
        }

        /// <summary>
        /// 发送状态
        /// </summary>
        /// <returns></returns>
        public async Task SendStatusAsync()
        {
            await SendAsync(new
            {
                MsgType = 5, // status
                StatusCode = (int)Status,
                Message = Status.ToString()
            }.ToJson());
        }
    }
}
