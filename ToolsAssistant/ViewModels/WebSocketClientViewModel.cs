using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Windows;
using ToolsAssistant.Services;

namespace ToolsAssistant.ViewModels
{
    public class WebSocketClientViewModel: ObservableObject,IDisposable
    {
        #region props
        private string _ConnectString = "连接";
        public string ConnectString { set => SetProperty(ref _ConnectString, value); get => _ConnectString; }

        private string _SendStr;
        public string SendStr { set => SetProperty(ref _SendStr, value); get => _SendStr; }

        private string _RecieveStr;
        public string RecieveStr { set => SetProperty(ref _RecieveStr, value); get => _RecieveStr; }

        private bool _IsUTF8 = true;
        public bool IsUTF8 {
            set
            {
                SetProperty(ref _IsUTF8, value);
                InitEncoder();
            }
            get => _IsUTF8; }

        private bool _IsASCII = false;
        public bool IsASCII {
            set
            {
                SetProperty(ref _IsASCII, value);
                InitEncoder();
            }
            get => _IsASCII; }

        private bool _IsHex = false;
        public bool IsHex {
            set
            {
                SetProperty(ref _IsHex, value);
                InitEncoder();
            }
            get => _IsHex; }

        private string _Url = "127.0.0.1:8888";
        public string Url { set => SetProperty(ref _Url, value); get => _Url; }
        #endregion
        #region commands
        public RelayCommand SendCommand { set; get; }
        public RelayCommand ClearSendCommand { set; get; }
        public RelayCommand ClearRecieveCommand { set; get; }
        public RelayCommand ConnectCommand { set; get; }
        #endregion
        #region methods
        protected readonly ILogger<WebSocketClientViewModel> _logger;
        protected readonly IWebSocketClient _client;
        public WebSocketClientViewModel()
        {
            _logger = App.Current.Services.GetService<ILogger<WebSocketClientViewModel>>();
            _client = App.Current.Services.GetService<IWebSocketClient>();
            Init();
        }

        private void Init()
        {
            SendCommand = new RelayCommand(OnSendCommand);
            ClearSendCommand = new RelayCommand(OnClearSendCommand);
            ClearRecieveCommand = new RelayCommand(OnClearRecieveCommand);
            ConnectCommand = new RelayCommand(OnConnectCommand);

            _client.ConnectEvent += ConnectEvent;
            _client.DataRecievedEvent += _client_DataRecievedEvent;

        }

        private void _client_DataRecievedEvent(string ipPort, string data)
        {
            RecieveStr = $"{RecieveStr}{DateTime.Now} ipPort => {data}\n";
        }

        private void InitEncoder()
        {
            if(IsUTF8)
            {
                _client.SetEncording(EncordingType.Utf8);
            }
            if (IsASCII)
            {
                _client.SetEncording(EncordingType.ASCII);
            }
            if (IsHex)
            {
                _client.SetEncording(EncordingType.Hex);
            }
        }

        private void ConnectEvent(string ipPort, bool isConnect)
        {
            if(isConnect)
            {
                ConnectString = "断开";
                InitEncoder();
            }
            else
            {
                ConnectString = "连接";
            }
        }

        private void OnConnectCommand()
        {
            try
            {
                if(ConnectString == "连接")
                {
                    _client.Connect(Url);
                }
                else
                {
                    _client.Disconnect();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }

        private void OnClearRecieveCommand()
        {
            try
            {
                RecieveStr = "";
            }catch(Exception ex)
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
                if(string.IsNullOrEmpty(SendStr))
                {
                    throw new Exception("发送内容不允许为空");
                }

                _client.SendData(SendStr);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }

        public void Dispose()
        {
            try
            {
                _client?.Disconnect();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
    }
}
