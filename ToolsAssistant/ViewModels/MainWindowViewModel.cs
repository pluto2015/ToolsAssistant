using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using ToolsAssistant.Views;
using ToolsAssistant.Factories;

namespace ToolsAssistant.ViewModels
{
    public class MainWindowViewModel:ObservableObject
    {
        #region props
        private Page _SubPage;
        public Page SubPage { set=>SetProperty(ref _SubPage,value); get=>_SubPage; }
        #endregion
        #region commands
        public RelayCommand WebSocketServerCommand { set; get; }
        public RelayCommand WebSocketClientCommand { set; get; }
        public RelayCommand TcpClientCommand { set; get; }
        public RelayCommand TcpServerCommand { set; get; }
        public RelayCommand SerialCommand { set; get; }
        #endregion
        #region methods
        protected ILogger<MainWindowViewModel> _logger;
        public MainWindowViewModel()
        {
            _logger = App.Current.Services.GetService<ILogger<MainWindowViewModel>>();
            Init();
        }
        private void Init()
        {
            try
            {
                InitCommand();

                OnWebSocketClientCommand();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }
        private void InitCommand()
        {
            WebSocketServerCommand = new RelayCommand(OnWebSocketServerCommand);
            WebSocketClientCommand = new RelayCommand(OnWebSocketClientCommand);
            TcpClientCommand = new RelayCommand(OnTcpClientCommand);
            TcpServerCommand = new RelayCommand(OnTcpServerCommand);
            SerialCommand = new RelayCommand(OnSerialCommand);
    }

        private void OnSerialCommand()
        {
            try
            {
                var dlg = PageFactory<SerialView>.GetPage(nameof(SerialView));
                SubPage = dlg;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void OnTcpServerCommand()
        {
            try
            {
                var dlg = PageFactory<TcpServerView>.GetPage(nameof(TcpServerView));
                SubPage = dlg;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void OnTcpClientCommand()
        {
            try
            {
                var dlg = PageFactory<TcpClientView>.GetPage(nameof(TcpClientView));
                SubPage = dlg;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void OnWebSocketClientCommand()
        {
            try
            {
                var dlg = PageFactory<WebSocketClientView>.GetPage(nameof(WebSocketClientView));
                SubPage = dlg;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void OnWebSocketServerCommand()
        {
            try
            {
                var dlg = PageFactory<WebSocketServerView>.GetPage(nameof(WebSocketServerView));
                SubPage = dlg;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        #endregion
    }
}
