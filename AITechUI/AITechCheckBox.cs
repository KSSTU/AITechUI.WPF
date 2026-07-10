using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AITechControls
{
    public class AITechCheckBox : CheckBox
    {
        #region 自定义依赖属性
        public static readonly DependencyProperty BoxCornerRadiusProperty =
            DependencyProperty.Register(
                nameof(BoxCornerRadius),
                typeof(CornerRadius),
                typeof(AITechCheckBox),
                new PropertyMetadata(new CornerRadius(4), OnStyleRefresh));

        public CornerRadius BoxCornerRadius
        {
            get => (CornerRadius)GetValue(BoxCornerRadiusProperty);
            set => SetValue(BoxCornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CheckGradientStartProperty =
            DependencyProperty.Register(
                nameof(CheckGradientStart),
                typeof(Color),
                typeof(AITechCheckBox),
                new PropertyMetadata(Color.FromRgb(37, 99, 235), OnStyleRefresh));

        public Color CheckGradientStart
        {
            get => (Color)GetValue(CheckGradientStartProperty);
            set => SetValue(CheckGradientStartProperty, value);
        }

        public static readonly DependencyProperty CheckGradientEndProperty =
            DependencyProperty.Register(
                nameof(CheckGradientEnd),
                typeof(Color),
                typeof(AITechCheckBox),
                new PropertyMetadata(Color.FromRgb(147, 51, 234), OnStyleRefresh));

        public Color CheckGradientEnd
        {
            get => (Color)GetValue(CheckGradientEndProperty);
            set => SetValue(CheckGradientEndProperty, value);
        }

        public static readonly DependencyProperty BorderNormalColorProperty =
            DependencyProperty.Register(
                nameof(BorderNormalColor),
                typeof(Color),
                typeof(AITechCheckBox),
                new PropertyMetadata(Color.FromRgb(180, 180, 200), OnStyleRefresh));

        public Color BorderNormalColor
        {
            get => (Color)GetValue(BorderNormalColorProperty);
            set => SetValue(BorderNormalColorProperty, value);
        }

        public static readonly DependencyProperty BoxSizeProperty =
            DependencyProperty.Register(
                nameof(BoxSize),
                typeof(double),
                typeof(AITechCheckBox),
                new PropertyMetadata(18d, OnStyleRefresh));

        public double BoxSize
        {
            get => (double)GetValue(BoxSizeProperty);
            set => SetValue(BoxSizeProperty, value);
        }

        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register(
                nameof(TextColor),
                typeof(Color),
                typeof(AITechCheckBox),
                new PropertyMetadata(Color.FromArgb(180, 255, 255, 255), OnStyleRefresh));

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static readonly DependencyProperty TextMarginProperty =
            DependencyProperty.Register(
                nameof(TextMargin),
                typeof(Thickness),
                typeof(AITechCheckBox),
                new PropertyMetadata(new Thickness(8, 0, 0, 0), OnStyleRefresh));

        public Thickness TextMargin
        {
            get => (Thickness)GetValue(TextMarginProperty);
            set => SetValue(TextMarginProperty, value);
        }

        private static void OnStyleRefresh(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AITechCheckBox ctrl)
                ctrl.Style = ctrl.BuildCheckBoxStyle();
        }
        #endregion

        static AITechCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AITechCheckBox),
                new FrameworkPropertyMetadata(typeof(AITechCheckBox)));
        }

        public AITechCheckBox()
        {
            FontSize = 15d;
            Style = BuildCheckBoxStyle();
        }

        private Style BuildCheckBoxStyle()
        {
            Style style = new Style(typeof(CheckBox));
            style.Setters.Add(new Setter(Control.ForegroundProperty, new SolidColorBrush(TextColor)));

            ControlTemplate template = new ControlTemplate(typeof(CheckBox));

            // 根布局
            FrameworkElementFactory rootStack = new FrameworkElementFactory(typeof(StackPanel));
            rootStack.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            rootStack.SetValue(StackPanel.VerticalAlignmentProperty, VerticalAlignment.Center);

            // 勾选框Border 命名
            FrameworkElementFactory boxBorder = new FrameworkElementFactory(typeof(Border));
            boxBorder.Name = "boxBorder";
            boxBorder.SetValue(Border.WidthProperty, BoxSize);
            boxBorder.SetValue(Border.HeightProperty, BoxSize);
            boxBorder.SetBinding(Border.CornerRadiusProperty, new Binding(nameof(BoxCornerRadius)) { RelativeSource = RelativeSource.TemplatedParent });
            boxBorder.SetValue(Border.BorderThicknessProperty, new Thickness(1, 1, 1, 1));
            boxBorder.SetValue(Border.BorderBrushProperty, new SolidColorBrush(BorderNormalColor));
            boxBorder.SetValue(Border.BackgroundProperty, Brushes.White);

            // 对勾Path 命名
            FrameworkElementFactory checkMark = new FrameworkElementFactory(typeof(Path));
            checkMark.Name = "checkMark";
            checkMark.SetValue(Path.StrokeProperty, Brushes.White);
            checkMark.SetValue(Path.StrokeThicknessProperty, 2.5d);
            checkMark.SetValue(Path.StrokeStartLineCapProperty, PenLineCap.Round);
            checkMark.SetValue(Path.StrokeEndLineCapProperty, PenLineCap.Round);
            checkMark.SetValue(Path.DataProperty, Geometry.Parse("M3,9 L7,13 L14,4"));
            checkMark.SetValue(Path.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            checkMark.SetValue(Path.VerticalAlignmentProperty, VerticalAlignment.Center);
            checkMark.SetValue(UIElement.VisibilityProperty, Visibility.Collapsed);
            boxBorder.AppendChild(checkMark);

            rootStack.AppendChild(boxBorder);

            // 文字
            FrameworkElementFactory textBlock = new FrameworkElementFactory(typeof(TextBlock));
            textBlock.SetBinding(TextBlock.TextProperty, new Binding(nameof(Content)) { RelativeSource = RelativeSource.TemplatedParent });
            textBlock.SetBinding(TextBlock.MarginProperty, new Binding(nameof(TextMargin)) { RelativeSource = RelativeSource.TemplatedParent });
            textBlock.SetBinding(TextBlock.ForegroundProperty, new Binding(nameof(Foreground)) { RelativeSource = RelativeSource.TemplatedParent });
            textBlock.SetBinding(TextBlock.FontSizeProperty, new Binding(nameof(FontSize)) { RelativeSource = RelativeSource.TemplatedParent });
            textBlock.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            rootStack.AppendChild(textBlock);

            template.VisualTree = rootStack;

            // 关键修复：触发器移到 Template.Triggers 内部，允许TargetName
            // 选中触发器
            Trigger checkedTrigger = new Trigger
            {
                Property = ToggleButton.IsCheckedProperty,
                Value = true
            };
            LinearGradientBrush checkGradient = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
                GradientStops =
                {
                    new GradientStop(CheckGradientStart, 0),
                    new GradientStop(CheckGradientEnd, 1)
                }
            };
            checkedTrigger.Setters.Add(new Setter(Border.BackgroundProperty, checkGradient) { TargetName = "boxBorder" });
            checkedTrigger.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Visible) { TargetName = "checkMark" });
            template.Triggers.Add(checkedTrigger);

            // 悬浮边框触发器
            Trigger hoverTrigger = new Trigger
            {
                Property = UIElement.IsMouseOverProperty,
                Value = true
            };
            hoverTrigger.Setters.Add(new Setter(Border.BorderBrushProperty, new SolidColorBrush(Color.FromRgb(140, 140, 170))) { TargetName = "boxBorder" });
            template.Triggers.Add(hoverTrigger);

            style.Setters.Add(new Setter(Control.TemplateProperty, template));
            return style;
        }
    }
}