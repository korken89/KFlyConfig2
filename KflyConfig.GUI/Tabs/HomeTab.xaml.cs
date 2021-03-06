﻿using System;
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
using System.IO;
using System.Reflection;

namespace KFly.GUI
{
    /// <summary>
    /// Interaction logic for HomeTab.xaml
    /// </summary>
    public partial class HomeTab : UserControl
    {
        public HomeTab()
        {
            InitializeComponent();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("KFly.GUI.revision.txt"));
            try
            {
                String version = sr.ReadLine();
                String isDirty = sr.ReadLine();
                if (version == null)
                {
                    version = "No version information found";
                }
                else
                {
                    version = "v." + version;
                }
                if (isDirty != null)
                {
                    version += "~dirty";
                }
                VersionLabel.Content = version;
            }
            finally
            {
                sr.Close();
            }

            Telemetry.Subscribe(KFlyCommandType.ConnectionStatusChanged, (ConnectionStatusChanged msg) =>
                {
                    Connected.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            Connected.Visibility = msg.IsConnected ? Visibility.Visible : Visibility.Hidden;
                            NotConnected.Visibility = msg.IsConnected ? Visibility.Hidden : Visibility.Visible;
                        }));
                });
            
        }

        private Window _testCalibrationWindow;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_testCalibrationWindow == null)
            {
                _testCalibrationWindow = new SensorValuesWindow();
                _testCalibrationWindow.Closed += (o, args) => _testCalibrationWindow = null;
            }
            if (_testCalibrationWindow.IsVisible)
                _testCalibrationWindow.Hide();
            else
                _testCalibrationWindow.Show();

        }

        private void ConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            XAMLHelper.GetParent<MainWindow>(this).ConnectionFlyout.IsOpen = true;
        }

    }
}
