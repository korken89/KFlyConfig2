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
using System.Collections.Concurrent;
using System.ComponentModel;

namespace KFly.GUI
{
    /// <summary>
    /// Interaction logic for ModalNotConnected.xaml
    /// </summary>
    public partial class CalibrateSensors : UserControl
    {
        private readonly BackgroundWorker _collectingWorker = new BackgroundWorker();
        private readonly BackgroundWorker _calculatingWorker = new BackgroundWorker();
        private SixStepCalibrationData _data;

        public CalibrateSensors()
        {
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

        private SensorCalibration _latestResult;
        void _calculatingWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SensorCalibration sc = e.Result as SensorCalibration;
            if (sc != null)
            {
                ResultLabel.Text = sc.ToString();
                _latestResult = sc;
                _data.Subs[6] = SixStepCalibrationData.SubSteps.Finished;
            }
            else
            {
                _data.Subs[6] = SixStepCalibrationData.SubSteps.Error;
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
                e.Result = SixPositionSensorCalibration.Calibrate(data);
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
                _data.CurrentSubStep = SixStepCalibrationData.SubSteps.Error;
                ErrorLabel.Content = e.Error.Message;
            }
            else if (!e.Cancelled)
            {
                _data.CurrentSubStep = SixStepCalibrationData.SubSteps.Finished;
                _data.CurrentStep++;
                if (_data.CurrentStep == 6)
                {
                    _toBeCalculated.Enqueue(new List<RawSensorData>(_data.RawData));
                    _data.CurrentSubStep = SixStepCalibrationData.SubSteps.Working; 
                    _calculatingWorker.RunWorkerAsync();
                }
                UpdateControls();
            }
            else //canceled
            {
                _data.CurrentSubStep = (_data.CurrentDataBag.Count >= 200) ? SixStepCalibrationData.SubSteps.Finished :
                    SixStepCalibrationData.SubSteps.NotStarted;
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

        private void UpdateControls()
        {
            //Todo: Set picture depending on Step here

            //Todo: Set text depending on Step here

            NextBtn.Visibility = (_data.CurrentSubStep == SixStepCalibrationData.SubSteps.Finished)? Visibility.Visible
                : Visibility.Hidden;
            NextBtn.IsEnabled = (_data.CurrentSubStep != SixStepCalibrationData.SubSteps.Working);
           
            LastBtn.Visibility = (_data.CurrentStep > 0) ? Visibility.Visible : Visibility.Collapsed;
            LastBtn.IsEnabled = (_data.CurrentSubStep != SixStepCalibrationData.SubSteps.Working);
           
            for (int i = 0; i < 4; i++)
            {
                ContentGrid.Children[i].Visibility = ((int)_data.CurrentSubStep == i) ? Visibility.Visible : Visibility.Hidden;
            }

            CollectingPanel.Visibility = (_data.CurrentStep < 6) ? Visibility.Visible : Visibility.Hidden;
            ResultPanel.Visibility = (_data.CurrentStep == 6) ? Visibility.Visible : Visibility.Hidden;

            UseDataBtn.IsEnabled = (_latestResult != null);

            StepLabel.Content = String.Format("Step {0} of 7", _data.CurrentStep+1);
        }

      
        private void UserControl_Initialized(object sender, EventArgs e)
        {
            _data = new SixStepCalibrationData();
            this.DataContext = _data;
            UpdateControls();
        }

      
        private void CollectDataBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!_collectingWorker.IsBusy)
            {
                _data.ClearCurrentDataBag();
                _data.CurrentSubStep = SixStepCalibrationData.SubSteps.Working;
                UpdateControls();
                _collectingWorker.RunWorkerAsync();
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((_data.CurrentStep < 7) && (_data.CurrentSubStep == SixStepCalibrationData.SubSteps.Finished))
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
                _data.CurrentSubStep = SixStepCalibrationData.SubSteps.Working;
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


    }


    public class SixStepCalibrationData
    {
        public enum SubSteps
        {
            NotStarted = 0,
            Working = 1,
            Error = 2,
            Finished = 3
        }

        private uint _currentStep = 0;

        private SubSteps[] _subSteps = new SubSteps[7];
        private ConcurrentBag<RawSensorData>[] _data = new ConcurrentBag<RawSensorData>[7];

        public IEnumerable<RawSensorData> RawData
        {
            get
            {
                for (int i = 0; i < 6; i++)
                {
                    foreach (RawSensorData rsd in _data[i])
                    {
                        yield return rsd;
                    }
                }
            }
        }

        public SubSteps[] Subs
        {
            get
            {
                return _subSteps;
            }
        }

        public SixStepCalibrationData()
        {
            for (var i = 0; i < 7; i++)
            {
                _subSteps[i] = SubSteps.NotStarted;
                _data[i] = new ConcurrentBag<RawSensorData>();
            }
        }

        public SubSteps CurrentSubStep
        {
            get
            {
                return _subSteps[_currentStep];
            }
            set
            {
                _subSteps[_currentStep] = value;
            }
        }

        public uint CurrentStep
        {
            get
            {
                return _currentStep;
            }
            set
            {
                _currentStep = Math.Min(6,value);
            }
        }

        public ConcurrentBag<RawSensorData> CurrentDataBag
        {
            get
            {
                return _data[_currentStep];
            }
        }

        public void ClearCurrentDataBag()
        {
            RawSensorData someItem;
            var bag = _data[_currentStep];
            while (!bag.IsEmpty) 
            {
                bag.TryTake(out someItem);
            }
        }

    }
}
