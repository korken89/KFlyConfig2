using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace KFly.GUI
{
    public static class XAMLHelper
    {
        public static T GetParent<T>(FrameworkElement child) where T : DependencyObject
        {
            var parent = child.Parent;
            while (!(parent is MainWindow) && (parent is FrameworkElement))
            {
                if ((parent as FrameworkElement).Parent == null)
                {
                    parent = (parent as FrameworkElement).TemplatedParent;
                }
                else
                {
                    parent = (parent as FrameworkElement).Parent;
                }
            }
            return parent as T;
        }
    }   
}
