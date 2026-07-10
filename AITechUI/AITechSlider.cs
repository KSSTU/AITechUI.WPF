using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
namespace AITechControls
{
    public class AITechSlider : Slider
    {
        #region 可自定义依赖属性
        // 圆角
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(AITechSlider),
                new PropertyMetadata(new CornerRadius(4), OnStyleRefresh));
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
                typeof(AITechSlider),
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
                typeof(AITechSlider),
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
                typeof(AITechSlider),
                new PropertyMetadata(Color.FromArgb(255,255, 255, 255), OnStyleRefresh));
        public Color TrackBackground
        {
            get => (Color)GetValue(TrackBackgroundProperty);
            set => SetValue(TrackBackgroundProperty, value);
        }
        // 滑块大小
        public static readonly DependencyProperty ThumbSizeProperty =
            DependencyProperty.Register(
                nameof(ThumbSize),
                typeof(double),
                typeof(AITechSlider),
                new PropertyMetadata(16d, OnStyleRefresh));
        public double ThumbSize
        {
            get => (double)GetValue(ThumbSizeProperty);
            set => SetValue(ThumbSizeProperty, value);
        }
        // 属性变更自动重建样式
        private static void OnStyleRefresh(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AITechSlider slider)
            {
                slider.Style = slider.BuildSliderStyle();
            }
        }
        #endregion

        private Border _trackFill;
        private Thumb _thumb;

        static AITechSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AITechSlider),
                new FrameworkPropertyMetadata(typeof(AITechSlider)));
        }

        public AITechSlider()
        {
            Style = BuildSliderStyle();
            SizeChanged += (s, e) => UpdateTrackLayout();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate(); 
            _trackBg = GetTemplateChild("PART_TrackBg") as Border;
            _trackFill = GetTemplateChild("PART_TrackFill") as Border;
            _thumb = GetTemplateChild("PART_Thumb") as Thumb;
            if (_thumb != null)
            {
                _thumb.DragDelta += OnThumbDragDelta;
            }
            UpdateTrackLayout();
        }

        private void OnThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            double delta = e.HorizontalChange / (ActualWidth - ThumbSize) * (Maximum - Minimum);
            Value = Math.Clamp(Value + delta, Minimum, Maximum);
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            UpdateTrackLayout();
        }
        private Border _trackBg;
        private void UpdateTrackLayout()
        {
            if (_trackFill == null || _thumb == null || ActualWidth <= 0) return;
            _trackBg.Width = ActualWidth;
            double percent = (Value - Minimum) / (Maximum - Minimum);
            double trackWidth = ActualWidth - ThumbSize;
            if (trackWidth <= 0) return;
            double fillWidth = trackWidth * percent + ThumbSize / 2;
            _trackFill.Width = fillWidth;
            Canvas.SetLeft(_thumb, trackWidth * percent);
        }

        private Style BuildSliderStyle()
        {
            Style style = new Style(typeof(Slider));
            style.Setters.Add(new Setter(MinHeightProperty, 24d));
            style.Setters.Add(new Setter(MinWidthProperty, 100d));
            style.Setters.Add(new Setter(BorderThicknessProperty, new Thickness(0)));
            style.Setters.Add(new Setter(CursorProperty, Cursors.Hand));

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

            ControlTemplate template = new ControlTemplate(typeof(Slider));
            FrameworkElementFactory canvas = new FrameworkElementFactory(typeof(Canvas));

            // 背景轨道
            FrameworkElementFactory trackBg = new FrameworkElementFactory(typeof(Border));
            trackBg.Name = "PART_TrackBg";
            trackBg.SetValue(Border.BackgroundProperty, new SolidColorBrush(TrackBackground));
            trackBg.SetBinding(Border.CornerRadiusProperty,
                new Binding(nameof(CornerRadius)) { RelativeSource = RelativeSource.TemplatedParent });
            trackBg.SetValue(FrameworkElement.HeightProperty, 4d);
            trackBg.SetValue(Canvas.TopProperty, 10d);
            trackBg.SetValue(FrameworkElement.WidthProperty, double.NaN);

            // 已填充轨道（渐变）
            FrameworkElementFactory trackFill = new FrameworkElementFactory(typeof(Border));
            trackFill.Name = "PART_TrackFill";
            trackFill.SetValue(Border.BackgroundProperty, gradientBrush);
            trackFill.SetBinding(Border.CornerRadiusProperty,
                new Binding(nameof(CornerRadius)) { RelativeSource = RelativeSource.TemplatedParent });
            trackFill.SetValue(FrameworkElement.HeightProperty, 4d);
            trackFill.SetValue(Canvas.TopProperty, 10d);
            trackFill.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);

            // 滑块样式
            Style thumbStyle = new Style(typeof(Thumb));
            ControlTemplate thumbTemplate = new ControlTemplate(typeof(Thumb));
            FrameworkElementFactory thumbBorder = new FrameworkElementFactory(typeof(Border));
            thumbBorder.SetValue(Border.BackgroundProperty, gradientBrush);
            thumbBorder.SetValue(Border.CornerRadiusProperty, new CornerRadius(8));
            thumbTemplate.VisualTree = thumbBorder;
            thumbStyle.Setters.Add(new Setter(Control.TemplateProperty, thumbTemplate));
            thumbStyle.Setters.Add(new Setter(FrameworkElement.WidthProperty, ThumbSize));
            thumbStyle.Setters.Add(new Setter(FrameworkElement.HeightProperty, ThumbSize));

            FrameworkElementFactory thumb = new FrameworkElementFactory(typeof(Thumb));
            thumb.Name = "PART_Thumb";
            thumb.SetValue(FrameworkElement.StyleProperty, thumbStyle);
            thumb.SetValue(Canvas.TopProperty, 4d);

            canvas.AppendChild(trackBg);
            canvas.AppendChild(trackFill);
            canvas.AppendChild(thumb);
            template.VisualTree = canvas;

            style.Setters.Add(new Setter(TemplateProperty, template));
            return style;
        }
    }
}