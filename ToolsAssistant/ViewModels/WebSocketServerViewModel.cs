using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToolsAssistant.Helpers;
using ToolsAssistant.Services;

namespace ToolsAssistant.ViewModels
{
    public class WebSocketServerViewModel: ObservableObject,IDisposable
    {
        #region props
        private string _ConnectString = "连接";
        public string ConnectString { set => SetProperty(ref _ConnectString, value); get => _ConnectString; }

        private string _SendStr;
        public string SendStr { set
            {
                if (IsHex)
                {
                    var lst = value.Replace(" ", "").Split(2);
                    value = string.Join(" ", lst);
                }
                SetProperty(ref _SendStr, value);
            }
            get => _SendStr; }

        private string _RecieveStr;
        public string RecieveStr { set => SetProperty(ref _RecieveStr, value); get => _RecieveStr; }

        private bool _IsUTF8 = true;
        public bool IsUTF8
        {
            set
            {
                SetProperty(ref _IsUTF8, value);
                InitEncoder();
            }
            get => _IsUTF8;
        }

        private bool _IsASCII = false;
        public bool IsASCII
        {
            set
            {
                SetProperty(ref _IsASCII, value);
                InitEncoder();
            }
            get => _IsASCII;
        }

        private bool _IsHex = false;
        public bool IsHex
        {
            set
            {
                SetProperty(ref _IsHex, value);
                InitEncoder();
            }
            get => _IsHex;
        }

        private string _Url = "127.0.0.1:8888";
        public string Url { set => SetProperty(ref _Url, value); get => _Url; }

        /// <summary>
        /// 已连接的客户端
        /// </summary>
        public ObservableCollection<string> ConnectedClients { get; } = new ObservableCollection<string>();

        private string _SelectClient = null;
        /// <summary>
        /// 选择的客户端
        /// </summary>
        public string SelectClient { set=>SetProperty(ref _SelectClient,value); get=>_SelectClient; }
        #endregion
        #region commands
        public RelayCommand SendCommand { set; get; }
        public RelayCommand ClearSendCommand { set; get; }
        public RelayCommand ClearRecieveCommand { set; get; }
        public RelayCommand ConnectCommand { set; get; }
        #endregion
        #region methods
        protected readonly ILogger<WebSocketServerViewModel> _logger;
        protected readonly IWebSocketServer _server;
        public WebSocketServerViewModel()
        {
            _logger = App.Current.Services.GetService<ILogger<WebSocketServerViewModel>>();
            _server = App.Current.Services.GetService<IWebSocketServer>();
            Init();
        }

        private void Init()
        {
            SendCommand = new RelayCommand(OnSendCommand);
            ClearSendCommand = new RelayCommand(OnClearSendCommand);
            ClearRecieveCommand = new RelayCommand(OnClearRecieveCommand);
            ConnectCommand = new RelayCommand(OnConnectCommand);

            _server.ClientConnected += ConnectEvent;
            _server.DataRecieved += _server_DataRecieved;
        }

        private void _server_DataRecieved(string ipPort, string data)
        {
            RecieveStr = $"{RecieveStr}{DateTime.Now} ipPort => {data}\n";
        }

        private void OnConnectCommand()
        {
            try
            {
                if (ConnectString == "连接")
                {
                    _server.Start(Url);
                    InitEncoder();
                    ConnectString = "断开";
                }
                else
                {
                    _server.Stop();
                    ConnectString = "连接";
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void ConnectEvent(string ipPort, bool isConnect)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (isConnect)
                {
                    if (!ConnectedClients.Contains(ipPort))
                    {
                        ConnectedClients.Add(ipPort);
                    }
                }
                else
                {
                    if (ConnectedClients.Contains(ipPort))
                    {
                        ConnectedClients.Remove(ipPort);
                    }

                    if (SelectClient == ipPort)
                    {
                        SelectClient = null;
                    }
                }
            }));
        }

        private void InitEncoder()
        {
            if (IsUTF8)
            {
                _server.SetEncording(EncordingType.Utf8);
            }
            if (IsASCII)
            {
                _server.SetEncording(EncordingType.ASCII);
            }
            if (IsHex)
            {
                _server.SetEncording(EncordingType.Hex);
            }
        }

        private void OnClearRecieveCommand()
        {
            try
            {
                RecieveStr = "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }

        private void OnClearSendCommand()
        {
            try
            {
                SendStr = "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }

        private void OnSendCommand()
        {
            try
            {
                if (string.IsNullOrEmpty(SendStr))
                {
                    throw new Exception("发送内容不允许为空");
                }
                if(string.IsNullOrEmpty(SelectClient))
                {
                    throw new Exception("请选择客户端后再试");
                }

                _server.SendData(SelectClient, SendStr);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }

        public void Dispose()
        {
            _server.Stop();
        }

        #endregion
    }
}
