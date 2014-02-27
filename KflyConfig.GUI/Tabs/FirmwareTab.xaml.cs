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

       
        private void UpdateFirmwareInfo()
        {
            LogManager.LogInfoLine("Requesting FirmwareInfo");
            Telemetry.SendAsync(new GetBootLoaderVersion());
            Telemetry.SendAsync(new GetFirmwareVersion());
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            Telemetry.Subscribe(KFlyCommandType.ConnectionStatusChanged, (ConnectionStatusChanged csc) =>
            {
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
