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
        /// True if connected and tab selected/visible
        /// </summary>
        public bool IsActive
        {
            get
            {
                return (IsConnected && IsSelected);
            }
        }

        /// <summary>
        /// Gets or sets additional content for the UserControl
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(KFlyTab),
              new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsSelectedChanged));

        private static void IsSelectedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var kflyTab = (KFlyTab)dependencyObject;
            kflyTab.RaiseTabStateChangedEvent();
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
            kflyTab.RaiseTabStateChangedEvent();
        }

        /// <summary>
        /// Has 
        /// </summary>
       public bool IsUpToDate
        {
            get { return (bool)GetValue(IsUpToDateProperty); }
            set { SetValue(IsUpToDateProperty, value); }
        }
       public static readonly DependencyProperty IsUpToDateProperty =
            DependencyProperty.Register("IsUpToDate", typeof(bool), typeof(KFlyTab));
          


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
        public static readonly RoutedEvent TabStateChangedEvent = EventManager.RegisterRoutedEvent(
            "TabStateChangedChanged", RoutingStrategy.Bubble, typeof(TabStateChangedEventHandler), typeof(KFlyTab));

        // Provide CLR accessors for the event 
        public event TabStateChangedEventHandler TabStateChanged
        {
            add { AddHandler(TabStateChangedEvent, value); }
            remove { RemoveHandler(TabStateChangedEvent, value); }
        }

        // This method raises the Connection Event, but only if this is the selected tab
        private void RaiseTabStateChangedEvent()
        {
            TabStateChangedEventArgs newEventArgs = new TabStateChangedEventArgs(KFlyTab.TabStateChangedEvent)
                {
                    IsConnected = this.IsConnected,
                    IsSelected = this.IsSelected
                };
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

    public delegate void TabStateChangedEventHandler(object sender, TabStateChangedEventArgs e);

    public class TabStateChangedEventArgs: RoutedEventArgs
    {
        public TabStateChangedEventArgs():base()
        { }
        public TabStateChangedEventArgs(RoutedEvent routedEvent): base(routedEvent)
        { }

        public Boolean IsConnected = false;
        public Boolean IsSelected = false;

     
    }

}
