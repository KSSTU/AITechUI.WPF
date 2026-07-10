using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace AITechControls
{
    public class AITechListBox : ListBox
    {
        #region 自定义依赖属性
        // 整体外框圆角
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(AITechListBox),
                new PropertyMetadata(new CornerRadius(8), OnStyleRefresh));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        // 常态边框颜色
        public static readonly DependencyProperty BorderNormalColorProperty =
            DependencyProperty.Register(
                nameof(BorderNormalColor),
                typeof(Color),
                typeof(AITechListBox),
                new PropertyMetadata(Color.FromArgb(180, 255, 255, 255), OnStyleRefresh));

        public Color BorderNormalColor
        {
            get => (Color)GetValue(BorderNormalColorProperty);
            set => SetValue(BorderNormalColorProperty, value);
        }

        // 选中项渐变起始色
        public static readonly DependencyProperty SelectGradStartProperty =
            DependencyProperty.Register(
                nameof(SelectGradStart),
                typeof(Color),
                typeof(AITechListBox),
                new PropertyMetadata(Color.FromArgb(100,0, 164, 255), OnStyleRefresh));

        public Color SelectGradStart
        {
            get => (Color)GetValue(SelectGradStartProperty);
            set => SetValue(SelectGradStartProperty, value);
        }

        // 选中项渐变结束色
        public static readonly DependencyProperty SelectGradEndProperty =
            DependencyProperty.Register(
                nameof(SelectGradEnd),
                typeof(Color),
                typeof(AITechListBox),
                new PropertyMetadata(Color.FromArgb(100, 0, 80, 255), OnStyleRefresh));

        public Color SelectGradEnd
        {
            get => (Color)GetValue(SelectGradEndProperty);
            set => SetValue(SelectGradEndProperty, value);
        }

        // 控件背景
        public static readonly DependencyProperty PanelBackgroundProperty =
            DependencyProperty.Register(
                nameof(PanelBackground),
                typeof(Brush),
                typeof(AITechListBox),
                new PropertyMetadata(Brushes.White, OnStyleRefresh));

        public Brush PanelBackground
        {
            get => (Brush)GetValue(PanelBackgroundProperty);
            set => SetValue(PanelBackgroundProperty, value);
        }

        // 文字颜色
        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register(
                nameof(TextColor),
                typeof(Color),
                typeof(AITechListBox),
                new PropertyMetadata(Color.FromArgb(180, 255, 255, 255), OnStyleRefresh));

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        // 列表项内边距
        public static readonly DependencyProperty ItemPaddingProperty =
            DependencyProperty.Register(
                nameof(ItemPadding),
                typeof(Thickness),
                typeof(AITechListBox),
                new PropertyMetadata(new Thickness(12, 8, 12, 8), OnStyleRefresh));

        public Thickness ItemPadding
        {
            get => (Thickness)GetValue(ItemPaddingProperty);
            set => SetValue(ItemPaddingProperty, value);
        }

        // 列表项圆角
        public static readonly DependencyProperty ItemCornerRadiusProperty =
            DependencyProperty.Register(
                nameof(ItemCornerRadius),
                typeof(CornerRadius),
                typeof(AITechListBox),
                new PropertyMetadata(new CornerRadius(4), OnStyleRefresh));

        public CornerRadius ItemCornerRadius
        {
            get => (CornerRadius)GetValue(ItemCornerRadiusProperty);
            set => SetValue(ItemCornerRadiusProperty, value);
        }

        private static void OnStyleRefresh(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AITechListBox list)
                list.Style = list.BuildListStyle();
        }
        #endregion

        static AITechListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AITechListBox),
                new FrameworkPropertyMetadata(typeof(AITechListBox)));
        }

        public AITechListBox()
        {
            FontSize = 15d;
            Style = BuildListStyle();
        }

        private Style BuildListStyle()
        {
            Style style = new Style(typeof(ListBox));
            style.Setters.Add(new Setter(ForegroundProperty, new SolidColorBrush(TextColor)));
            style.Setters.Add(new Setter(BorderThicknessProperty, new Thickness(1, 1, 1, 1)));
            style.Setters.Add(new Setter(BorderBrushProperty, new SolidColorBrush(BorderNormalColor)));
            style.Setters.Add(new Setter(BackgroundProperty, PanelBackground));

            ControlTemplate template = new ControlTemplate(typeof(ListBox));
            FrameworkElementFactory rootBorder = new FrameworkElementFactory(typeof(Border));
            rootBorder.Name = "PART_MainBorder";
            rootBorder.SetBinding(Border.CornerRadiusProperty, new Binding(nameof(CornerRadius)) { RelativeSource = RelativeSource.Self });
            rootBorder.SetBinding(Border.BackgroundProperty, new Binding(nameof(Background)) { RelativeSource = RelativeSource.Self });
            rootBorder.SetBinding(Border.BorderBrushProperty, new Binding(nameof(BorderBrush)) { RelativeSource = RelativeSource.Self });
            rootBorder.SetBinding(Border.BorderThicknessProperty, new Binding(nameof(BorderThickness)) { RelativeSource = RelativeSource.Self });

            FrameworkElementFactory scrollViewer = new FrameworkElementFactory(typeof(ScrollViewer));
            scrollViewer.Name = "PART_ContentHost";
            scrollViewer.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
            scrollViewer.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
            scrollViewer.SetValue(ScrollViewer.PaddingProperty, new Thickness(4, 4, 4, 4));

            FrameworkElementFactory itemsPresenter = new FrameworkElementFactory(typeof(ItemsPresenter));
            scrollViewer.AppendChild(itemsPresenter);
            rootBorder.AppendChild(scrollViewer);
            template.VisualTree = rootBorder;
            style.Setters.Add(new Setter(Control.TemplateProperty, template));

            // 列表项容器样式
            Style itemStyle = new Style(typeof(ListBoxItem));
            itemStyle.Setters.Add(new Setter(ContentControl.HorizontalContentAlignmentProperty, HorizontalAlignment.Left));
            itemStyle.Setters.Add(new Setter(ContentControl.VerticalContentAlignmentProperty, VerticalAlignment.Center));

            ControlTemplate itemTemplate = new ControlTemplate(typeof(ListBoxItem));
            FrameworkElementFactory itemBorder = new FrameworkElementFactory(typeof(Border));
            itemBorder.Name = "PART_ItemBorder";
            itemBorder.SetBinding(Border.CornerRadiusProperty, new Binding(nameof(ItemCornerRadius))
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(AITechListBox), 1)
            });
            itemBorder.SetValue(Border.BackgroundProperty, Brushes.Transparent);
            itemBorder.SetValue(Border.MarginProperty, new Thickness(2, 2, 2, 2));
            itemBorder.SetBinding(Border.PaddingProperty, new Binding(nameof(ItemPadding))
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(AITechListBox), 1)
            });

            // 修复点：字符串"Content"替代nameof(Content)，消除CS0103
            FrameworkElementFactory textBlock = new FrameworkElementFactory(typeof(TextBlock));
            textBlock.SetBinding(TextBlock.TextProperty, new Binding("Content") { RelativeSource = RelativeSource.TemplatedParent });
            textBlock.SetBinding(TextBlock.ForegroundProperty, new Binding(nameof(Foreground))
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(AITechListBox), 1)
            });
            textBlock.SetBinding(TextBlock.FontSizeProperty, new Binding(nameof(FontSize))
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(AITechListBox), 1)
            });
            textBlock.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            textBlock.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            itemBorder.AppendChild(textBlock);
            itemTemplate.VisualTree = itemBorder;

            LinearGradientBrush pressGrad = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
                GradientStops =
                {
                    new GradientStop(SelectGradStart, 0),
                    new GradientStop(SelectGradEnd, 1)
                }
            };

            // 悬浮高亮（优先级最低）
            Trigger hoverTrigger = new Trigger
            {
                Property = UIElement.IsMouseOverProperty,
                Value = true
            };
            hoverTrigger.Setters.Add(new Setter(Border.BackgroundProperty, new SolidColorBrush(Color.FromArgb(10, 245, 255, 255))) { TargetName = "PART_ItemBorder" });
            itemTemplate.Triggers.Add(hoverTrigger);

            // 选中渐变触发器（优先级高于悬浮）
            Trigger selectedTrigger = new Trigger
            {
                Property = ListBoxItem.IsSelectedProperty,
                Value = true
            };
            selectedTrigger.Setters.Add(new Setter(Border.BackgroundProperty, pressGrad) { TargetName = "PART_ItemBorder" });
            selectedTrigger.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.White));
            itemTemplate.Triggers.Add(selectedTrigger);

            // 鼠标按下渐变：通过设置 ListBoxItem 的 Tag 为 "Pressed" 作为状态标记，
            // 松开时清除 Tag 的本地值，让触发器重新接管（优先级最高）
            itemStyle.Setters.Add(new Setter(ListBoxItem.TagProperty, string.Empty));

            Trigger pressedTrigger = new Trigger
            {
                Property = ListBoxItem.TagProperty,
                Value = "Pressed"
            };
            pressedTrigger.Setters.Add(new Setter(Border.BackgroundProperty, pressGrad) { TargetName = "PART_ItemBorder" });
            pressedTrigger.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.White));
            itemTemplate.Triggers.Add(pressedTrigger);

            itemStyle.Setters.Add(new EventSetter(UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler((s, e) =>
            {
                if (s is ListBoxItem item && !item.IsSelected)
                {
                    item.Tag = "Pressed";
                }
            })));

            itemStyle.Setters.Add(new EventSetter(UIElement.PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler((s, e) =>
            {
                if (s is ListBoxItem item && item.Tag is string tag && tag == "Pressed")
                {
                    item.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        item.ClearValue(ListBoxItem.TagProperty);
                    }), System.Windows.Threading.DispatcherPriority.Input);
                }
            })));

            itemStyle.Setters.Add(new EventSetter(UIElement.MouseLeaveEvent, new MouseEventHandler((s, e) =>
            {
                if (s is ListBoxItem item && item.Tag is string tag && tag == "Pressed")
                {
                    item.ClearValue(ListBoxItem.TagProperty);
                }
            })));

            itemStyle.Setters.Add(new Setter(Control.TemplateProperty, itemTemplate));
            style.Setters.Add(new Setter(ListBox.ItemContainerStyleProperty, itemStyle));

            return style;
        }
    }
}