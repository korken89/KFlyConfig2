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
            PortsCombo.Items.Clear();
            foreach (string str in SerialPort.GetPortNames())
            {
                PortsCombo.Items.Add(str);
            }
            var oldChoosen = Properties.Settings.Default.ComPort;
            if ((oldChoosen != null) && (oldChoosen.Length > 0))
            {
                if (!(PortsCombo.Items.Contains(oldChoosen)))
                {
                    oldChoosen += " <Not available>";
                    PortsCombo.Items.Add(oldChoosen);
                }
                PortsCombo.SelectedItem = oldChoosen;
            }
            else if (PortsCombo.Items.Count > 0)
            {
                PortsCombo.SelectedItem = PortsCombo.Items[0];
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
                    PortsCombo.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        ReloadPorts();
                    }));
                }
            }
            return IntPtr.Zero;
        }


        private void BaudrateCombo_Initialized(object sender, EventArgs e)
        {
            BaudrateCombo.Items.Clear();
            foreach (Baudrate baudrate in Enum.GetValues(typeof(Baudrate)))
            {
                BaudrateCombo.Items.Add(DisplayValueEnum.GetDescriptionValue(baudrate));
            }
            if (Properties.Settings.Default.Baudrate.Length > 0)
                BaudrateCombo.Text = Properties.Settings.Default.Baudrate;
            else
                BaudrateCombo.Text = "1000000";
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (PortsCombo.Text.Length > 0)
            {
                var port = PortsCombo.Text.Split(' ').First();
                Telemetry.Port = port; 
                // Telemetry.BaudRate = BaudrateCombo.Text;
                Properties.Settings.Default.ComPort = port;
                Properties.Settings.Default.Baudrate = BaudrateCombo.Text;
                Properties.Settings.Default.Save();

                LogManager.LogInfoLine("Connecting to serial port " + port + "...");
                Telemetry.Connect();
               // DisconnectBtn.IsEnabled = true;
               // ConnectBtn.IsEnabled = false;
            }
            else
            {
              //  MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
              //  this.ShowMessageAsync("Can not connect", "You need to choose a serial port");
            }

        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            Telemetry.Disconnect();
           // DisconnectBtn.IsEnabled = false;
           // ConnectBtn.IsEnabled = true;
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            ReloadPorts();
        }

        
        private void AutoConnect_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.AutoConnectOnStartup = (AutoConnect.IsChecked == true);
            Properties.Settings.Default.Save();
        }

        private void MagicToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (PortsCombo.Text.Length > 0)
            {
                var port = PortsCombo.Text.Split(' ').First();
                Telemetry.Port = port;
                // Telemetry.BaudRate = BaudrateCombo.Text;
                Properties.Settings.Default.ComPort = port;
                Properties.Settings.Default.Baudrate = BaudrateCombo.Text;
                Properties.Settings.Default.Save();

                LogManager.LogInfoLine("Connecting to serial port " + port + "...");
                Telemetry.Connect();
                // DisconnectBtn.IsEnabled = true;
                // ConnectBtn.IsEnabled = false;
            }
            else
            {
                //  MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
                //  this.ShowMessageAsync("Can not connect", "You need to choose a serial port");
            }
        }

        private void MagicToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Telemetry.Disconnect();
         
        }

    }
}
