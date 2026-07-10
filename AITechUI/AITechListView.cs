using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AITechControls
{
    public class AITechListView : ListBox
    {
        #region Item卡片外观属性
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register(
                nameof(ItemWidth),
                typeof(double),
                typeof(AITechListView),
                new PropertyMetadata(double.NaN, OnStyleUpdate));

        public double ItemWidth
        {
            get => (double)GetValue(ItemWidthProperty);
            set => SetValue(ItemWidthProperty, value);
        }

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register(
                nameof(ItemHeight),
                typeof(double),
                typeof(AITechListView),
                new PropertyMetadata(double.NaN, OnStyleUpdate));

        public double ItemHeight
        {
            get => (double)GetValue(ItemHeightProperty);
            set => SetValue(ItemHeightProperty, value);
        }

        public static readonly DependencyProperty ItemCornerRadiusProperty =
            DependencyProperty.Register(
                nameof(ItemCornerRadius),
                typeof(CornerRadius),
                typeof(AITechListView),
                new PropertyMetadata(new CornerRadius(12), OnStyleUpdate));

        public CornerRadius ItemCornerRadius
        {
            get => (CornerRadius)GetValue(ItemCornerRadiusProperty);
            set => SetValue(ItemCornerRadiusProperty, value);
        }

        public static readonly DependencyProperty ItemBackgroundProperty =
            DependencyProperty.Register(
                nameof(ItemBackground),
                typeof(Brush),
                typeof(AITechListView),
                new PropertyMetadata(new SolidColorBrush(Color.FromArgb(10, 255, 255, 255)), OnStyleUpdate));

        public Brush ItemBackground
        {
            get => (Brush)GetValue(ItemBackgroundProperty);
            set => SetValue(ItemBackgroundProperty, value);
        }

        public static readonly DependencyProperty ItemBorderBrushProperty =
            DependencyProperty.Register(
                nameof(ItemBorderBrush),
                typeof(Color),
                typeof(AITechListView),
                new PropertyMetadata(Color.FromArgb(60, 120, 140, 200), OnStyleUpdate));

        public Color ItemBorderBrush
        {
            get => (Color)GetValue(ItemBorderBrushProperty);
            set => SetValue(ItemBorderBrushProperty, value);
        }

        public static readonly DependencyProperty ItemHoverBorderBrushProperty =
            DependencyProperty.Register(
                nameof(ItemHoverBorderBrush),
                typeof(Color),
                typeof(AITechListView),
                new PropertyMetadata(Color.FromRgb(60, 220, 255), OnStyleUpdate));

        public Color ItemHoverBorderBrush
        {
            get => (Color)GetValue(ItemHoverBorderBrushProperty);
            set => SetValue(ItemHoverBorderBrushProperty, value);
        }

        public static readonly DependencyProperty ItemBorderThicknessProperty =
            DependencyProperty.Register(
                nameof(ItemBorderThickness),
                typeof(Thickness),
                typeof(AITechListView),
                new PropertyMetadata(new Thickness(1), OnStyleUpdate));

        public Thickness ItemBorderThickness
        {
            get => (Thickness)GetValue(ItemBorderThicknessProperty);
            set => SetValue(ItemBorderThicknessProperty, value);
        }

        public static readonly DependencyProperty ItemPaddingProperty =
            DependencyProperty.Register(
                nameof(ItemPadding),
                typeof(Thickness),
                typeof(AITechListView),
                new PropertyMetadata(new Thickness(20), OnStyleUpdate));

        public Thickness ItemPadding
        {
            get => (Thickness)GetValue(ItemPaddingProperty);
            set => SetValue(ItemPaddingProperty, value);
        }

        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.Register(
                nameof(ItemMargin),
                typeof(Thickness),
                typeof(AITechListView),
                new PropertyMetadata(new Thickness(10), OnStyleUpdate));

        public Thickness ItemMargin
        {
            get => (Thickness)GetValue(ItemMarginProperty);
            set => SetValue(ItemMarginProperty, value);
        }

        public static readonly DependencyProperty HoverAnimationDurationProperty =
            DependencyProperty.Register(
                nameof(HoverAnimationDuration),
                typeof(Duration),
                typeof(AITechListView),
                new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(200)), OnStyleUpdate));

        public Duration HoverAnimationDuration
        {
            get => (Duration)GetValue(HoverAnimationDurationProperty);
            set => SetValue(HoverAnimationDurationProperty, value);
        }
        #endregion

        private static void OnStyleUpdate(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AITechListView list)
                list.Style = list.BuildTechCardStyle();
        }

        static AITechListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AITechListView),
                new FrameworkPropertyMetadata(typeof(AITechListView)));
        }

        public AITechListView()
        {
            FontSize = 15;
            Style = BuildTechCardStyle();
        }

        private Style BuildTechCardStyle()
        {
            Style listStyle = new Style(typeof(ListBox));
            listStyle.Setters.Add(new Setter(BackgroundProperty, Brushes.Transparent));

            ControlTemplate template = new ControlTemplate(typeof(ListBox));
            FrameworkElementFactory scrollHost = new FrameworkElementFactory(typeof(ScrollViewer));
            scrollHost.Name = "PART_ContentHost";
            scrollHost.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
            scrollHost.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
            scrollHost.SetValue(ScrollViewer.BackgroundProperty, Brushes.Transparent);

            FrameworkElementFactory itemsPresenter = new FrameworkElementFactory(typeof(ItemsPresenter));
            scrollHost.AppendChild(itemsPresenter);
            template.VisualTree = scrollHost;

            listStyle.Setters.Add(new Setter(Control.TemplateProperty, template));

            #region Item 样式
            Style itemStyle = new Style(typeof(ListBoxItem));
            itemStyle.Setters.Add(new Setter(ListBoxItem.BackgroundProperty, Brushes.Transparent));
            itemStyle.Setters.Add(new Setter(ListBoxItem.BorderThicknessProperty, new Thickness(0)));
            itemStyle.Setters.Add(new Setter(ListBoxItem.PaddingProperty, new Thickness(0)));
            itemStyle.Setters.Add(new Setter(ListBoxItem.MarginProperty, new Binding(nameof(ItemMargin)) { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(AITechListView), 1) }));
            itemStyle.Setters.Add(new Setter(ListBoxItem.HorizontalContentAlignmentProperty, HorizontalAlignment.Stretch));
            itemStyle.Setters.Add(new Setter(ListBoxItem.VerticalContentAlignmentProperty, VerticalAlignment.Stretch));

            if (!double.IsNaN(ItemWidth))
                itemStyle.Setters.Add(new Setter(FrameworkElement.WidthProperty, ItemWidth));
            if (!double.IsNaN(ItemHeight))
                itemStyle.Setters.Add(new Setter(FrameworkElement.HeightProperty, ItemHeight));

            ControlTemplate itemTemplate = new ControlTemplate(typeof(ListBoxItem));

            FrameworkElementFactory itemBorder = new FrameworkElementFactory(typeof(Border));
            itemBorder.Name = "PART_ItemBorder";
            itemBorder.SetBinding(Border.CornerRadiusProperty, new Binding(nameof(ItemCornerRadius)) { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(AITechListView), 1) });
            itemBorder.SetBinding(Border.BackgroundProperty, new Binding(nameof(ItemBackground)) { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(AITechListView), 1) });
            itemBorder.SetValue(Border.BorderBrushProperty, new SolidColorBrush(ItemBorderBrush));
            itemBorder.SetBinding(Border.BorderThicknessProperty, new Binding(nameof(ItemBorderThickness)) { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(AITechListView), 1) });
            itemBorder.SetBinding(Border.PaddingProperty, new Binding(nameof(ItemPadding)) { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(AITechListView), 1) });
            itemBorder.SetValue(UIElement.RenderTransformOriginProperty, new Point(0.5, 0.5));

            ScaleTransform scaleTransform = new ScaleTransform(1, 1);
            itemBorder.SetValue(UIElement.RenderTransformProperty, scaleTransform);

            FrameworkElementFactory contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
            contentPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Stretch);
            itemBorder.AppendChild(contentPresenter);

            itemTemplate.VisualTree = itemBorder;

            ColorAnimation hoverAnim = new ColorAnimation
            {
                To = ItemHoverBorderBrush,
                Duration = HoverAnimationDuration
            };

            DoubleAnimation scaleXHoverAnim = new DoubleAnimation
            {
                To = 1.01,
                Duration = HoverAnimationDuration,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            DoubleAnimation scaleYHoverAnim = new DoubleAnimation
            {
                To = 1.01,
                Duration = HoverAnimationDuration,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            Storyboard hoverStoryboard = new Storyboard();
            hoverStoryboard.Children.Add(hoverAnim);
            hoverStoryboard.Children.Add(scaleXHoverAnim);
            hoverStoryboard.Children.Add(scaleYHoverAnim);
            Storyboard.SetTargetName(hoverAnim, "PART_ItemBorder");
            Storyboard.SetTargetProperty(hoverAnim, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));
            Storyboard.SetTargetName(scaleXHoverAnim, "PART_ItemBorder");
            Storyboard.SetTargetProperty(scaleXHoverAnim, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
            Storyboard.SetTargetName(scaleYHoverAnim, "PART_ItemBorder");
            Storyboard.SetTargetProperty(scaleYHoverAnim, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));

            ColorAnimation leaveAnim = new ColorAnimation
            {
                To = ItemBorderBrush,
                Duration = HoverAnimationDuration
            };

            DoubleAnimation scaleXLeaveAnim = new DoubleAnimation
            {
                To = 1.0,
                Duration = HoverAnimationDuration,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            DoubleAnimation scaleYLeaveAnim = new DoubleAnimation
            {
                To = 1.0,
                Duration = HoverAnimationDuration,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            Storyboard leaveStoryboard = new Storyboard();
            leaveStoryboard.Children.Add(leaveAnim);
            leaveStoryboard.Children.Add(scaleXLeaveAnim);
            leaveStoryboard.Children.Add(scaleYLeaveAnim);
            Storyboard.SetTargetName(leaveAnim, "PART_ItemBorder");
            Storyboard.SetTargetProperty(leaveAnim, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));
            Storyboard.SetTargetName(scaleXLeaveAnim, "PART_ItemBorder");
            Storyboard.SetTargetProperty(scaleXLeaveAnim, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
            Storyboard.SetTargetName(scaleYLeaveAnim, "PART_ItemBorder");
            Storyboard.SetTargetProperty(scaleYLeaveAnim, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));

            EventTrigger mouseEnterTrigger = new EventTrigger(UIElement.MouseEnterEvent);
            mouseEnterTrigger.Actions.Add(new BeginStoryboard { Storyboard = hoverStoryboard });
            itemTemplate.Triggers.Add(mouseEnterTrigger);

            EventTrigger mouseLeaveTrigger = new EventTrigger(UIElement.MouseLeaveEvent);
            mouseLeaveTrigger.Actions.Add(new BeginStoryboard { Storyboard = leaveStoryboard });
            itemTemplate.Triggers.Add(mouseLeaveTrigger);

            itemStyle.Setters.Add(new Setter(Control.TemplateProperty, itemTemplate));
            listStyle.Setters.Add(new Setter(ListBox.ItemContainerStyleProperty, itemStyle));
            #endregion

            return listStyle;
        }
    }
}
