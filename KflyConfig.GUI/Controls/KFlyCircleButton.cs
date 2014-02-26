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
   
    public class KFlyCircleButton : Button
    {
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Visual), typeof(KFlyCircleButton),
                                 new PropertyMetadata(default(Visual)));

        static KFlyCircleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KFlyCircleButton), new FrameworkPropertyMetadata(typeof(KFlyCircleButton)));
        }

        /// <summary>
        /// Gets/sets the diameter of the ellipses used in the indeterminate animation.
        /// </summary>
        public Visual Icon
        {
            get { return (Visual)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
    }
}
