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
    public partial class AttitudeControllerTab : UserControl, IBound<AttitudeControllerTabData>
    {
        private AttitudeControllerTabData _data;

        public AttitudeControllerTab()
        {
            InitializeComponent();
            _data = new AttitudeControllerTabData();
            this.DataContext = _data;
        }

       
       
        private void UserControl_Initialized(object sender, EventArgs e)
        {
            Telemetry.Subscribe(KFlyCommandType.GetAttitudeControllerData, (GetControllerData gcd) =>
            {
                _data.AttitudeCData = gcd.Data;
            });
            Telemetry.Subscribe(KFlyCommandType.GetRateControllerData, (GetControllerData gcd) =>
            {
                _data.RateCData = gcd.Data;
            });

        }

        private void StopDownloadButtonRotation()
        {
            DownloadBtn.Dispatcher.BeginInvoke(new Action(() =>
            {
                DownloadBtn.IsRotating = false;
            }));
        }

        private void DownloadBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!DownloadBtn.IsRotating) //Use button to know if we already refreshing
            {
                DownloadBtn.IsRotating = true;
                Telemetry.SendAsyncWithAck(new GetControllerData(KFlyCommandType.GetAttitudeControllerData),
                    1000, (SendResult result) =>
                    {
                        if (result == SendResult.OK)
                        {
                            Telemetry.SendAsyncWithAck(new GetControllerData(KFlyCommandType.GetRateControllerData),
                                1000, (SendResult result2) =>
                                {
                                    if (result2 == SendResult.OK)
                                    {
                                        StopDownloadButtonRotation();
                                    }
                                });
                        }
                    });
            }
        }


    }
}
