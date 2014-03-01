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

    [TemplateVisualState(Name = "Still", GroupName = "RotateStates")]
    [TemplateVisualState(Name = "Rotating", GroupName = "RotateStates")]
    public class KFlyCircleButton : Button
    {
        private List<Action> _deferredActions = new List<Action>();

        static KFlyCircleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KFlyCircleButton), new FrameworkPropertyMetadata(typeof(KFlyCircleButton)));
            VisibilityProperty.OverrideMetadata(typeof(KFlyCircleButton), new FrameworkPropertyMetadata(new PropertyChangedCallback((cButton, e) =>
                    {
                        //auto set IsActive to false if we're hiding it.
                        if ((Visibility)e.NewValue != Visibility.Visible)
                        {
                            var ring = (KFlyCircleButton)cButton;
                            ring.SetCurrentValue(KFlyCircleButton.IsRotatingProperty, false); //sets the value without overriding it's binding (if any).
                        }
                    })));
        }

        /// <summary>
        /// Gets/sets the diameter of the ellipses used in the indeterminate animation.
        /// </summary>
        public Visual Icon
        {
            get { return (Visual)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Visual), typeof(KFlyCircleButton),
                           new PropertyMetadata(default(Visual)));

        /// <summary>
        /// Gets or sets additional content for the UserControl
        /// </summary>
        public bool IsRotating
        {
            get { return (bool)GetValue(IsRotatingProperty); }
            set { SetValue(IsRotatingProperty, value); }
        }
        public static readonly DependencyProperty IsRotatingProperty =
            DependencyProperty.Register("IsRotating", typeof(bool), typeof(KFlyCircleButton),
              new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsRotatingChanged));

        private static void IsRotatingChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var button = dependencyObject as KFlyCircleButton;
            if (button == null)
                return;

            button.UpdateRotateState();
        }


        private void UpdateRotateState()
        {
            Action action;

            if (IsRotating)
                action = () => VisualStateManager.GoToState(this, "Rotating", true);
            else
                action = () => VisualStateManager.GoToState(this, "Still", true);

            if (_deferredActions != null)
                _deferredActions.Add(action);

            else
                action();
        }

        public override void OnApplyTemplate()
        {
            //make sure the states get updated
            UpdateRotateState();
            base.OnApplyTemplate();
            if (_deferredActions != null)
                foreach (var action in _deferredActions)
                    action();
            _deferredActions = null;
        }
    }
}
