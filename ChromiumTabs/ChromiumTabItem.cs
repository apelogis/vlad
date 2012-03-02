﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace ChromiumTabs
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ChromiumTabs"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ChromiumTabs;assembly=ChromiumTabs"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:ChromiumTabItem/>
    ///
    /// </summary>
    public class ChromiumTabItem : HeaderedContentControl
    {
        public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof(ChromiumTabItem), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        static ChromiumTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChromiumTabItem), new FrameworkPropertyMetadata(typeof(ChromiumTabItem)));
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        private void HandleMouseMove(object sender, MouseEventArgs e)
        {
            ParentTabControl.MouseMove -= HandleMouseMove;
            if (this.hasButtonDown)
            {
                Point nowPoint = e.GetPosition(ParentTabControl);
                Thickness margin = new Thickness(nowPoint.X - downPoint.X, this.Margin.Top, downPoint.X - nowPoint.X, this.Margin.Bottom);
                SetValue(FrameworkElement.MarginProperty, margin);
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            downPoint = e.GetPosition(ParentTabControl);
            this.hasButtonDown = true;
            ParentTabControl.PreviewMouseMove += HandleMouseMove;
            ParentTabControl.PreviewMouseLeftButtonUp += HandleMouseUp;
            base.OnMouseLeftButtonDown(e);
        }

        private void HandleMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.hasButtonDown && !this.IsSelected)
            {
                ParentTabControl.SelectedItem = this;
            }
            this.hasButtonDown = false;
        }

        private ChromiumTabControl ParentTabControl
        {
            get { return ItemsControl.ItemsControlFromItemContainer(this) as ChromiumTabControl; }
        }

        private bool hasButtonDown;
        private Point downPoint;
    }
}