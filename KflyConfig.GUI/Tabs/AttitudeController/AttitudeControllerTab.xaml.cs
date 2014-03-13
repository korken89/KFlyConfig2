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
using MahApps.Metro.Controls;

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
            Telemetry.Subscribe(KFlyCommandType.GetAttitudeControllerData, (GetAttitudeControllerData gcd) =>
            {
                _data.AttitudeCData = gcd.Data;
                _data.LimitCollection.AttitudeRateLimit = gcd.RateLimit;
                _data.LimitCollection.AngleLimit = gcd.AngleLimit;
                AttitudeLockBtn.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        AttitudeLockBtn.IsChecked = gcd.Data.LockPitchRoll;
                        AttitudeLockBtn.ToolTip = (gcd.Data.LockPitchRoll) ? "Unlink Pitch/Roll" : "Link Pitch/Roll";
                        AttitudeGB.IsInSyncWithController = true;
                    }));
                _data.NotifyPropertyChanged("LimitCollection"); //Need to trigger manually since we don't change the actual collection
            });
            Telemetry.Subscribe(KFlyCommandType.GetRateControllerData, (GetRateControllerData gcd) =>
            {
                _data.RateCData = gcd.Data;
                _data.LimitCollection.RateLimit = gcd.RateLimit;
                RateLockBtn.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        RateLockBtn.IsChecked = gcd.Data.LockPitchRoll;
                        RateLockBtn.ToolTip = (gcd.Data.LockPitchRoll) ? "Unlink Pitch/Roll" : "Link Pitch/Roll";
                        RateGB.IsInSyncWithController = true;
                        ConstraintsGB.IsInSyncWithController = true;
                    }));
                _data.NotifyPropertyChanged("LimitCollection"); //Need to trigger manually since we don't change the actual collection
            });

        }


        private void Reload()
        {
            if (!DownloadBtn.IsRotating) //Use button to know if we already refreshing
            {
                DownloadBtn.IsRotating = true;
                Telemetry.SendAsyncWithAck(new CmdCollection(new GetAttitudeControllerData(), new GetRateControllerData()),
                    1000, (SendResult result2) =>
                     {
                         if (result2 == SendResult.OK)
                         {
                             _upToDate = true;
                             LogManager.LogInfoLine("Attitude controller data downloaded from KFly");
                         }
                         else
                         {
                             LogManager.LogErrorLine("Failed downloading attitude controller data");
                         }
                         DownloadBtn.Dispatcher.BeginInvoke(new Action(() =>
                         {
                             DownloadBtn.IsRotating = false;
                         }));
                     });
            }
        }

        private void DownloadBtn_Click(object sender, RoutedEventArgs e)
        {
            Reload();
        }

        private Boolean _upToDate = false;

        private void KFlyTab_TabStateChanged(object sender, TabStateChangedEventArgs e)
        {
            if (!e.IsConnected)
                _upToDate = false;

            if ((e.IsSelected && e.IsConnected) && !_upToDate) //Time to reload
            {
                Reload();
            }
        }

        private void UploadBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!UploadBtn.IsRotating) //Use button to know if we already refreshing
            {
                UploadBtn.IsRotating = true;
                var sacd = new SetAttitudeControllerData()
                {
                    Data = _data.AttitudeCData,
                    RateLimit = _data.LimitCollection.AttitudeRateLimit,
                    AngleLimit = _data.LimitCollection.AngleLimit
                };
                var srcd = new SetRateControllerData()
                {
                    Data = _data.RateCData,
                    RateLimit = _data.LimitCollection.RateLimit,
                };
                Telemetry.SendAsyncWithAck(new CmdCollection(sacd, srcd),
                    1000, (SendResult result) =>
                    {
                        if (result == SendResult.OK)
                        {
                            _upToDate = true;
                            LogManager.LogInfoLine("Attitude controller data uploaded to KFly");
                        }
                        else
                        {
                            LogManager.LogErrorLine("Failed Uploading attitude controller data");
                        }
                        DownloadBtn.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            UploadBtn.IsRotating = false;
                            RateGB.IsInSyncWithController = true;
                            ConstraintsGB.IsInSyncWithController = true;
                            AttitudeGB.IsInSyncWithController = true; 
                        }));
                    });
               
            }
        }

        private void Upload()
        {
            var sacd = new SetAttitudeControllerData()
            {
                Data = _data.AttitudeCData,
                RateLimit = _data.LimitCollection.AttitudeRateLimit,
                AngleLimit = _data.LimitCollection.AngleLimit
            };
            var srcd = new SetRateControllerData()
            {
                Data = _data.RateCData,
                RateLimit = _data.LimitCollection.RateLimit,
            };
            Telemetry.SendAsync(sacd);
            Telemetry.SendAsync(srcd);
        }

        private Boolean _sendingOnFly = false;

        private async void SendOnFlyCB_Checked(object sender, RoutedEventArgs e)
        {
            if (!_sendingOnFly && TheTab.IsActive)
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "I know what I'm doing",
                    NegativeButtonText = "Cancel",
                };
                MessageDialogResult res = await XAMLHelper.GetParent<MainWindow>(this).ShowMessageAsync("Warning!", "This will directly send changes to the KFly when you make them. This can be"+
                    " dangerous if the kfly is sitting on a flying machine and should only be done very carefully",MessageDialogStyle.AffirmativeAndNegative,
                    mySettings);
                if (res == MessageDialogResult.Affirmative)
                {
                    UploadBtn.IsEnabled = false;
                    DownloadBtn.IsEnabled = false;
                    _sendingOnFly = true;
                    Upload();
                }
                else
                {
                    SendOnFlyCB.IsChecked = false;
                }
            }
        }

        private void SendOnFlyCB_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_sendingOnFly)
            {
                UploadBtn.IsEnabled = true;
                DownloadBtn.IsEnabled = true;
                _sendingOnFly = false;
            }
        }

        private void ConstraintsGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (_sendingOnFly)
            {
                Upload();
            }
            else
            {
                if (sender is KFlyGroupBox)
                {
                    (sender as KFlyGroupBox).IsInSyncWithController = false;
                }
            }
        }

        private void RateLockBtn_Checked(object sender, RoutedEventArgs e)
        {
            _data.RateCData.LockPitchRoll = true;
            RateLockBtn.ToolTip = "Unlink Pitch/Roll";
        }

        private void RateLockBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            _data.RateCData.LockPitchRoll = false;
             RateLockBtn.ToolTip = "Link Pitch/Roll";
        }

        private void AttitudeLockBtn_Checked(object sender, RoutedEventArgs e)
        {
            _data.AttitudeCData.LockPitchRoll = true;
            AttitudeLockBtn.ToolTip = "Unlink Pitch/Roll";
        }

        private void AttitudeLockBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            _data.AttitudeCData.LockPitchRoll = false;
            AttitudeLockBtn.ToolTip = "Link Pitch/Roll";
        }

    }
}
