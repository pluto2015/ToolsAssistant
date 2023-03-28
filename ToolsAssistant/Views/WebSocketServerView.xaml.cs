using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToolsAssistant.ViewModels;

namespace ToolsAssistant.Views
{
    /// <summary>
    /// WebSocketServerViewModel.xaml 的交互逻辑
    /// </summary>
    public partial class WebSocketServerView : Page
    {
        public WebSocketServerView()
        {
            InitializeComponent();
            DataContext = new WebSocketServerViewModel();
        }
    }
}
