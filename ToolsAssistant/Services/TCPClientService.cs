using GodSharp.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsAssistant.Models;

namespace ToolsAssistant.Services
{
    public class TCPClientService : IClientService
    {
        public bool IsConnected => _client != null && _client.Connected;

        public ClientServerType ClientType => ClientServerType.TCP;

        public event EventHandlers.DataRecievedEventHandler DataRecievedEvent;
        public event EventHandlers.ConnectEventHandler ConnectEvent;
        /// <summary>
        /// tcp客户端
        /// </summary>
        private ITcpClient _client;
        /// <summary>
        /// 编码格式
        /// </summary>
        private EncordingType _encordingType = EncordingType.Utf8;

        public Task<bool> ConnectWithTimeoutAsync(string ipPort, int timeOut = 5)
        {
            return Task.Run(() =>
            {
                try
                {
                    if (_client != null && _client.Connected)
                    {
                        _client.Stop();
                    }

                    var lst = ipPort.Split(':');
                    _client = new TcpClient(lst[0], int.Parse(lst[1]), connectTimeout: timeOut)
                    {
                        OnConnected = (c) =>
                        {
                            ConnectEvent?.Invoke(null, true);
                        },
                        OnDisconnected = (c) =>
                        {
                            ConnectEvent?.Invoke(null, false);
                        },
                        OnReceived = (c) =>
                        {
                            string data = "";
                            switch (_encordingType)
                            {
                                case EncordingType.Utf8:
                                    data = Encoding.UTF8.GetString(c.Buffers);
                                    break;
                                case EncordingType.ASCII:
                                    data = Encoding.ASCII.GetString(c.Buffers);
                                    break;
                                case EncordingType.Hex:
                                    for (int i = 0; i < c.Buffers.Length; i++)
                                    {
                                        data += Convert.ToString(c.Buffers[i], 16) + " ";
                                    }
                                    break;
                            }
                            DataRecievedEvent?.Invoke(null, data);
                        }
                    };

                    _client.Start();
                    if (!_client.Connected)
                    {
                        return false;
                    }
                }catch(Exception ex)
                {
                    return false;
                }
                return true;
            });
        }

        public void Disconnect()
        {
            if (_client != null && _client.Connected)
            {
                _client.Stop();
                ConnectEvent?.Invoke(null, false);
            }
        }

        public void SendData(string data)
        {
            byte[] dataBytes;
            switch (_encordingType)
            {
                case EncordingType.ASCII:
                    dataBytes = Encoding.ASCII.GetBytes(data);
                    break;
                case EncordingType.Hex:
                    var lst = data.Split(" ");
                    dataBytes = lst.Select(x => Convert.ToByte(x, 16)).ToList().ToArray();
                    break;
                default: //EncordingType.Utf8
                    dataBytes = Encoding.UTF8.GetBytes(data);
                    break;
            }

            _client.Connection.Send(dataBytes);
        }

        public void SetEncording(EncordingType encordingType)
        {
            _encordingType = encordingType;
        }
    }
}
