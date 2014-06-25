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
            this.DataContext = new KFLyConfiguration(true);
            InitializeComponent();
        }

        public static MainWindow Find(Visual child)
        {
            var p = child.GetParentObject();
            while (p != null && !(p is MainWindow))
            {
                p = p.GetParentObject();
            }
            return p as MainWindow;
        }
        

        private void Window_Initialized(object sender, EventArgs e)
        {
            SubscribeToCommunication();
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
                        SaveToFlashPanel.Visibility = (!Telemetry.Saved && Telemetry.IsConnected) ? 
                            Visibility.Visible : Visibility.Collapsed;
                    }));
            });

            Telemetry.Subscribe(KFlyCommandType.SaveStatusChanged, (SaveStatusChanged cmd) =>
            {
                SaveToFlashPanel.Dispatcher.BeginInvoke((Action)(() =>
                {
                    SaveToFlashPanel.Visibility = (!cmd.Saved && Telemetry.IsConnected) ? Visibility.Visible : Visibility.Collapsed; 
                }));
            });
        }

        

        private void OpenConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            ConnectionFlyout.IsOpen = !ConnectionFlyout.IsOpen;
        }


        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            Telemetry.Disconnect();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem ti = e.AddedItems[0] as TabItem;
            foreach (object item in TabControl.Items)
            {
                SetSelected(item as TabItem, (item == e.AddedItems[0]));
            }
        }

        private void SetSelected(TabItem tab, Boolean value)
        {
            if (tab != null)
            {
                KFlyTab kfly = XAMLHelper.GetChild<KFlyTab>(tab);
                if (kfly != null)
                {
                    kfly.IsSelected = value;
                }
            }
        }

        private void MyWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && (e.Key == Key.S))
            {
                SensorCalibrationData data = SCTab.DataContext as SensorCalibrationData;
                if (data != null)
                {
                    Properties.Settings.Default.Calibration = data;
                }
            }
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && (e.Key == Key.L))
            {
                SensorCalibrationData data = Properties.Settings.Default.Calibration;
                if (data != null)
                {
                    SCTab.DataContext = data;
                }
            }
        }

        private void SaveToFlashBtn_Click(object sender, RoutedEventArgs e)
        {
            Telemetry.SendAsyncWithAck(new SaveToFlash(), 1000, (SendResult result) =>
            {
                if (result == SendResult.OK)
                {
                    LogManager.LogInfoLine("All settings saved to flash");
                }
                else
                {
                    LogManager.LogErrorLine("Failed saving to flash");
                }
            });
        }

    }
}
