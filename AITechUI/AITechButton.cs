using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace AITechControls
{
    public class AITechButton : Button
    {
        #region 可自定义依赖属性
        // 圆角
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(AITechButton),
                new PropertyMetadata(new CornerRadius(8), OnStyleRefresh));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        // 常态渐变起止颜色
        public static readonly DependencyProperty GradientStartProperty =
            DependencyProperty.Register(
                nameof(GradientStart),
                typeof(Color),
                typeof(AITechButton),
                new PropertyMetadata(Color.FromRgb(37, 99, 235), OnStyleRefresh));

        public Color GradientStart
        {
            get => (Color)GetValue(GradientStartProperty);
            set => SetValue(GradientStartProperty, value);
        }

        public static readonly DependencyProperty GradientEndProperty =
            DependencyProperty.Register(
                nameof(GradientEnd),
                typeof(Color),
                typeof(AITechButton),
                new PropertyMetadata(Color.FromRgb(147, 51, 234), OnStyleRefresh));

        public Color GradientEnd
        {
            get => (Color)GetValue(GradientEndProperty);
            set => SetValue(GradientEndProperty, value);
        }

        // 悬浮渐变起止颜色
        public static readonly DependencyProperty HoverGradientStartProperty =
            DependencyProperty.Register(
                nameof(HoverGradientStart),
                typeof(Color),
                typeof(AITechButton),
                new PropertyMetadata(Color.FromRgb(29, 78, 216), OnStyleRefresh));

        public Color HoverGradientStart
        {
            get => (Color)GetValue(HoverGradientStartProperty);
            set => SetValue(HoverGradientStartProperty, value);
        }

        public static readonly DependencyProperty HoverGradientEndProperty =
            DependencyProperty.Register(
                nameof(HoverGradientEnd),
                typeof(Color),
                typeof(AITechButton),
                new PropertyMetadata(Color.FromRgb(126, 34, 206), OnStyleRefresh));

        public Color HoverGradientEnd
        {
            get => (Color)GetValue(HoverGradientEndProperty);
            set => SetValue(HoverGradientEndProperty, value);
        }

        // 属性变更自动重建样式
        private static void OnStyleRefresh(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AITechButton btn)
            {
                btn.Style = btn.BuildPrimaryStyle();
            }
        }
        #endregion

        static AITechButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AITechButton),
                new FrameworkPropertyMetadata(typeof(AITechButton)));
        }

        public AITechButton()
        {
            Style = BuildPrimaryStyle();
        }

        private Style BuildPrimaryStyle()
        {
            Style style = new Style(typeof(Button));
            style.Setters.Add(new Setter(ForegroundProperty, Brushes.White));
            style.Setters.Add(new Setter(PaddingProperty, new Thickness(24, 14, 24, 14)));
            style.Setters.Add(new Setter(BorderThicknessProperty, new Thickness(0)));
            style.Setters.Add(new Setter(FontSizeProperty, 15d));
            style.Setters.Add(new Setter(FontWeightProperty, FontWeights.SemiBold));
            style.Setters.Add(new Setter(CursorProperty, Cursors.Hand));

            // 正常模板，绑定自身依赖属性
            ControlTemplate normalTemplate = new ControlTemplate(typeof(Button));
            FrameworkElementFactory borderNormal = new FrameworkElementFactory(typeof(Border));
            borderNormal.SetBinding(Border.CornerRadiusProperty, new Binding(nameof(CornerRadius)) { RelativeSource = RelativeSource.TemplatedParent });

            FrameworkElementFactory cpNormal = new FrameworkElementFactory(typeof(ContentPresenter));
            cpNormal.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            cpNormal.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            borderNormal.AppendChild(cpNormal);
            normalTemplate.VisualTree = borderNormal;

            // 动态生成渐变，取当前实例颜色
            normalTemplate.Resources.Add("NormalGrad", new LinearGradientBrush()
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
                GradientStops =
                {
                    new GradientStop(GradientStart, 0),
                    new GradientStop(GradientEnd, 1)
                }
            });
            borderNormal.SetResourceReference(Border.BackgroundProperty, "NormalGrad");
            style.Setters.Add(new Setter(TemplateProperty, normalTemplate));

            // 悬浮触发器
            Trigger hoverTrigger = new Trigger
            {
                Property = IsMouseOverProperty,
                Value = true
            };
            ControlTemplate hoverTemplate = new ControlTemplate(typeof(Button));
            FrameworkElementFactory borderHover = new FrameworkElementFactory(typeof(Border));
            borderHover.SetBinding(Border.CornerRadiusProperty, new Binding(nameof(CornerRadius)) { RelativeSource = RelativeSource.TemplatedParent });

            FrameworkElementFactory cpHover = new FrameworkElementFactory(typeof(ContentPresenter));
            cpHover.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            cpHover.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            borderHover.AppendChild(cpHover);
            hoverTemplate.VisualTree = borderHover;

            hoverTemplate.Resources.Add("HoverGrad", new LinearGradientBrush()
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
                GradientStops =
                {
                    new GradientStop(HoverGradientStart, 0),
                    new GradientStop(HoverGradientEnd, 1)
                }
            });
            borderHover.SetResourceReference(Border.BackgroundProperty, "HoverGrad");
            hoverTrigger.Setters.Add(new Setter(TemplateProperty, hoverTemplate));
            style.Triggers.Add(hoverTrigger);

            return style;
        }
    }

  
}