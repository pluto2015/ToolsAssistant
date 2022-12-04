using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WatsonWebsocket;

namespace ToolsAssistant.Services
{
    internal class WebSocketServer:IWebSocketServer
    {
        private WatsonWsServer _server =null;

        private EncordingType _encordingType = EncordingType.Utf8;

        public event EventHandlers.DataRecievedEventHandler DataRecievedEvent;
        public event EventHandlers.ConnectEventHandler ConnectEvent;

        public void Start(int port)
        {
            if(_server != null&&_server.IsListening)
            {
                _server.Stop();
            }
            _server = new WatsonWsServer("+", port);

            _server.ServerStopped += ServerStopped;
            _server.ClientConnected += ClientConnected;
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
                        for (int i = 0; i < e.Data.Length; i++)
                        {
                            data += Convert.ToString(e.Data[i], 16) + " ";
                        }
                        break;
                }
                DataRecievedEvent?.Invoke(e.IpPort, data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            ConnectEvent?.Invoke(e.IpPort,false);
        }

        private void ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            ConnectEvent?.Invoke(e.IpPort, true);
        }

        private void ServerStopped(object sender, EventArgs e)
        {

        }

        public void Stop()
        {
            _server.Stop();
        }

        public void SetEncording(EncordingType encordingType)
        {
            _encordingType = encordingType;
        }
    }
}
