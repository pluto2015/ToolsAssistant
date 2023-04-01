using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WatsonWebsocket;

namespace ToolsAssistant.Services
{
    internal class WebSocketClient : IWebSocketClient
    {
        private WatsonWsClient _client = null;

        private EncordingType _encordingType = EncordingType.Utf8;

        public bool IsConnected => _client != null && _client.Connected;

        public event EventHandlers.DataRecievedEventHandler DataRecievedEvent;
        public event EventHandlers.ConnectEventHandler ConnectEvent;

        public Task<bool> ConnectWithTimeoutAsync(string ipPort, int timeOut)
        {
            if(_client == null||!_client.Connected)
            {
                _client = new WatsonWsClient(new Uri($"ws://{ipPort}"));
            }else
            {
                _client.Stop();
                _client = new WatsonWsClient(new Uri($"ws://{ipPort}"));
            }

            _client.ServerConnected += ServerConnected;
            _client.MessageReceived += MessageReceived;
            _client.ServerDisconnected += ServerDisconnected;

            return _client.StartWithTimeoutAsync(timeOut);
        }

        private void ServerDisconnected(object sender, EventArgs e)
        {
            ConnectEvent?.Invoke(null, false);
        }

        private void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            try
            {
                string data = "";
                switch(_encordingType)
                {
                    case EncordingType.Utf8:
                        data = Encoding.UTF8.GetString(e.Data);
                        break;
                    case EncordingType.ASCII:
                        data = Encoding.ASCII.GetString(e.Data);
                        break;
                    case EncordingType.Hex:
                        for(int i=0;i<e.Data.Count;i++)
                        {
                            data += Convert.ToString(e.Data[i],16)+" ";
                        }
                        break;
                }
                DataRecievedEvent?.Invoke(null,data);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ServerConnected(object sender, EventArgs e)
        {
            ConnectEvent?.Invoke(null, true);
        }

        public void Disconnect()
        {
            if(_client != null && _client.Connected)
            {
                _client.Stop();
                ConnectEvent?.Invoke(null, false);
            }
        }

        public void SetEncording(EncordingType encordingType)
        {
            _encordingType = encordingType;
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
                    dataBytes = lst.Select(x=>Convert.ToByte(x,16)).ToList().ToArray();
                    break;
                default: //EncordingType.Utf8
                    dataBytes = Encoding.UTF8.GetBytes(data);
                    break;
            }

            _client.SendAsync(dataBytes);
        }
    }
}
