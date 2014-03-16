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
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;


namespace KFly.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SensorValuesWindow : MetroWindow
    {

        private DateTime _start = DateTime.Now;


        private PlotModel CreateModel(String title)
        {
            var plotModel1 = new PlotModel();
            plotModel1.Title = title;
            var linearAxis1 = new DateTimeAxis(AxisPosition.Bottom, _start, _start + TimeSpan.FromMinutes(1));
            linearAxis1.MajorGridlineStyle = LineStyle.Solid;
            linearAxis1.MinorGridlineStyle = LineStyle.Dot;
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis(AxisPosition.Left, "Value");
            linearAxis2.MajorGridlineStyle = LineStyle.Solid;
            linearAxis2.MinorGridlineStyle = LineStyle.Dot;
            plotModel1.Axes.Add(linearAxis2);
            plotModel1.Series.Add(new LineSeries()
            {
                Title = "X",
                Points = new List<IDataPoint>()
            });
            plotModel1.Series.Add(new LineSeries()
            {
                Title = "Y",
                Points = new List<IDataPoint>()
            });
            plotModel1.Series.Add(new LineSeries()
            {
                Title = "Z",
                Points = new List<IDataPoint>()
            });
            return plotModel1;
        }


        public SensorValuesWindow()
        {
             InitializeComponent();
            Gyro.Model = CreateModel("Gyros");
            Accelerometer.Model = CreateModel("Accelerometers");
            Magnometer.Model = CreateModel("Magnometers");
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
        private TeleSubscription _ts2;
        private TimeSpan _updateInterval = new TimeSpan(0, 0, 0, 0, 50); //ms
        private int _updateIntervalGraph = 4;
        private int _updateGraphCounter = 0;
        private void TestCalibration_Loaded(object sender, RoutedEventArgs e)
        {
            _ts = Telemetry.Subscribe(KFlyCommandType.GetSensorData, (GetSensorData cmd) =>
            {
                  double now = DateTimeAxis.ToDouble(DateTime.Now);
                  Gyro.Dispatcher.BeginInvoke(new Action(()=>
                    {
                        (Gyro.Model.Series[0] as LineSeries).Points.Add(new DataPoint(now, cmd.Data.Gyro.X));
                        (Gyro.Model.Series[1] as LineSeries).Points.Add(new DataPoint(now, cmd.Data.Gyro.Y));
                        (Gyro.Model.Series[2] as LineSeries).Points.Add(new DataPoint(now, cmd.Data.Gyro.Z));
                        (Accelerometer.Model.Series[0] as LineSeries).Points.Add(new DataPoint(now, cmd.Data.Accelerometer.X));
                        (Accelerometer.Model.Series[1] as LineSeries).Points.Add(new DataPoint(now, cmd.Data.Accelerometer.Y));
                        (Accelerometer.Model.Series[2] as LineSeries).Points.Add(new DataPoint(now, cmd.Data.Accelerometer.Z));
                        (Magnometer.Model.Series[0] as LineSeries).Points.Add(new DataPoint(now, cmd.Data.Magnometer.X));
                        (Magnometer.Model.Series[1] as LineSeries).Points.Add(new DataPoint(now, cmd.Data.Magnometer.Y));
                        (Magnometer.Model.Series[2] as LineSeries).Points.Add(new DataPoint(now, cmd.Data.Magnometer.Z));

                        _updateGraphCounter++;
                        if (_updateGraphCounter >= _updateIntervalGraph)
                        {
                            _updateGraphCounter = 0;
                            Gyro.RefreshPlot(true);
                            Accelerometer.RefreshPlot(true);
                            Magnometer.RefreshPlot(true);
                        }
                    }));
            });
            _ts2 = Telemetry.Subscribe(KFlyCommandType.GetRawSensorData, (GetRawSensorData cmd) =>
            {
                double now = DateTimeAxis.ToDouble(DateTime.Now);
                Gyro.Dispatcher.BeginInvoke(new Action(() =>
                {
                    (Gyro.Model.Series[0] as LineSeries).Points.Add(new DataPoint(now, cmd.Data.Gyro.X));
                    (Gyro.Model.Series[1] as LineSeries).Points.Add(new DataPoint(now, cmd.Data.Gyro.Y));
                    (Gyro.Model.Series[2] as LineSeries).Points.Add(new DataPoint(now, cmd.Data.Gyro.Z));
                    (Accelerometer.Model.Series[0] as LineSeries).Points.Add(new DataPoint(now, cmd.Data.Accelerometer.X));
                    (Accelerometer.Model.Series[1] as LineSeries).Points.Add(new DataPoint(now, cmd.Data.Accelerometer.Y));
                    (Accelerometer.Model.Series[2] as LineSeries).Points.Add(new DataPoint(now, cmd.Data.Accelerometer.Z));
                    (Magnometer.Model.Series[0] as LineSeries).Points.Add(new DataPoint(now, cmd.Data.Magnometer.X));
                    (Magnometer.Model.Series[1] as LineSeries).Points.Add(new DataPoint(now, cmd.Data.Magnometer.Y));
                    (Magnometer.Model.Series[2] as LineSeries).Points.Add(new DataPoint(now, cmd.Data.Magnometer.Z));

                    _updateGraphCounter++;
                    if (_updateGraphCounter >= _updateIntervalGraph)
                    {
                        _updateGraphCounter = 0;
                        Gyro.RefreshPlot(true);
                        Accelerometer.RefreshPlot(true);
                        Magnometer.RefreshPlot(true);
                    }
                }));
            });
           _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(DispatcherTimer_Tick);
            _timer.Interval = _updateInterval;
            _timer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (Telemetry.IsConnected)
            {
                Telemetry.SendAsync(new GetSensorData());
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
            if (_ts2 != null)
            {
                Telemetry.Unsubscribe(_ts2);
                _ts2 = null;
            }
        }

 
    }
}
