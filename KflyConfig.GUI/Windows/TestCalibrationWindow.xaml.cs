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
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Resources;
using System.IO;
using System.Windows.Media.Media3D;
using System.Windows.Interop;
using KflyConfig.GUI;
using MahApps.Metro.Controls;
using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
using KFly.Logging;


namespace KFly.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TestCalibrationWindow : MetroWindow
    {
        public TestCalibrationWindow()
        {
            InitializeComponent();
        }


            
       

        private void SubscribeToCommunication()
        {
            //Log window
            Telemetry.Subscribe(KFlyCommandType.ConnectionStatusChanged, (ConnectionStatusChanged cmd) =>
            {
             /*   ConnectionStatusText.Dispatcher.BeginInvoke((Action)(()=>
                    {
                        ConnectionStatusText.Text = (cmd.IsConnected)? "Connected" : "Not connected";
                        if (cmd.IsConnected)
                        {
                            ConnectionStatusIcon.Content = FindResource("connected");
                        }
                        else
                        {
                            ConnectionStatusIcon.Content = FindResource("disconnected");
                        }
                    }));*/
            });

        }

        private DispatcherTimer _timer;
        private TeleSubscription _ts;
        private TimeSpan _updateInterval = new TimeSpan(0, 0, 0, 0, 500); //500 ms
        private void TestCalibration_Loaded(object sender, RoutedEventArgs e)
        {
            _ts = Telemetry.Subscribe(KFlyCommandType.GetEstimationAttitude, (GetEstimationAttitude cmd) =>
            {
                QuaternionRotation3D rot = new QuaternionRotation3D(
                    new System.Windows.Media.Media3D.Quaternion(
                        cmd.Data.X, cmd.Data.Y, cmd.Data.Z, cmd.Data.W));
                view1.Dispatcher.BeginInvoke(new Action(()=>
                    {
                        KFlyRotation.Rotation = rot;
                    }));
            });
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(DispatcherTimer_Tick);
            _timer.Interval = _updateInterval;
            _timer.Start();
            Load3DModel();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (Telemetry.IsConnected)
            {
                Telemetry.SendAsync(new GetEstimationAttitude());
            }
        }

        private void TestCalibration_Closed(object sender, EventArgs e)
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }
            if (_ts != null)
            {
                Telemetry.Unsubscribe(_ts);
                _ts = null;
            }
        }

        private async void Load3DModel()
        {
            try
            {
                var streamResourceInfo = Application.GetResourceStream(new Uri("../Resources/kfly.stl", UriKind.Relative));

                Model3D test = await this.aLoadAsync(streamResourceInfo.Stream);
                await view1.Dispatcher.BeginInvoke(new Action(() =>
                {
                    MyModel.Content = test;
                }));
            }
            catch
            { };
        }

        private async Task<Model3DGroup> aLoadAsync(Stream stream)
        {
            return await Task.Factory.StartNew(() =>
            {
                var mi = new StLFromInventorReader(this.Dispatcher);
                return mi.Read(stream);
            });
        }

 
    }
}
