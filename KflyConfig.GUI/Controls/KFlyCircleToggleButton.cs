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
using System.Windows.Controls.Primitives;

namespace KFly.GUI
{

    public class KFlyCircleToggleButton : ToggleButton
    {
   
        static KFlyCircleToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KFlyCircleToggleButton), new FrameworkPropertyMetadata(typeof(KFlyCircleToggleButton)));

        }

        /// <summary>
        /// Gets/sets the diameter of the ellipses used in the indeterminate animation.
        /// </summary>
        public Visual CheckedIcon
        {
            get { return (Visual)GetValue(CheckedIconProperty); }
            set { SetValue(CheckedIconProperty, value); }
        }
        public static readonly DependencyProperty CheckedIconProperty =
            DependencyProperty.Register("CheckedIcon", typeof(Visual), typeof(KFlyCircleToggleButton),
                           new PropertyMetadata(default(Visual)));

        /// <summary>
        /// Gets/sets the diameter of the ellipses used in the indeterminate animation.
        /// </summary>
        public Visual UncheckedIcon
        {
            get { return (Visual)GetValue(UncheckedIconProperty); }
            set { SetValue(UncheckedIconProperty, value); }
        }
        public static readonly DependencyProperty UncheckedIconProperty =
            DependencyProperty.Register("UncheckedIcon", typeof(Visual), typeof(KFlyCircleToggleButton),
                           new PropertyMetadata(default(Visual)));

     
    
    }
}
