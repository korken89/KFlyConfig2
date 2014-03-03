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
using KFly;

namespace KFly.GUI
{
    /// <summary>
    /// Interaction logic for HomeTab.xaml
    /// </summary>
    public partial class SensorCalibrationTab : UserControl
    {
        public SensorCalibrationTab()
        {   
            InitializeComponent();
            this.DataContext = new SensorCalibration();
        }

        private void UpdateSensorCalibrationData()
        {
        }
      
        private void UserControl_Initialized(object sender, EventArgs e)
        {
           

            Telemetry.Subscribe(KFlyCommandType.ConnectionStatusChanged, (ConnectionStatusChanged csc) =>
            {
                if (csc.IsConnected)
                {
                    UpdateSensorCalibrationData();
                }
            });
            Telemetry.Subscribe(KFlyCommandType.GetSensorCalibration, (GetSensorCalibration msg) =>
            {
                AccelerometerGrid.Dispatcher.Invoke(new Action(() =>
                {
                    this.DataContext = msg.Data;
                }));
           });
          
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CalibrationModal.ShowModalContent();
        }

        private void UploadBtn_Click(object sender, RoutedEventArgs e)
        {
            Upload();
        }

        private void DownloadBtn_Click(object sender, RoutedEventArgs e)
        {
            Download();
        }

        private void Upload()
        {
            if (!UploadBtn.IsRotating)
            {
                UploadBtn.IsRotating = true;
                var cmd = new SetSensorCalibration(this.DataContext as SensorCalibration);
                Telemetry.SendAsyncWithAck(cmd, 1000, (SendResult sr) =>
                {
                    if (sr == SendResult.OK)
                    {
                        LogManager.LogInfoLine("Sensor calibration data uploaded!");
                    }
                    else
                    {
                        LogManager.LogErrorLine("Failed uploading sensor calibration data!");
                    }
                    UploadBtn.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            UploadBtn.IsRotating = false;
                        }));
                });
            }
        }

        private void Download()
        {
            if (!DownloadBtn.IsRotating)
            {
                DownloadBtn.IsRotating = true;
                Telemetry.SendAsyncWithAck(new GetSensorCalibration(), 1000, (SendResult sr) =>
                    {
                        if (sr == SendResult.OK)
                        {
                            LogManager.LogInfoLine("Sensor calibration data recevied!");
                        }
                        else
                        {
                            LogManager.LogErrorLine("Failed receiving sensor calibration data!");
                        }
                        DownloadBtn.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            DownloadBtn.IsRotating = false;
                        }));
                    });
            }
        }

        private void KFlyTab_TabStateChanged(object sender, TabStateChangedEventArgs e)
        {
            if (!e.IsUpToDate && e.IsConnected && e.IsSelected)
            {
                Download();
            }
        }

        private Window _testCalibrationWindow;
        private void TestCalibrationBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_testCalibrationWindow == null)
            {
                _testCalibrationWindow = new TestCalibrationWindow();
                _testCalibrationWindow.Closed += (o, args) => _testCalibrationWindow = null;
            }
            if (_testCalibrationWindow.IsVisible)
                _testCalibrationWindow.Hide();
            else
                _testCalibrationWindow.Show();
        }


    }
}
