using GodSharp.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsAssistant.Models;

namespace ToolsAssistant.Services
{
    public class TCPServerService : IServerService
    {
        public event EventHandlers.DataRecievedEventHandler DataRecieved;
        public event EventHandlers.ConnectEventHandler ClientConnected;
        public event EventHandler ServerStoped;

        private ITcpServer _server = null;

        private EncordingType _encordingType = EncordingType.Utf8;

        public ClientServerType ServerType => ClientServerType.TCP;

        public void SendData(string ipPort, string data)
        {
            byte[] dataBytes;
            switch (_encordingType)
            {
                case EncordingType.ASCII:
                    dataBytes = Encoding.ASCII.GetBytes(data.Replace(" ", ""));
                    break;
                case EncordingType.Hex:
                    var lst = data.Split(" ");
                    dataBytes = lst.Select(x => Convert.ToByte(x, 16)).ToArray();
                    break;
                default: //EncordingType.Utf8
                    dataBytes = Encoding.UTF8.GetBytes(data);
                    break;
            }

            var client = _server.Connections.FirstOrDefault(x => $"{x.Value.RemoteEndPoint.Address}:{x.Value.RemoteEndPoint.Port}" == ipPort).Value;
            if (client == null)
            {
                throw new Exception($"客户端({ipPort})不存在");
            }

            client.Send(dataBytes);
        }

        public void SetEncording(EncordingType encordingType)
        {
            _encordingType = encordingType;
        }

        public void Start(string url)
        {
            if (_server != null && _server.Running)
            {
                _server.Stop();
            }
            var lst = url.Split(":");
            _server = new TcpServer(host: lst[0], port: int.Parse(lst[1]))
            {
                OnConnected = (c) =>
                {
                    var ep = c.NetConnection.RemoteEndPoint;
                    ClientConnected?.Invoke($"{ep.Address}:{ep.Port}", true);
                },
                OnDisconnected = (c) =>
                {
                    var ep = c.NetConnection.RemoteEndPoint;
                    ClientConnected?.Invoke($"{ep.Address}:{ep.Port}", false);
                },
                OnStopped = (c) =>
                {
                    ServerStoped?.Invoke(this,EventArgs.Empty);
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
                            data = string.Join(" ", c.Buffers.Select(x => Convert.ToString(x, 16)));
                            break;
                    }
                    DataRecieved?.Invoke($"{c.RemoteEndPoint.Address}:{c.RemoteEndPoint.Port}", data);
                }
            };

            _server.Start();
        }

        public void Stop()
        {
            if (_server != null && _server.Running)
            {
                _server.Stop();
            }
        }
    }
}
