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
            RefreshFirmwareBtn.IsEnabled = Telemetry.IsConnected;
            RefreshFirmwareBtn.ToolTip = (Telemetry.IsConnected) ? "Refresh" : "Need to be connected to refresh";

            Telemetry.Subscribe(KFlyCommandType.ConnectionStatusChanged, (ConnectionStatusChanged csc) =>
            {
                RefreshFirmwareBtn.Dispatcher.Invoke((Action)(() =>
                {
                    RefreshFirmwareBtn.IsEnabled = csc.Connected;
                    RefreshFirmwareBtn.ToolTip = (csc.Connected) ? "Refresh" : "Need to be connected to refresh";
                }));
            });
        }
    }
}
