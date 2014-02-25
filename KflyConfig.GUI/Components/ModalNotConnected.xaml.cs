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

namespace KFly.GUI
{
    /// <summary>
    /// Interaction logic for ModalNotConnected.xaml
    /// </summary>
    public partial class ModalNotConnected : UserControl
    {
        public ModalNotConnected()
        {
            InitializeComponent();
        }

        private void openConnectionSettings_Click(object sender, RoutedEventArgs e)
        {
            var parent = this.Parent;
            while (!(parent is MainWindow) && (parent is FrameworkElement))
            {
                parent = (parent as FrameworkElement).Parent;
            }
            if (parent is MainWindow)
            {
                (parent as MainWindow).ConnectionFlyout.IsOpen = true;
            }
        }
    }
}
