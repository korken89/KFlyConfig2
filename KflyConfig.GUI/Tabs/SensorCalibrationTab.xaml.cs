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
    public partial class SensorCalibrationTab : UserControl
    {
        public SensorCalibrationTab()
        {
            InitializeComponent();
        }

        private void UpdateSensorCalibrationData()
        {
            LogManager.LogInfoLine("Requesting sensor calibration");
            Telemetry.SendAsync(new GetSensorCalibration());
        }
      
        private void UserControl_Initialized(object sender, EventArgs e)
        {
            SensorCalibration sd = new SensorCalibration();
            this.AccelerometerGrid.DataContext = sd;
            this.MagnometerGrid.DataContext = sd;

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
                    AccelerometerGrid.DataContext = msg.Data;
                    MagnometerGrid.DataContext = msg.Data;
                }));
                LogManager.LogInfoLine("Sensor calibration recevied!");
            });
          
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CalibrationModal.ShowModalContent();
        }


    }
}
