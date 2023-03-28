using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected readonly ILogger<MainWindow> _logger;
        public MainWindow()
        {
            InitializeComponent();
            _logger = App.Current.Services.GetService<ILogger<MainWindow>>();
            DataContext = new MainWindowViewModel();
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            try
            {
                var frame = sender as Frame;
                if(frame.CanGoBack)
                {
                    frame.RemoveBackEntry();
                    //主动回收，加快释放资源
                    GC.Collect();
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        private void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            try
            {
                var frame = sender as Frame;
                var page = frame.DataContext as Page;
                if(page!= null)
                {
                    var vm = page.DataContext as IDisposable;
                    vm.Dispose();
                    page.DataContext = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
    }
}
