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
using KFly.Communication;
using KFly.Logging;
using KFly.Utils;
using System.IO.Ports;

namespace KFly.GUI
{
    /// <summary>
    /// Interaction logic for Connection.xaml
    /// </summary>
    public partial class Connection : UserControl
    {
        public Connection()
        {
            InitializeComponent();
        }

        private void ReloadPorts()
        {
            SerialPortCombo.Items.Clear();
            foreach (string str in SerialPort.GetPortNames())
            {
                SerialPortCombo.Items.Add(str);
            }
            var oldChoosen = Properties.Settings.Default.ComPort;
            if ((oldChoosen != null) && (oldChoosen.Length > 0))
            {
                if (!(SerialPortCombo.Items.Contains(oldChoosen)))
                {
                    oldChoosen += " <Not available>";
                    SerialPortCombo.Items.Add(oldChoosen);
                }
                SerialPortCombo.SelectedItem = oldChoosen;
            }
            else if (SerialPortCombo.Items.Count > 0)
            {
                SerialPortCombo.SelectedItem = SerialPortCombo.Items[0];
            }
        }
        
        /// <summary>
        /// Main window needs to add an hook to this function to work
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == USBHandler.WM_DEVICECHANGE)
            {
                if (wParam.ToInt32() == USBHandler.DBT_DEVNODES_CHANGED)
                {
                    SerialPortCombo.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        ReloadPorts();
                    }));
                }
            }
            return IntPtr.Zero;
        }


        private void SerialPortCombo_Initialized(object sender, EventArgs e)
        {
            ReloadPorts();
        }
        
        private void BaudrateCombo_Initialized(object sender, EventArgs e)
        {
            BaudrateCombo.Items.Clear();
            BaudrateCombo.ItemsSource = Enum.GetValues(typeof(Baudrate)).Cast<Baudrate>();
            BaudrateCombo.SelectedItem = Properties.Settings.Default.Baudrate;
        }

        
        private void UserControl_Initialized(object sender, EventArgs e)
        {
            Telemetry.Subscribe(KFlyCommandType.ConnectionStatusChanged, (ConnectionStatusChanged csc) =>
            {
                StatusLabel.Dispatcher.Invoke(new Action(() =>{
                    switch (csc.Status)
                    {
                        case ConnectionStatus.Connected:
                            StatusLabel.Content = "Connected";
                            StatusLabel.Foreground = (Brush)FindResource("AccentColorBrush");
                            StatusIcon.Content = FindResource("connected");
                            break;
                        case ConnectionStatus.Polling:
                            StatusLabel.Content = "Polling " + Telemetry.Port;
                            StatusLabel.Foreground = Brushes.Yellow;
                            StatusIcon.Content = FindResource("polling");
                            break;
                        default:
                            StatusLabel.Content = "Disconnected";
                            StatusLabel.Foreground = Brushes.Red;
                            StatusIcon.Content = FindResource("disconnected");
                            break;
                    }
                }));
            });
            Telemetry.Subscribe(KFlyCommandType.ConnectionStatistics, (ConnectionStatistics cs) =>
            {
                TotalInLabel.Dispatcher.Invoke(new Action(() =>
                {
                    TotalInLabel.Content = cs.BytesIn.ToString();
                    TotalOutLabel.Content = cs.BytesOut.ToString();
                }));
            });
        }

        
        private void AutoConnect_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.AutoConnectOnStartup = (AutoConnect.IsChecked == true);
            Properties.Settings.Default.Save();
        }


        private void ConnectionToggle_Checked(object sender, RoutedEventArgs e)
        {
            var port = SerialPortCombo.Text.Split(' ').First();          
            if (port.Length <= 0)
            {
                ConnectionToggle.IsChecked = false;
                ConnectErrorLabel.Content = "You must choose a serial port";
                return;
            }
            if (BaudrateCombo.SelectedItem == null)
            {
                ConnectionToggle.IsChecked = false;
                ConnectErrorLabel.Content = "You must choose a valid baudrate";
                return;
            }

            Telemetry.Port = port;
            Properties.Settings.Default.ComPort = port;
            Properties.Settings.Default.Baudrate = (Baudrate)BaudrateCombo.SelectedItem;
            Properties.Settings.Default.Save();

            LogManager.LogInfoLine("Connecting to serial port " + port + "...");
            Telemetry.Connect();
         
        }

        private void ConnectionToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            Telemetry.Disconnect();
        }


    }
}
