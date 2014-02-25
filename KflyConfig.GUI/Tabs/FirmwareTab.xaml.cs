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

namespace KFly.GUI
{
    /// <summary>
    /// Interaction logic for HomeTab.xaml
    /// </summary>
    public partial class FirmwareTab : UserControl
    {
        public FirmwareTab()
        {
            InitializeComponent();
        }

        private void RefreshFirmwareBtn_Initialized(object sender, EventArgs e)
        {
            RefreshFirmwareBtn.IsEnabled = true;// Telemetry.IsConnected;
            RefreshFirmwareBtn.ToolTip = "Need to be connected to refresh";

            Telemetry.Subscribe(KFlyCommandType.ConnectionStatusChanged, (ConnectionStatusChanged csc) =>
            {
                RefreshFirmwareBtn.Dispatcher.Invoke((Action)(() =>
                {
                    RefreshFirmwareBtn.IsEnabled = csc.IsConnected;
                    RefreshFirmwareBtn.ToolTip = (csc.IsConnected) ? "Refresh" : "Need to be connected to refresh";
                }));
            });
        }

        private void RefreshFirmwareBtn_Click(object sender, RoutedEventArgs e)
        {
            //this.connectionModal.ShowModalContent();
            //this.connectionModal
        }

   

        private void UpdateConnectionControls()
        {
            var connected = Telemetry.IsConnected;
            RefreshFirmwareBtn.IsEnabled = connected;
            RefreshFirmwareBtn.ToolTip = (connected) ? "Refresh" : "Need to be connected to refresh";
            if (connected)
                connectionModal.HideModalContent();
            else
                connectionModal.ShowModalContent();
        }

        private void UpdateFirmwareInfo()
        {
            LogManager.LogInfoLine("Requesting FirmwareInfo");
            Telemetry.SendAsync(new GetBootLoaderVersion());
            Telemetry.SendAsync(new GetFirmwareVersion());
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            UpdateConnectionControls();
            Telemetry.Subscribe(KFlyCommandType.ConnectionStatusChanged, (ConnectionStatusChanged csc) =>
            {
                RefreshFirmwareBtn.Dispatcher.Invoke(new Action(()=> UpdateConnectionControls()));
                if (csc.IsConnected)
                {
                    UpdateFirmwareInfo();
                }
            });
            Telemetry.Subscribe(KFlyCommandType.GetBootloaderVersion, (GetBootLoaderVersion msg) =>
            {
                BootloaderVersionLabel.Dispatcher.Invoke(new Action(() => {
                    BootloaderVersionLabel.Content = msg.Version;
                }));
                LogManager.LogInfoLine("Bootloader version received: " + msg.Version);  
            });
            Telemetry.Subscribe(KFlyCommandType.GetFirmwareVersion, (GetFirmwareVersion msg) =>
            {
                FirmwareVersionLabel.Dispatcher.Invoke(new Action(() =>
                {
                    FirmwareVersionLabel.Content = msg.Version;
                }));
                LogManager.LogInfoLine("Firmware version received: " + msg.Version);
            });
        }


    }
}
