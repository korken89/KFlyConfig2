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
            while (!(parent is T) && (parent is FrameworkElement))
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

        /// <summary>
        /// THis CAN be expensive if panel wiht many children is used
        /// using BFS
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <returns></returns>
        public static T GetChild<T>(ContentControl parent) where T : DependencyObject
        {
            var child = parent.Content;
            while (!(child is T) && (child is ContentControl))
            {
                child = (child as ContentControl).Content;
            }
            return child as T;
        }
    }   
}
