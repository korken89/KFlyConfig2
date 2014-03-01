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
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Threading;
using System.IO;

namespace KFly.GUI
{
    /// <summary>
    /// Interaction logic for ModalNotConnected.xaml
    /// </summary>
    public partial class CalibrateSensors : UserControl
    {
        private readonly BackgroundWorker _collectingWorker = new BackgroundWorker();
        private readonly BackgroundWorker _calculatingWorker = new BackgroundWorker();
        private SixPointsCalibrationData _data;

        public CalibrateSensors()
        {
            _data = new SixPointsCalibrationData();
            this.DataContext = _data;
            InitializeComponent();
 
            _collectingWorker.WorkerReportsProgress = true;
            _collectingWorker.WorkerSupportsCancellation = true;
            _collectingWorker.DoWork += _collectingWorker_DoWork;
            _collectingWorker.RunWorkerCompleted += _collectingWorker_RunWorkerCompleted;
            _collectingWorker.ProgressChanged += _collectingWorker_ProgressChanged;
            _calculatingWorker.WorkerSupportsCancellation = true;
            _calculatingWorker.DoWork += _calculatingWorker_DoWork;
            _calculatingWorker.RunWorkerCompleted += _calculatingWorker_RunWorkerCompleted;    
        }

        void _calculatingWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SensorCalibration sc = e.Result as SensorCalibration;
            if (sc != null)
            {
                _data.CurrentResult = sc;
                if (sc.IsValid)
                {
                    _data.Subs[6] = SixPointsCalibrationData.SubSteps.Finished;
                }
                else
                {
                    _data.Subs[6] = SixPointsCalibrationData.SubSteps.Error;
                }
            }
            else
            {
                _data.Subs[6] = SixPointsCalibrationData.SubSteps.Error;
            }
            UpdateControls();
        }

        private ConcurrentQueue<List<RawSensorData>> _toBeCalculated = new ConcurrentQueue<List<RawSensorData>>();
        
