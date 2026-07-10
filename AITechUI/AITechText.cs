using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace AITechControls
{
    public class AITechText : ContentControl
    {
        #region 可自定义依赖属性
        // 圆角
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(AITechText),
                new PropertyMetadata(new CornerRadius(8), OnStyleRefresh));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        // 文字渐变起始色
        public static readonly DependencyProperty GradientStartProperty =
            DependencyProperty.Register(
                nameof(GradientStart),
                typeof(Color),
                typeof(AITechText),
                new PropertyMetadata(Color.FromRgb(37, 99, 235), OnStyleRefresh));

        public Color GradientStart
        {
            get => (Color)GetValue(GradientStartProperty);
            set => SetValue(GradientStartProperty, value);
        }

        // 文字渐变结束色
        public static readonly DependencyProperty GradientEndProperty =
            DependencyProperty.Register(
                nameof(GradientEnd),
                typeof(Color),
                typeof(AITechText),
                new PropertyMetadata(Color.FromRgb(147, 51, 234), OnStyleRefresh));

        public Color GradientEnd
        {
            get => (Color)GetValue(GradientEndProperty);
            set => SetValue(GradientEndProperty, value);
        }

        // 背景色
        public static readonly DependencyProperty PanelBackgroundProperty =
            DependencyProperty.Register(
                nameof(PanelBackground),
                typeof(Brush),
                typeof(AITechText),
                new PropertyMetadata(Brushes.Transparent, OnStyleRefresh));

        public Brush PanelBackground
        {
            get => (Brush)GetValue(PanelBackgroundProperty);
            set => SetValue(PanelBackgroundProperty, value);
        }

        // 内边距
        public static readonly DependencyProperty PaddingProperty =
            DependencyProperty.Register(
                nameof(Padding),
                typeof(Thickness),
                typeof(AITechText),
                new PropertyMetadata(new Thickness(8, 6, 8, 6), OnStyleRefresh));

        public Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        // 字号
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register(
                nameof(FontSize),
                typeof(double),
                typeof(AITechText),
                new PropertyMetadata(15d, OnStyleRefresh));

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        // 字重
        public static readonly DependencyProperty FontWeightProperty =
            DependencyProperty.Register(
                nameof(FontWeight),
                typeof(FontWeight),
                typeof(AITechText),
                new PropertyMetadata(FontWeights.Normal, OnStyleRefresh));

        public FontWeight FontWeight
        {
            get => (FontWeight)GetValue(FontWeightProperty);
            set => SetValue(FontWeightProperty, value);
        }

        // 水平对齐
        public static readonly DependencyProperty HorizontalContentAlignProperty =
            DependencyProperty.Register(
                nameof(HorizontalContentAlign),
                typeof(HorizontalAlignment),
                typeof(AITechText),
                new PropertyMetadata(HorizontalAlignment.Left, OnStyleRefresh));

        public HorizontalAlignment HorizontalContentAlign
        {
            get => (HorizontalAlignment)GetValue(HorizontalContentAlignProperty);
            set => SetValue(HorizontalContentAlignProperty, value);
        }

        // 垂直对齐
        public static readonly DependencyProperty VerticalContentAlignProperty =
            DependencyProperty.Register(
                nameof(VerticalContentAlign),
                typeof(VerticalAlignment),
                typeof(AITechText),
                new PropertyMetadata(VerticalAlignment.Center, OnStyleRefresh));

        public VerticalAlignment VerticalContentAlign
        {
            get => (VerticalAlignment)GetValue(VerticalContentAlignProperty);
            set => SetValue(VerticalContentAlignProperty, value);
        }

        private static void OnStyleRefresh(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AITechText textCtrl)
                textCtrl.Style = textCtrl.BuildTextStyle();
        }
        #endregion

        static AITechText()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AITechText),
                new FrameworkPropertyMetadata(typeof(AITechText)));
        }

        public AITechText()
        {
            Style = BuildTextStyle();
        }

        private Style BuildTextStyle()
        {
            Style style = new Style(typeof(ContentControl));

            // 移除错误的 FrameworkElement 字体Setter，模板内TextBlock绑定字体
            style.Setters.Add(new Setter(ContentControl.PaddingProperty, Padding));
            style.Setters.Add(new Setter(ContentControl.HorizontalContentAlignmentProperty, HorizontalContentAlign));
            style.Setters.Add(new Setter(ContentControl.VerticalContentAlignmentProperty, VerticalContentAlign));

            ControlTemplate template = new ControlTemplate(typeof(ContentControl));
            FrameworkElementFactory borderRoot = new FrameworkElementFactory(typeof(Border));
            borderRoot.SetBinding(Border.CornerRadiusProperty, new Binding(nameof(CornerRadius)) { RelativeSource = RelativeSource.TemplatedParent });
            borderRoot.SetBinding(Border.BackgroundProperty, new Binding(nameof(PanelBackground)) { RelativeSource = RelativeSource.TemplatedParent });

            // 渐变文字容器
            FrameworkElementFactory textBlock = new FrameworkElementFactory(typeof(TextBlock));
            textBlock.SetBinding(TextBlock.TextProperty, new Binding(nameof(Content)) { RelativeSource = RelativeSource.TemplatedParent });
            // 正确绑定当前控件自定义字体属性
            textBlock.SetBinding(TextBlock.FontSizeProperty, new Binding(nameof(FontSize)) { RelativeSource = RelativeSource.TemplatedParent });
            textBlock.SetBinding(TextBlock.FontWeightProperty, new Binding(nameof(FontWeight)) { RelativeSource = RelativeSource.TemplatedParent });
            textBlock.SetBinding(TextBlock.HorizontalAlignmentProperty, new Binding(nameof(HorizontalContentAlign)) { RelativeSource = RelativeSource.TemplatedParent });
            textBlock.SetBinding(TextBlock.VerticalAlignmentProperty, new Binding(nameof(VerticalContentAlign)) { RelativeSource = RelativeSource.TemplatedParent });

            // 文字渐变画笔（和AITechButton/AITechEdit统一对角渐变）
            LinearGradientBrush textGradient = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
                GradientStops =
                {
                    new GradientStop(GradientStart, 0),
                    new GradientStop(GradientEnd, 1)
                }
            };
            textBlock.SetValue(TextBlock.ForegroundProperty, textGradient);

            borderRoot.AppendChild(textBlock);
            template.VisualTree = borderRoot;
            style.Setters.Add(new Setter(Control.TemplateProperty, template));

            return style;
        }
    }
}