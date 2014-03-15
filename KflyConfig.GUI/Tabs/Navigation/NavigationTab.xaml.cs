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
    public partial class NavigationTab : UserControl
    {
        public NavigationTab()
        {
            InitializeComponent();
        }

       
       
        private void UserControl_Initialized(object sender, EventArgs e)
        {
        }

        private void ComboBox_Selected(object sender, RoutedEventArgs e)
        {
            ComboBox b = sender as ComboBox;
            b.Height = 20;
        }

        private void ComboBox_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            ComboBox b = sender as ComboBox;
            b.Height = 20;

        }

        private void LayerBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //LayerMenu.IsOpen = true;
        }


    }
}