        void _calculatingWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<RawSensorData> data;
            while (_toBeCalculated.TryDequeue(out data)) 
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                e.Result = SixPointSensorCalibration.Calibrate(data);
            }
        }

        void _collectingWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressLabel.Content = String.Format("Collecting sensor data: {0} of 200", _data.CurrentDataBag.Count);
        }

        void _collectingWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                _data.CurrentSubStep = SixPointsCalibrationData.SubSteps.Error;
                ErrorLabel.Content = e.Error.Message;
            }
            else if (!e.Cancelled)
            {
                _data.CurrentSubStep = SixPointsCalibrationData.SubSteps.Finished;
                _data.CurrentStep++;
                if (_data.CurrentStep == 6)
                {
                    _toBeCalculated.Enqueue(new List<RawSensorData>(_data.RawData));
                    _data.CurrentSubStep = SixPointsCalibrationData.SubSteps.Working; 
                    _calculatingWorker.RunWorkerAsync();
                }
                UpdateControls();
            }
            else //canceled
            {
                _data.CurrentSubStep = (_data.CurrentDataBag.Count >= 200) ? SixPointsCalibrationData.SubSteps.Finished :
                    SixPointsCalibrationData.SubSteps.NotStarted;
            }
        }

        void _collectingWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            BlockingCollection<RawSensorData> _temp = new BlockingCollection<RawSensorData>(); 
            var subscription = Telemetry.Subscribe(KFlyCommandType.GetRawSensorData, (GetRawSensorData msg) =>
                {
                    _temp.Add(msg.Data);
                });
            int dataCount = 0;
            while (dataCount < 200)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                Telemetry.SendAsync(new GetRawSensorData());
                RawSensorData data = null;
                if (_temp.TryTake(out data, 100))
                {
                    _data.CurrentDataBag.Add(data);
                    dataCount = _data.CurrentDataBag.Count;
                    worker.ReportProgress(dataCount / 2);
                }
                System.Threading.Thread.Sleep(20);
            }
            Telemetry.Unsubscribe(subscription);
        }


        private void openConnectionSettings_Click(object sender, RoutedEventArgs e)
        {
            var parent = this.Parent;
            while (!(parent is MainWindow) && (parent is FrameworkElement))
            {
                parent = (parent as FrameworkElement).Parent;
            }
            if (parent is MainWindow)
            {
                (parent as MainWindow).ConnectionFlyout.IsOpen = true;
            }
        }

        private uint _lastStep = 0;

        private void UpdateControls()
        {
            //Todo: Set picture depending on Step here
            if ((_lastStep != _data.CurrentStep) && (_data.CurrentStep < 6))
            {
                AxisAngleRotation3D rotateAxis;
                switch (_data.CurrentStep)
                {
                    case 0:
                        rotateAxis = new AxisAngleRotation3D(new Vector3D(-1, 0, 0), 0);
                        break;
                    case 1:
                        rotateAxis = new AxisAngleRotation3D(new Vector3D(-1, 0, 0), 90);
                        break;
                    case 2:
                        rotateAxis = new AxisAngleRotation3D(new Vector3D(-1, 0, 0), 180);
                        break;
                    case 3:
                        rotateAxis = new AxisAngleRotation3D(new Vector3D(-1, 0, 0), 270);
                        break;
                    case 4:
                        rotateAxis = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 90);
                        break;
                    default:
                        rotateAxis = new AxisAngleRotation3D(new Vector3D(0, 1, 0), -90);
                        break;
                }
                var ranim = new Rotation3DAnimation(rotateAxis, TimeSpan.FromSeconds(1.5));
                KFlyRotation.BeginAnimation(RotateTransform3D.RotationProperty, ranim);
            }

            //Todo: Set text depending on Step here

            NextBtn.Visibility = (_data.CurrentSubStep == SixPointsCalibrationData.SubSteps.Finished)? Visibility.Visible
                : Visibility.Hidden;
            NextBtn.IsEnabled = (_data.CurrentSubStep != SixPointsCalibrationData.SubSteps.Working);
           
            LastBtn.Visibility = (_data.CurrentStep > 0) ? Visibility.Visible : Visibility.Collapsed;
            LastBtn.IsEnabled = (_data.CurrentSubStep != SixPointsCalibrationData.SubSteps.Working);
           
            for (int i = 0; i < 4; i++)
            {
                ContentGrid.Children[i].Visibility = ((int)_data.CurrentSubStep == i) ? Visibility.Visible : Visibility.Hidden;
            }

            CollectingPanel.Visibility = (_data.CurrentStep < 6) ? Visibility.Visible : Visibility.Hidden;
            ResultPanel.Visibility = (_data.CurrentStep == 6) ? Visibility.Visible : Visibility.Hidden;

            UseDataBtn.IsEnabled = (_data.CurrentResult.IsValid);

            StepLabel.Content = String.Format("Step {0} of 7", _data.CurrentStep+1);

            //Calculation panel
            CalculationWorking.Visibility = 
                ((_data.CurrentStep == 6) && (_data.CurrentSubStep == SixPointsCalibrationData.SubSteps.Working)) ?
                Visibility.Visible : Visibility.Hidden;
            CalculationResult.Visibility =
               ((_data.CurrentStep == 6) && (_data.CurrentSubStep != SixPointsCalibrationData.SubSteps.Working)) ?
               Visibility.Visible : Visibility.Hidden;
            if ((_data.CurrentStep == 6) && (_data.CurrentSubStep != SixPointsCalibrationData.SubSteps.Finished))
            {
                CalculationResultLabel.Text = "Calculation failed! This might be because of you not using all 6 position correctly.\nYou can either redo the whole process or go back and redo single positions.";
                CalculationResultLabel.Foreground = Brushes.Red;
                UseDataBtn.Content = "Restart calibration";
            }
            else if (_data.CurrentStep == 6) //ERROR
            {
                CalculationResultLabel.Text = "Calculation Successful!";
                CalculationResultLabel.Foreground = Brushes.Black;
                UseDataBtn.Content = "Use this result";
            }

            _lastStep = _data.CurrentStep;
        }

      
        private void UserControl_Initialized(object sender, EventArgs e)
        {
           UpdateControls();
        }

      
        private void CollectDataBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!_collectingWorker.IsBusy)
            {
                _data.ClearCurrentDataBag();
                _data.CurrentSubStep = SixPointsCalibrationData.SubSteps.Working;
                UpdateControls();
                _collectingWorker.RunWorkerAsync();
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((_data.CurrentStep < 7) && (_data.CurrentSubStep == SixPointsCalibrationData.SubSteps.Finished))
            {
                _data.CurrentStep++;
                UpdateControls();
            }
        }

        private void LastBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_data.CurrentStep > 0)
            {
                _data.CurrentStep--;
                UpdateControls();
            }
        }
        
        private void ReCollectDataBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!_collectingWorker.IsBusy)
            {
                _data.ClearCurrentDataBag();
                _data.CurrentSubStep = SixPointsCalibrationData.SubSteps.Working;
                UpdateControls();
                _collectingWorker.RunWorkerAsync();
            }
        }

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);
            if ((parent != null) && (!(parent is T)))
            {
                return FindParent<T>(parent);           
            }
            return parent as T;
        }

        private void AbortBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_collectingWorker.IsBusy)
            {
                _collectingWorker.CancelAsync();
            }
            if (_calculatingWorker.IsBusy)
            {
                _calculatingWorker.CancelAsync();
            }
            FindParent<ModalContentPresenter>(this).HideModalContent();
        }

        private void UseDataBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_data.CurrentSubStep == SixPointsCalibrationData.SubSteps.Finished)
            {
                if (_collectingWorker.IsBusy)
                {
                    _collectingWorker.CancelAsync();
                }
                if (_calculatingWorker.IsBusy)
                {
                    _calculatingWorker.CancelAsync();
                }
                Telemetry.Handle(new GetSensorCalibration() { Data = _data.CurrentResult });
                FindParent<ModalContentPresenter>(this).HideModalContent();
            }
            else if (_data.CurrentSubStep == SixPointsCalibrationData.SubSteps.Error)
            {
                _data.FullReset();
                UpdateControls();
            }
        }

        private void view1_Initialized(object sender, EventArgs e)
        {
            Load3DModel();
        }

        private async void Load3DModel()
        {
            var streamResourceInfo = Application.GetResourceStream(new Uri("../Resources/kfly.stl", UriKind.Relative));

              Model3D test =  await this.aLoadAsync(streamResourceInfo.Stream);
              await view1.Dispatcher.BeginInvoke(new Action(() =>
                  {
                      MyModel.Content = test;
                  }));
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
