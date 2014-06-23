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
    public partial class InputsTab : UserControl
    {
        public InputsTab()
        {
            InitializeComponent();
            this.DataContext = new RCCalibrationData();
      
        }

        private Boolean _isUpToDate = false;
        private void KFlyTab_TabStateChanged(object sender, TabStateChangedEventArgs e)
        {
            if (!e.IsConnected)
                _isUpToDate = false;

            if (!_isUpToDate && e.IsConnected && e.IsSelected)
            {
                Download();
            }
        }

        private void Upload()
        {
            if (!UploadBtn.IsRotating)
            {
                UploadBtn.IsRotating = true;
                var cmd = new SetRCCalibration(this.DataContext as RCCalibrationData);
                Telemetry.SendAsyncWithAck(cmd, 1000, (SendResult sr) =>
                {
                    if (sr == SendResult.OK)
                    {
                        LogManager.LogInfoLine("RC input calibration data uploaded!");
                    }
                    else
                    {
                        LogManager.LogErrorLine("Failed uploading rc input calibration data!");
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
                Telemetry.SendAsyncWithAck(new GetRCCalibration(), 1000, (SendResult sr) =>
                {
                    if (sr == SendResult.OK)
                    {
                        _isUpToDate = true;
                        LogManager.LogInfoLine("RC input calibration data recevied!");
                    }
                    else
                    {
                        LogManager.LogErrorLine("Failed receiving rc input calibration data!");
                    }
                    DownloadBtn.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        DownloadBtn.IsRotating = false;
                        if (sr == SendResult.OK)
                        {
                            InputBox.IsInSyncWithController = true;
                        }
                    }));
                });
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Telemetry.Subscribe(KFlyCommandType.GetRCCalibration, (GetRCCalibration msg) =>
            {
                CalibrationGrid.Dispatcher.Invoke(new Action(() =>
                {
                    this.DataContext = msg.Data;
                    InputBox.IsInSyncWithController = true;
                }));
            });
        }

        private void UploadBtn_Click(object sender, RoutedEventArgs e)
        {
            Upload();
        }

        private void DownloadBtn_Click(object sender, RoutedEventArgs e)
        {
            Download();
        }

        private void KFlyGroupBox_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (sender is KFlyGroupBox)
            {
                (sender as KFlyGroupBox).IsInSyncWithController = false;
            }
        }
    }
}
