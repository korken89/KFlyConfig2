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
using MahApps.Metro.Controls.Dialogs;

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
            LogManager.LogInfoLine("Requesting device information");
            Telemetry.SendAsync(new GetDeviceInfo());
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
        }

        private async void ChangeUserIdBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = MainWindow.Find(this);
            if (mw != null)
            {
                var re = await mw.ShowInputAsync("Set user device id", "maximum 100 characters");
                if (re != null)
                {
                    Telemetry.SendAsyncWithAck(new SetDeviceString(re), 1000, (SendResult res) =>
                        {
                            if (res == SendResult.OK)
                            {
                                LogManager.LogInfoLine("User device id set to: " + res);
                            }
                            else
                            {
                                LogManager.LogErrorLine("Failed uploading new user device id");
                            }
                        });
                }
            }
        }


    }
}
