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
    public partial class DataDumpTab : UserControl
    {
        public DataDumpTab()
        {
            InitializeComponent();
            var ddvm = new DataDumpViewModel();
            this.DataContext = ddvm;
            Telemetry.Subscribe(KFlyCommandType.All, (KFlyCommand cmd) =>
                {
                    DataDumpLog(cmd, ddvm);
                });
      
        }

        private void DataDumpLog(KFlyCommand cmd, DataDumpViewModel ddvm)
        {
            if (ddvm.Running)
            {
                ddvm.Data.Push(new KeyValuePair<DateTime,KFlyCommand>(DateTime.Now, cmd));
            }
        }

        private void KFlyTab_TabStateChanged(object sender, TabStateChangedEventArgs e)
        {
        }

     
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            var ddvm = this.DataContext as DataDumpViewModel;
            ddvm.Start = DateTime.Now;
            ddvm.Running = true;
            var col = new List<ManageSubscriptions>();
            foreach (ChoiseKFlyCommand ck in ddvm.Collection)
            {
                if (ck.IsChecked)
                {
                    ddvm.Subscriptions.Add(ck.Type);
                    col.Add(new ManageSubscriptions()
                    {
                        Data = new SubscriptionData()
                        {
                            KFlyCommand = ck.Type,
                            Port = KFlyPort.CURRENT,
                            Command = SubscriptionData.Cmd.ON,
                            DeltaTime = ddvm.DeltaTime
                        },
                        
                    });
                        
                }
            }
            Telemetry.SendAsyncWithAck(new CmdCollection(col.ToArray()), 1000, (SendResult sr) =>
                {
                    if (sr != SendResult.OK)
                    {
                        LogManager.LogErrorLine("Failed subscribe");
                    }

                });
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            var ddvm = this.DataContext as DataDumpViewModel;
            ddvm.Running = false;
            ddvm.Stop = DateTime.Now;
            var col = new List<ManageSubscriptions>();
            foreach (KFlyCommandType type in ddvm.Subscriptions)
            {
                col.Add(new ManageSubscriptions()
                {
                    Data = new SubscriptionData()
                    {
                        KFlyCommand = type,
                        Port = KFlyPort.CURRENT,
                        Command = SubscriptionData.Cmd.OFF
                    }
                });
            }
            Telemetry.SendAsyncWithAck(new CmdCollection(col.ToArray()), 1000, (SendResult sr) =>
            {
                if (sr != SendResult.OK)
                {
                    LogManager.LogErrorLine("Failed unsubscribe");
                }

            });
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            var ddvm = this.DataContext as DataDumpViewModel;
            ddvm.Data.Clear();
            ddvm.Stop = ddvm.Start;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var ddvm = this.DataContext as DataDumpViewModel;
            if (ddvm.Running == false)
            {
                GUIDataDumper.DumpDataToFile(ddvm.Data);
            }
          
        }
    }
}
