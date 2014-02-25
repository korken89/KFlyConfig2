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
using System.Windows.Media.Animation;
using System.Resources;
using System.IO.Ports;
using System.Windows.Interop;
using KFly.Communication;
using KflyConfig.GUI;
using MahApps.Metro.Controls;
using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
using KFly.Logging;

namespace KFly.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            SubscribeToCommunication();
            LogManager.AddLogDestination(new RichTextBoxLog(LogBox));
            LogManager.AddLogDestination(new LabelLog(LogRowLabel));
        }

        /// <summary>
        /// Hook WndProc for USB detection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(ConnectionControl.WndProc);
        }

        
      
       

        private void SubscribeToCommunication()
        {
            //Log window
            Telemetry.Subscribe(KFlyCommandType.ConnectionStatusChanged, (ConnectionStatusChanged cmd) =>
            {
                ConnectionStatusText.Dispatcher.BeginInvoke((Action)(()=>
                    {
                        ConnectionStatusText.Text = (cmd.IsConnected)? "Connected" : "Not connected";
                        if (cmd.IsConnected)
                        {
                            ConnectionStatusIcon.Content = FindResource("connected");
                        }
                        else
                        {
                            ConnectionStatusIcon.Content = FindResource("disconnected");
                        }
                    }));
            });

        }

        private void LogBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LogBox.ScrollToEnd();
        }

        private void LogBox_Initialized(object sender, EventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            ConnectionFlyout.IsOpen = !ConnectionFlyout.IsOpen;
        }

        private void ExpandLogPanelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (LogPanel.Height == 30)
            {
                ExpandLogPanelBtn.Content = FindResource("arrowDown");
                BeginStoryboard((Storyboard)FindResource("LogPanelExpand"));
                ExpandLogPanelBtn.ToolTip = "Hide log";
            }
            else
            {
                ExpandLogPanelBtn.Content = FindResource("arrowUp");
                BeginStoryboard((Storyboard)FindResource("LogPanelShrink"));
                ExpandLogPanelBtn.ToolTip = "Show log";
            }
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            Telemetry.Disconnect();
        }


    }
}
