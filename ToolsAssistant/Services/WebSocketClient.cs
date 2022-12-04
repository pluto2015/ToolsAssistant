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

        public event EventHandlers.DataRecievedEventHandler DataRecievedEvent;
        public event EventHandlers.ConnectEventHandler ConnectEvent;

        public void Connect(string ipPort)
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

            _client.Start();
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
                        for(int i=0;i<e.Data.Length;i++)
                        {
                            data += Convert.ToString(e.Data[i],16)+" ";
                        }
                        break;
                }
                DataRecievedEvent?.Invoke(e.IpPort,data);
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
            if(_client.Connected)
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
                    dataBytes = Encoding.ASCII.GetBytes(data.Replace(" ",""));
                    break;
                case EncordingType.Hex:
                    var lst = data.Split(" ");
                    dataBytes = new byte[lst.Length];
                    for (int i = 0; i < lst.Length; i++)
                    {
                        dataBytes[i] = Convert.ToByte(lst[i], 16);
                    }
                    break;
                default: //EncordingType.Utf8
                    dataBytes = Encoding.UTF8.GetBytes(data);
                    break;
            }

            _client.SendAsync(dataBytes);
        }
    }
}
