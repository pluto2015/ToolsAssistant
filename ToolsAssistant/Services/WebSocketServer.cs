using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToolsAssistant.Models;
using WatsonWebsocket;

namespace ToolsAssistant.Services
{
    internal class WebSocketServer:IServerService
    {
        private WatsonWsServer _server =null;

        private EncordingType _encordingType = EncordingType.Utf8;

        public ClientServerType ServerType => ClientServerType.Websocket;

        public event EventHandlers.DataRecievedEventHandler DataRecieved;
        public event EventHandlers.ConnectEventHandler ClientConnected;
        public event EventHandler ServerStoped;

        public void Start(string url)
        {
            if (_server != null && _server.IsListening)
            {
                _server.Stop();
            }
            _server = new WatsonWsServer(new Uri($"http://{url}"));

            _server.ServerStopped += ServerStopped;
            _server.ClientConnected += OnClientConnected;
            _server.ClientDisconnected += ClientDisconnected;
            _server.MessageReceived += MessageReceived;

            _server.Start();
        }

        private void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            try
            {
                string data = "";
                switch (_encordingType)
                {
                    case EncordingType.Utf8:
                        data = Encoding.UTF8.GetString(e.Data);
                        break;
                    case EncordingType.ASCII:
                        data = Encoding.ASCII.GetString(e.Data);
                        break;
                    case EncordingType.Hex:
                        data = string.Join(" ", e.Data.Select(x => Convert.ToString(x, 16)));
                        break;
                }
                DataRecieved?.Invoke(e.Client.IpPort, data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ClientDisconnected(object sender, DisconnectionEventArgs e)
        {
            ClientConnected?.Invoke(e.Client.IpPort,false);
        }

        private void OnClientConnected(object sender, ConnectionEventArgs e)
        {
            ClientConnected?.Invoke(e.Client.IpPort, true);
        }

        private void ServerStopped(object sender, EventArgs e)
        {
            ServerStoped?.Invoke(this, e);
        }

        public void Stop()
        {
            if(_server != null && _server.IsListening)
            {
                _server.Stop();
            }
        }

        public void SetEncording(EncordingType encordingType)
        {
            _encordingType = encordingType;
        }

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
                    dataBytes = lst.Select(x=>Convert.ToByte(x,16)).ToArray();
                    break;
                default: //EncordingType.Utf8
                    dataBytes = Encoding.UTF8.GetBytes(data);
                    break;
            }

            var client = _server.ListClients().FirstOrDefault(x => x.IpPort == ipPort);
            if (client == null)
            {
                throw new Exception($"客户端({ipPort})不存在");
            }

            _server.SendAsync(client.Guid, dataBytes);
        }
    }
}
