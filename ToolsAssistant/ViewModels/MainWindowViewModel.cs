using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using ToolsAssistant.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Pluto.Wpf.Controls.Tab;

namespace ToolsAssistant.ViewModels
{
    public class MainWindowViewModel:ObservableObject
    {
        #region props
        /// <summary>
        /// Tab列表
        /// </summary>
        public ObservableCollection<Tab> Tabs { get; } = new ObservableCollection<Tab>();

        private Tab _SelectedTab;
        /// <summary>
        /// 当前选中的tab
        /// </summary>
        public Tab SelectedTab { set => SetProperty(ref _SelectedTab, value); get => _SelectedTab; }
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
                var str = "串口";
                var ids = Tabs.Where(x => x.Header.StartsWith(str))?.Select(x => int.Parse(x.Header.Replace(str, "")));
                var id = ids.Count() == 0 ? 1 : ids.Max() + 1;
                var tab = new Tab
                {
                    ContentPage = new SerialView(),
                    Header = "串口" + GetId("串口"),
                    TabWidth = 100
                };
                Tabs.Add(tab);

                //切换到页面
                SelectedTab = tab;
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
                var tab = new Tab
                {
                    Header = "TCP服务器" + GetId("TCP服务器"),
                    ContentPage = new TcpServerView(),
                    TabWidth = 150
                };
                Tabs.Add(tab);

                //切换页面
                SelectedTab = tab;
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
                var tab = new Tab
                {
                    Header = "TCP客户端" + GetId("TCP客户端"),
                    ContentPage = new TcpClientView(),
                    TabWidth = 150
                };
                Tabs.Add(tab);

                //切换页面
                SelectedTab = tab;
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
                var tab = new Tab
                {
                    Header = "WebSocket客户端" +GetId("WebSocket客户端"),
                    ContentPage = new WebSocketClientView(),
                    TabWidth = 160
                };
                Tabs.Add(tab);

                //切换页面
                SelectedTab = tab;
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
                var tab = new Tab
                {
                    Header = "WebSocket服务端" + GetId("WebSocket服务端"),
                    ContentPage = new WebSocketServerView(),
                    TabWidth = 160
                };
                Tabs.Add(tab);

                //切换页面
                SelectedTab = tab;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 通过名称获取id
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        int GetId(string name)
        {
            var ids = Tabs.Where(x => x.Header.StartsWith(name))?.Select(x => int.Parse(x.Header.Replace(name, "")));
            var id = ids.Count() == 0 ? 1 : ids.Max() + 1;

            return id;
        }

        #endregion
    }
}
