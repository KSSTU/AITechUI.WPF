using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
namespace AITechControls
{
    public class AITechProgressBar : ProgressBar
    {
        #region 可自定义依赖属性
        // 圆角
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(AITechProgressBar),
                new PropertyMetadata(new CornerRadius(5), OnStyleRefresh));
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        // 进度渐变起止颜色
        public static readonly DependencyProperty GradientStartProperty =
            DependencyProperty.Register(
                nameof(GradientStart),
                typeof(Color),
                typeof(AITechProgressBar),
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
                typeof(AITechProgressBar),
                new PropertyMetadata(Color.FromRgb(147, 51, 234), OnStyleRefresh));
        public Color GradientEnd
        {
            get => (Color)GetValue(GradientEndProperty);
            set => SetValue(GradientEndProperty, value);
        }
        // 轨道背景色
        public static readonly DependencyProperty TrackBackgroundProperty =
            DependencyProperty.Register(
                nameof(TrackBackground),
                typeof(Color),
                typeof(AITechProgressBar),
                new PropertyMetadata(Color.FromRgb(230, 230, 240), OnStyleRefresh));
        public Color TrackBackground
        {
            get => (Color)GetValue(TrackBackgroundProperty);
            set => SetValue(TrackBackgroundProperty, value);
        }
        // 属性变更自动重建样式
        private static void OnStyleRefresh(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AITechProgressBar bar)
            {
                bar.Style = bar.BuildProgressStyle();
            }
        }
        #endregion
        static AITechProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AITechProgressBar),
                new FrameworkPropertyMetadata(typeof(AITechProgressBar)));
        }
        public AITechProgressBar()
        {
            Style = BuildProgressStyle();
        }
        private Style BuildProgressStyle()
        {
            Style style = new Style(typeof(ProgressBar));
            style.Setters.Add(new Setter(HeightProperty, 8d));
            style.Setters.Add(new Setter(MinWidthProperty, 100d));
            style.Setters.Add(new Setter(BorderThicknessProperty, new Thickness(0)));
            // 控件模板
            ControlTemplate template = new ControlTemplate(typeof(ProgressBar));
            // 轨道背景
            FrameworkElementFactory trackBorder = new FrameworkElementFactory(typeof(Border));
            trackBorder.Name = "PART_Track";
            trackBorder.SetBinding(Border.CornerRadiusProperty,
                new Binding(nameof(CornerRadius)) { RelativeSource = RelativeSource.TemplatedParent });
            trackBorder.SetValue(Border.BackgroundProperty, new SolidColorBrush(TrackBackground));
            trackBorder.SetValue(UIElement.ClipToBoundsProperty, true);
            // 进度填充
            FrameworkElementFactory indicatorDecorator = new FrameworkElementFactory(typeof(Decorator));
            indicatorDecorator.Name = "PART_Indicator";
            indicatorDecorator.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            FrameworkElementFactory indicatorBorder = new FrameworkElementFactory(typeof(Border));
            indicatorBorder.SetBinding(Border.CornerRadiusProperty,
                new Binding(nameof(CornerRadius)) { RelativeSource = RelativeSource.TemplatedParent });
            // 渐变画刷
            var gradientBrush = new LinearGradientBrush()
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 0),
                GradientStops =
                {
                    new GradientStop(GradientStart, 0),
                    new GradientStop(GradientEnd, 1)
                }
            };
            indicatorBorder.SetValue(Border.BackgroundProperty, gradientBrush);
            indicatorDecorator.AppendChild(indicatorBorder);
            trackBorder.AppendChild(indicatorDecorator);
            template.VisualTree = trackBorder;
            style.Setters.Add(new Setter(TemplateProperty, template));
            return style;
        }
    }
}