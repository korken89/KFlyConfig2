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
using System.Windows.Media.Animation;
using System.ComponentModel;
using KFly.Communication;
using KFly.Logging;

namespace KFly.GUI
{
    /// <summary>
    /// Interaction logic for HomeTab.xaml
    /// </summary>
    public partial class LogPanel : DockPanel, INotifyPropertyChanged
    {
        public LogPanel()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private double _logPanelExpandedHeight = 200;
        public double LogPanelExpandedHeight
        {
            get
            {
                return _logPanelExpandedHeight;
            }
            set
            {
                _logPanelExpandedHeight = value;
                OnPropertyChanged("LogPanelExpandedHeight");
            }
        }

        private void LogPanelExpand_Completed(object sender, EventArgs e)
        {
            this.Height = Math.Max(LogPanelExpandedHeight, 200);
            SetExpandBtnResource();
        }

        private void LogPanelShrink_Completed(object sender, EventArgs e)
        {
            this.Height = 30;
            SetExpandBtnResource();
        }

        private Boolean _isResizingLog = false;
        private Point _clickPosition;
        private double _startHeight;
        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isResizingLog = true;
            _clickPosition = e.GetPosition(this.Parent as IInputElement);
            _startHeight = this.Height;
            (sender as DockPanel).CaptureMouse();
        }

        private void Header_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isResizingLog)
            {
                var point = e.GetPosition(this.Parent as IInputElement);
                var newHeight = Math.Max(30, _startHeight + (_clickPosition.Y - point.Y));
                if (newHeight < 45)
                    newHeight = 30;
                this.Height = newHeight;
                SetExpandBtnResource();
            }
        }

        private void Header_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isResizingLog = false;
            (sender as DockPanel).ReleaseMouseCapture();
            if (this.Height == 30)
            {
                LogPanelExpandedHeight = _startHeight;
            }
            else
            {
                LogPanelExpandedHeight = this.Height;
            }
        }

        private void MainControl_Initialized(object sender, EventArgs e)
        {
            LogManager.AddLogDestination(new RichTextBoxLog(LogBox));
            LogManager.AddLogDestination(new LabelLog(LogRowLabel));
        }

        private void LogBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AutoScrollCB.IsChecked == true)
                LogBox.ScrollToEnd();
        }

        private void SetExpandBtnResource()
        {
            ExpandLogPanelBtn.Content = (Height == 30)? FindResource("arrowUp") : FindResource("arrowDown");
            AutoScrollCB.Visibility = (Height == 30)? Visibility.Collapsed: Visibility.Visible;
            Debug.Visibility = (Height == 30) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void ExpandLogPanelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.Height == 30)
            {
                var st = (Storyboard)FindResource("LogPanelExpand");
                (st.Children[0] as DoubleAnimation).To = Math.Max(LogPanelExpandedHeight, 200);
                BeginStoryboard(st);
                ExpandLogPanelBtn.ToolTip = "Hide log";
            }
            else
            {
                ExpandLogPanelBtn.Content = FindResource("arrowUp");
                BeginStoryboard((Storyboard)FindResource("LogPanelShrink"));
                ExpandLogPanelBtn.ToolTip = "Show log";
            }
        }

        private void Debug_Click(object sender, RoutedEventArgs e)
        {
            LogManager.Debug = Debug.IsChecked == true;
        }
    }
}
