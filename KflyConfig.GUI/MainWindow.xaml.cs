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
            InitCommunicationGUI();
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
            source.AddHook(WndProc);
        }

        IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == USBHandler.WM_DEVICECHANGE)
            {
                if (wParam.ToInt32() == USBHandler.DBT_DEVNODES_CHANGED)
                {
                    PortsCombo.Dispatcher.BeginInvoke((Action)(()=>
                    {
                        ReloadPorts();
                    }));
                }
            }
            return IntPtr.Zero;
        }

        private void InitCommunicationGUI()
        {
            ReloadPorts();
        }

        private void ReloadPorts()
        {
            foreach (string str in SerialPort.GetPortNames())
            {
                PortsCombo.Items.Add(str);
                if (str == Properties.Settings.Default.ComPort)
                    PortsCombo.SelectedItem = str;
            }
        }

        private void SubscribeToCommunication()
        {
            //Log window
            Telemetry.Subscribe(KFlyCommandType.ConnectionStatusChanged, (ConnectionStatusChanged cmd) =>
            {
                ConnectionStatusText.Dispatcher.BeginInvoke((Action)(()=>
                    {
                        ConnectionStatusText.Text = (cmd.Connected)? "Connected" : "Not connected";
                        if (cmd.Connected)
                        {
                            ConnectionStatusIcon.Fill = new VisualBrush((Canvas)Resources["appbar_disconnect"]);
                        }
                        else
                        {
                            ConnectionStatusIcon.Fill = new VisualBrush((Canvas)Resources["appbar_connect"]);
                            LogManager.LogInfoLine("<#FF0000>Serialport disconnected</#>");
                        }
                    }));
            });

            Telemetry.Subscribe(KFlyCommandType.All, (KFlyCommand cmd) =>
            {
              /*  infoBox.BeginInvoke((MethodInvoker)delegate
                {
                    infoBox.AppendText(cmd.ToString());
                });*/
            });

            //Firmware version info textbox
            Telemetry.Subscribe(KFlyCommandType.GetFirmwareVersion, (GetFirmwareVersion cmd) =>
            {
               /* firmwareVersion.BeginInvoke((Action)(() =>
                {
                    firmwareVersion.Text = cmd.Version;
                }));*/
            });

            //Bootloader version info textbox
            Telemetry.Subscribe(KFlyCommandType.GetBootloaderVersion, (GetBootLoaderVersion cmd) =>
            {
               /* bootloaderVersion.BeginInvoke((Action)(() =>
                {
                    bootloaderVersion.Text = cmd.Version;
                }));*/
            });
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (PortsCombo.Text.Length > 0)
            {
                Telemetry.Port = PortsCombo.Text;
                // Telemetry.BaudRate = BaudrateCombo.Text;
                Properties.Settings.Default.ComPort = PortsCombo.Text;
                Properties.Settings.Default.Baudrate = BaudrateCombo.Text;
                Properties.Settings.Default.Save();

                LogManager.LogInfoLine("Connecting to serial port " + PortsCombo.Text + "...");
                Telemetry.Connect();
                DisconnectBtn.IsEnabled = true;
                ConnectBtn.IsEnabled = false;
            }
            else
            {
                MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
                this.ShowMessageAsync("Can not connect", "You need to choose a serial port");
            }
        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            Telemetry.Disconnect();
            DisconnectBtn.IsEnabled = false;
            ConnectBtn.IsEnabled = true;
        }

        private void BaudrateCombo_Initialized(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Baudrate.Length > 0)
                BaudrateCombo.Text = Properties.Settings.Default.Baudrate;
            else
                BaudrateCombo.Text = "1000000";
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
