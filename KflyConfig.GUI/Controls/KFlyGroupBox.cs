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
using System.ComponentModel;

namespace KFly.GUI
{

    public class KFlyGroupBox : GroupBox
    {
        public KFlyGroupBox()
        {
            DefaultStyleKey = typeof(KFlyGroupBox);
        }

        /// <summary>
        /// Gets or sets additional content for the UserControl
        /// </summary>
        public bool IsInSyncWithController
        {
            get { return (bool)GetValue(IsInSyncWithControllerProperty); }
            set { SetValue(IsInSyncWithControllerProperty, value); }
        }
        public static readonly DependencyProperty IsInSyncWithControllerProperty =
            DependencyProperty.Register("IsInSyncWithController", typeof(bool), typeof(KFlyGroupBox),
              new FrameworkPropertyMetadata(true));


    }

}
