using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace AITechControls
{
    public class AITechEdit : TextBox
    {
        #region 自定义依赖属性
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(AITechEdit),
                new PropertyMetadata(new CornerRadius(8), OnStyleRefresh));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty BorderNormalColorProperty =
            DependencyProperty.Register(
                nameof(BorderNormalColor),
                typeof(Color),
                typeof(AITechEdit),
                new PropertyMetadata(Color.FromRgb(200, 200, 220), OnStyleRefresh));

        public Color BorderNormalColor
        {
            get => (Color)GetValue(BorderNormalColorProperty);
            set => SetValue(BorderNormalColorProperty, value);
        }

        public static readonly DependencyProperty FocusGradientStartProperty =
            DependencyProperty.Register(
                nameof(FocusGradientStart),
                typeof(Color),
                typeof(AITechEdit),
                new PropertyMetadata(Color.FromRgb(37, 99, 235), OnStyleRefresh));

        public Color FocusGradientStart
        {
            get => (Color)GetValue(FocusGradientStartProperty);
            set => SetValue(FocusGradientStartProperty, value);
        }

        public static readonly DependencyProperty FocusGradientEndProperty =
            DependencyProperty.Register(
                nameof(FocusGradientEnd),
                typeof(Color),
                typeof(AITechEdit),
                new PropertyMetadata(Color.FromRgb(147, 51, 234), OnStyleRefresh));

        public Color FocusGradientEnd
        {
            get => (Color)GetValue(FocusGradientEndProperty);
            set => SetValue(FocusGradientEndProperty, value);
        }

        public static readonly DependencyProperty EditBackgroundProperty =
            DependencyProperty.Register(
                nameof(EditBackground),
                typeof(Brush),
                typeof(AITechEdit),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 255)), OnStyleRefresh));

        public Brush EditBackground
        {
            get => (Brush)GetValue(EditBackgroundProperty);
            set => SetValue(EditBackgroundProperty, value);
        }

        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register(
                nameof(TextColor),
                typeof(Color),
                typeof(AITechEdit),
                new PropertyMetadata(Color.FromRgb(30, 30, 30), OnStyleRefresh));

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static readonly DependencyProperty InnerPaddingProperty =
            DependencyProperty.Register(
                nameof(InnerPadding),
                typeof(Thickness),
                typeof(AITechEdit),
                new PropertyMetadata(new Thickness(12, 10, 32, 10), OnStyleRefresh));

        public Thickness InnerPadding
        {
            get => (Thickness)GetValue(InnerPaddingProperty);
            set => SetValue(InnerPaddingProperty, value);
        }

        /// <summary>是否显示右侧清空×按钮</summary>
        public static readonly DependencyProperty ShowClearButtonProperty =
            DependencyProperty.Register(
                nameof(ShowClearButton),
                typeof(bool),
                typeof(AITechEdit),
                new PropertyMetadata(false, OnStyleRefresh));

        public bool ShowClearButton
        {
            get => (bool)GetValue(ShowClearButtonProperty);
            set => SetValue(ShowClearButtonProperty, value);
        }

        private static void OnStyleRefresh(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AITechEdit edit)
                edit.Style = edit.BuildEditStyle();
        }
        #endregion

        static AITechEdit()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AITechEdit),
                new FrameworkPropertyMetadata(typeof(AITechEdit)));
        }

        public AITechEdit()
        {
            FontSize = 15d;
            Cursor = Cursors.IBeam;
            Style = BuildEditStyle();
        }

        private Style BuildEditStyle()
        {
            Style style = new Style(typeof(TextBox));

            // 基础属性
            style.Setters.Add(new Setter(ForegroundProperty, new SolidColorBrush(TextColor)));
            style.Setters.Add(new Setter(PaddingProperty, InnerPadding));
            style.Setters.Add(new Setter(BackgroundProperty, EditBackground));
            style.Setters.Add(new Setter(BorderThicknessProperty, new Thickness(1, 1, 1, 1)));
            style.Setters.Add(new Setter(BorderBrushProperty, new SolidColorBrush(BorderNormalColor)));
            style.Setters.Add(new Setter(CursorProperty, Cursors.IBeam));

            ControlTemplate template = new ControlTemplate(typeof(TextBox));
            FrameworkElementFactory borderRoot = new FrameworkElementFactory(typeof(Border));
            borderRoot.SetBinding(Border.CornerRadiusProperty, new Binding(nameof(CornerRadius)) { RelativeSource = RelativeSource.TemplatedParent });
            borderRoot.SetBinding(Border.BackgroundProperty, new Binding(nameof(Background)) { RelativeSource = RelativeSource.TemplatedParent });
            borderRoot.SetBinding(Border.BorderBrushProperty, new Binding(nameof(BorderBrush)) { RelativeSource = RelativeSource.TemplatedParent });
            borderRoot.SetBinding(Border.BorderThicknessProperty, new Binding(nameof(BorderThickness)) { RelativeSource = RelativeSource.TemplatedParent });

            FrameworkElementFactory dockPanel = new FrameworkElementFactory(typeof(DockPanel));

            Style clearBtnStyle = new Style(typeof(Button));
            clearBtnStyle.Setters.Add(new Setter(Button.BackgroundProperty, Brushes.Transparent));
            clearBtnStyle.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(0)));
            clearBtnStyle.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(0)));
            clearBtnStyle.Setters.Add(new Setter(Button.CursorProperty, Cursors.Hand));
            clearBtnStyle.Setters.Add(new Setter(Button.ForegroundProperty, new SolidColorBrush(Color.FromArgb(150, 120, 120, 140))));
            clearBtnStyle.Setters.Add(new Setter(Button.MarginProperty, new Thickness(0, 0, 10, 0)));

            ControlTemplate clearBtnTemplate = new ControlTemplate(typeof(Button));
            FrameworkElementFactory clearBtnPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            clearBtnPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            clearBtnPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            clearBtnTemplate.VisualTree = clearBtnPresenter;
            clearBtnStyle.Setters.Add(new Setter(Button.TemplateProperty, clearBtnTemplate));

            Trigger clearBtnHoverTrigger = new Trigger
            {
                Property = Button.IsMouseOverProperty,
                Value = true
            };
            clearBtnHoverTrigger.Setters.Add(new Setter(Button.ForegroundProperty, new SolidColorBrush(Color.FromArgb(255, 120, 120, 140))));
            clearBtnStyle.Triggers.Add(clearBtnHoverTrigger);

            FrameworkElementFactory clearBtn = new FrameworkElementFactory(typeof(Button));
            clearBtn.SetValue(DockPanel.DockProperty, Dock.Right);
            clearBtn.SetValue(Button.WidthProperty, 26d);
            clearBtn.SetValue(Button.HeightProperty, double.NaN);
            clearBtn.SetValue(Button.StyleProperty, clearBtnStyle);
            clearBtn.SetBinding(UIElement.VisibilityProperty, new Binding(nameof(ShowClearButton))
            {
                RelativeSource = RelativeSource.TemplatedParent,
                Converter = new BoolToVisibilityConverter()
            });

            FrameworkElementFactory textX = new FrameworkElementFactory(typeof(TextBlock));
            textX.SetValue(TextBlock.TextProperty, "×");
            textX.SetValue(TextBlock.FontSizeProperty, 26d);
            textX.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            textX.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            clearBtn.AppendChild(textX);

            clearBtn.AddHandler(Button.ClickEvent, new RoutedEventHandler((s, e) =>
            {
                this.Text = string.Empty;
            }));
            dockPanel.AppendChild(clearBtn);

            // 输入宿主 PART_ContentHost
            FrameworkElementFactory scrollViewer = new FrameworkElementFactory(typeof(ScrollViewer));
            scrollViewer.Name = "PART_ContentHost";
            scrollViewer.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);
            scrollViewer.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);
            scrollViewer.SetBinding(ScrollViewer.PaddingProperty, new Binding(nameof(Padding)) { RelativeSource = RelativeSource.TemplatedParent });
            dockPanel.AppendChild(scrollViewer);

            borderRoot.AppendChild(dockPanel);
            template.VisualTree = borderRoot;
            style.Setters.Add(new Setter(TemplateProperty, template));

            // 聚焦渐变边框
            Trigger focusTrigger = new Trigger
            {
                Property = IsFocusedProperty,
                Value = true
            };
            LinearGradientBrush focusBorderGrad = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
                GradientStops =
                {
                    new GradientStop(FocusGradientStart, 0),
                    new GradientStop(FocusGradientEnd, 1)
                }
            };
            focusTrigger.Setters.Add(new Setter(BorderBrushProperty, focusBorderGrad));
            focusTrigger.Setters.Add(new Setter(BorderThicknessProperty, new Thickness(1.2, 1.2, 1.2, 1.2)));
            style.Triggers.Add(focusTrigger);

            // 输入框悬浮边框加深（仅输入框边框变化，清除按钮不受影响）
            Trigger hoverTrigger = new Trigger
            {
                Property = IsMouseOverProperty,
                Value = true
            };
            hoverTrigger.Setters.Add(new Setter(BorderBrushProperty, new SolidColorBrush(Color.FromRgb(160, 160, 190))));
            style.Triggers.Add(hoverTrigger);

            return style;
        }
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b && b)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}