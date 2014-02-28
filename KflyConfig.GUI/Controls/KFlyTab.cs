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

    [TemplatePart(Name = "PART_Toolbar", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PART_NotConnectedModal", Type = typeof(ModalContentPresenter))]
    public class KFlyTab : ContentControl
    {
        public KFlyTab()
        {
            DefaultStyleKey = typeof(KFlyTab);
            this.Loaded += KFlyTab_Loaded;
        }

        void KFlyTab_Loaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                UpdateComponentsOnConnectedChange(true);
            }
            else
            {
                UpdateComponentsOnConnectedChange(IsConnected);
            }
            Telemetry.Subscribe(KFlyCommandType.ConnectionStatusChanged, (ConnectionStatusChanged csc) =>
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        IsConnected = csc.IsConnected;
                    }));
            });
        }

        /// <summary>
        /// Gets or sets additional content for the UserControl
        /// </summary>
        public bool IsConnected
        {
            get { return (bool)GetValue(IsConnectedProperty); }
            set { SetValue(IsConnectedProperty, value); }
        }
        public static readonly DependencyProperty IsConnectedProperty =
            DependencyProperty.Register("IsConnected", typeof(bool), typeof(KFlyTab),
              new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsConnectedChanged));

        private static void IsConnectedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var kflyTab = (KFlyTab)dependencyObject;
            var connected = (bool)e.NewValue;
            kflyTab.UpdateComponentsOnConnectedChange(connected);
        }

        public static readonly DependencyProperty ToolbarProperty = 
            DependencyProperty.Register("Toolbar", typeof(object), typeof(KFlyTab));

        public object Toolbar
        {
            get { return GetValue(ToolbarProperty); }
            set { SetValue(ToolbarProperty, value); }
        }


        private void UpdateComponentsOnConnectedChange(bool connected)
        {
            var toolbar = this.Template.FindName("PART_Toolbar", this) as FrameworkElement;
            if (toolbar != null)
            {
                toolbar.IsEnabled = connected;
            }
            var mcp = this.Template.FindName("PART_NotConnectedModal", this) as ModalContentPresenter;
            if (mcp != null)
            {
                if (connected)
                    mcp.HideModalContent();
                else
                    mcp.ShowModalContent();
            }
        }

        // Create a custom routed event by first registering a RoutedEventID 
        // This event uses the bubbling routing strategy 
        public static readonly RoutedEvent ConnectionEvent = EventManager.RegisterRoutedEvent(
            "ConnectionChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(KFlyTab));

        // Provide CLR accessors for the event 
        public event RoutedEventHandler ConnectionChanged
        {
            add { AddHandler(ConnectionEvent, value); }
            remove { RemoveHandler(ConnectionEvent, value); }
        }

        // This method raises the Tap event 
        void RaiseConnectionEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(KFlyTab.ConnectionEvent);
            RaiseEvent(newEventArgs);
        }

        /// <summary>
        /// THe background of the tab
        /// </summary>
        public ImageSource BackgroundImage
        {
            get { return (ImageSource)GetValue(BackgroundImageProperty); }
            set { SetValue(BackgroundImageProperty, value); }
        }
        public static readonly DependencyProperty BackgroundImageProperty =
            DependencyProperty.Register("BackgroundImage", typeof(ImageSource), typeof(KFlyTab),
              new PropertyMetadata(default(ImageSource)));


        /// <summary>
        /// Title
        /// </summary>
        public String Title
        {
            get { return (String)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(String), typeof(KFlyTab),
              new PropertyMetadata(null));


    }
}
