using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AITechControls
{
    public class AITechPivotItem : HeaderedContentControl
    {
        public static readonly DependencyProperty IsBrandProperty =
            DependencyProperty.Register(nameof(IsBrand), typeof(bool), typeof(AITechPivotItem),
                new PropertyMetadata(false));

        public bool IsBrand
        {
            get => (bool)GetValue(IsBrandProperty);
            set => SetValue(IsBrandProperty, value);
        }

        static AITechPivotItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AITechPivotItem),
                new FrameworkPropertyMetadata(typeof(AITechPivotItem)));
        }

        public AITechPivotItem()
        {
            Background = Brushes.Transparent;
            BorderThickness = new Thickness(0);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            var parentPivot = ItemsControl.ItemsControlFromItemContainer(this) as AITechPivot;
            if (parentPivot != null)
            {
                int idx = parentPivot.ItemContainerGenerator.IndexFromContainer(this);
                if (idx >= 0)
                    parentPivot.SelectedIndex = idx;
            }
            e.Handled = true;
        }
    }

    public class AITechPivot : Selector
    {
        #region 渐变颜色
        public static readonly DependencyProperty GradientStartProperty =
            DependencyProperty.Register(nameof(GradientStart), typeof(Color), typeof(AITechPivot),
                new PropertyMetadata(Color.FromRgb(37, 99, 235), OnStyleChanged));

        public Color GradientStart
        {
            get => (Color)GetValue(GradientStartProperty);
            set => SetValue(GradientStartProperty, value);
        }

        public static readonly DependencyProperty GradientEndProperty =
            DependencyProperty.Register(nameof(GradientEnd), typeof(Color), typeof(AITechPivot),
                new PropertyMetadata(Color.FromRgb(147, 51, 234), OnStyleChanged));

        public Color GradientEnd
        {
            get => (Color)GetValue(GradientEndProperty);
            set => SetValue(GradientEndProperty, value);
        }
        #endregion

        #region Item 样式
        public static readonly DependencyProperty ItemFontSizeProperty =
            DependencyProperty.Register(nameof(ItemFontSize), typeof(double), typeof(AITechPivot),
                new PropertyMetadata(20.0, OnStyleChanged));

        public double ItemFontSize
        {
            get => (double)GetValue(ItemFontSizeProperty);
            set => SetValue(ItemFontSizeProperty, value);
        }

        public static readonly DependencyProperty SelectedFontSizeProperty =
            DependencyProperty.Register(nameof(SelectedFontSize), typeof(double), typeof(AITechPivot),
                new PropertyMetadata(25.0, OnStyleChanged));

        public double SelectedFontSize
        {
            get => (double)GetValue(SelectedFontSizeProperty);
            set => SetValue(SelectedFontSizeProperty, value);
        }

        public static readonly DependencyProperty BrandFontSizeProperty =
            DependencyProperty.Register(nameof(BrandFontSize), typeof(double), typeof(AITechPivot),
                new PropertyMetadata(22.0, OnStyleChanged));

        public double BrandFontSize
        {
            get => (double)GetValue(BrandFontSizeProperty);
            set => SetValue(BrandFontSizeProperty, value);
        }

        public static readonly DependencyProperty ItemFontWeightProperty =
            DependencyProperty.Register(nameof(ItemFontWeight), typeof(FontWeight), typeof(AITechPivot),
                new PropertyMetadata(FontWeights.Normal, OnStyleChanged));

        public FontWeight ItemFontWeight
        {
            get => (FontWeight)GetValue(ItemFontWeightProperty);
            set => SetValue(ItemFontWeightProperty, value);
        }

        public static readonly DependencyProperty BrandFontWeightProperty =
            DependencyProperty.Register(nameof(BrandFontWeight), typeof(FontWeight), typeof(AITechPivot),
                new PropertyMetadata(FontWeights.SemiBold, OnStyleChanged));

        public FontWeight BrandFontWeight
        {
            get => (FontWeight)GetValue(BrandFontWeightProperty);
            set => SetValue(BrandFontWeightProperty, value);
        }

        public static readonly DependencyProperty ItemForegroundProperty =
            DependencyProperty.Register(nameof(ItemForeground), typeof(Color), typeof(AITechPivot),
                new PropertyMetadata(Color.FromArgb(100, 255, 255, 255), OnStyleChanged));

        public Color ItemForeground
        {
            get => (Color)GetValue(ItemForegroundProperty);
            set => SetValue(ItemForegroundProperty, value);
        }

        public static readonly DependencyProperty ItemHoverForegroundProperty =
            DependencyProperty.Register(nameof(ItemHoverForeground), typeof(Color), typeof(AITechPivot),
                new PropertyMetadata(Color.FromRgb(60, 60, 80), OnStyleChanged));

        public Color ItemHoverForeground
        {
            get => (Color)GetValue(ItemHoverForegroundProperty);
            set => SetValue(ItemHoverForegroundProperty, value);
        }

        public static readonly DependencyProperty ItemPaddingProperty =
            DependencyProperty.Register(nameof(ItemPadding), typeof(Thickness), typeof(AITechPivot),
                new PropertyMetadata(new Thickness(10, 8, 10, 8), OnStyleChanged));

        public Thickness ItemPadding
        {
            get => (Thickness)GetValue(ItemPaddingProperty);
            set => SetValue(ItemPaddingProperty, value);
        }

        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.Register(nameof(ItemMargin), typeof(Thickness), typeof(AITechPivot),
                new PropertyMetadata(new Thickness(5, 0, 5, 0), OnStyleChanged));

        public Thickness ItemMargin
        {
            get => (Thickness)GetValue(ItemMarginProperty);
            set => SetValue(ItemMarginProperty, value);
        }
        #endregion

        private ContentPresenter _contentPresenter;
        private Border _contentBorder;
        private int _previousIndex = 0;

        private static void OnStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AITechPivot pivot)
                pivot.RebuildStyle();
        }

        static AITechPivot()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AITechPivot),
                new FrameworkPropertyMetadata(typeof(AITechPivot)));
        }

        public AITechPivot()
        {
            Background = Brushes.Transparent;
            BorderThickness = new Thickness(0);
            Loaded += OnLoaded;
            SelectionChanged += (s, e) => ApplySelection();
            RebuildStyle();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Items.Count > 0)
            {
                if (SelectedIndex < 0)
                    SelectedIndex = 0;
                // ↓ 加这两行：直接强制填充第一个项的内容
                var firstItem = Items[SelectedIndex] as AITechPivotItem;
                if (_contentPresenter != null)
                    _contentPresenter.Content = firstItem?.Content;
            }
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
            => item is AITechPivotItem;

        protected override DependencyObject GetContainerForItemOverride()
            => new AITechPivotItem();

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _contentPresenter = GetTemplateChild("PART_ContentPresenter") as ContentPresenter;
            _contentBorder = GetTemplateChild("PART_ContentBorder") as Border;
            ApplySelection();
        }

        private void RebuildStyle()
        {
            Style = BuildPivotStyle();
        }

        private Style BuildPivotStyle()
        {
            var style = new Style(typeof(Selector));
            style.Setters.Add(new Setter(BackgroundProperty, Brushes.Transparent));
            style.Setters.Add(new Setter(BorderThicknessProperty, new Thickness(0)));

            #region 控件模板
            var template = new ControlTemplate(typeof(Selector));

            var root = new FrameworkElementFactory(typeof(DockPanel));

            // 头部 ItemsPresenter
            var itemsPresenter = new FrameworkElementFactory(typeof(ItemsPresenter));
            itemsPresenter.SetValue(DockPanel.DockProperty, Dock.Top);
            itemsPresenter.SetValue(FrameworkElement.MarginProperty, new Thickness(0, 0, 0, 8));

            // 内容区域
            var contentBorder = new FrameworkElementFactory(typeof(Border));
            contentBorder.Name = "PART_ContentBorder";
            contentBorder.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
            contentBorder.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Stretch);
            contentBorder.SetValue(UIElement.ClipToBoundsProperty, true);

            var contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenter.Name = "PART_ContentPresenter";
            contentPresenter.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
            contentPresenter.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Stretch);

            contentBorder.AppendChild(contentPresenter);
            root.AppendChild(itemsPresenter);
            root.AppendChild(contentBorder);
            template.VisualTree = root;

            style.Setters.Add(new Setter(Control.TemplateProperty, template));
            #endregion

            #region ItemsPanel
            var stackPanel = new FrameworkElementFactory(typeof(StackPanel));
            stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            var itemsPanelTemplate = new ItemsPanelTemplate();
            itemsPanelTemplate.VisualTree = stackPanel;
            style.Setters.Add(new Setter(ItemsPanelProperty, itemsPanelTemplate));
            #endregion

            #region ItemContainerStyle
            var itemStyle = new Style(typeof(AITechPivotItem));
            itemStyle.Setters.Add(new Setter(AITechPivotItem.CursorProperty, Cursors.Hand));
            itemStyle.Setters.Add(new Setter(AITechPivotItem.BackgroundProperty, Brushes.Transparent));
            itemStyle.Setters.Add(new Setter(AITechPivotItem.BorderThicknessProperty, new Thickness(0)));
            itemStyle.Setters.Add(new Setter(AITechPivotItem.ForegroundProperty, new SolidColorBrush(ItemForeground)));
            itemStyle.Setters.Add(new Setter(AITechPivotItem.FontSizeProperty, new Binding(nameof(ItemFontSize)) { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(AITechPivot), 1) }));
            itemStyle.Setters.Add(new Setter(AITechPivotItem.FontWeightProperty, new Binding(nameof(ItemFontWeight)) { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(AITechPivot), 1) }));
            itemStyle.Setters.Add(new Setter(AITechPivotItem.PaddingProperty, new Binding(nameof(ItemPadding)) { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(AITechPivot), 1) }));
            itemStyle.Setters.Add(new Setter(AITechPivotItem.MarginProperty, new Binding(nameof(ItemMargin)) { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(AITechPivot), 1) }));

            // Item 模板: 只显示 Header
            var itemTemplate = new ControlTemplate(typeof(AITechPivotItem));

            var itemBorder = new FrameworkElementFactory(typeof(Border));
            itemBorder.SetBinding(Border.BackgroundProperty, new Binding(nameof(Background)) { RelativeSource = RelativeSource.TemplatedParent });
            itemBorder.SetBinding(Border.BorderBrushProperty, new Binding(nameof(BorderBrush)) { RelativeSource = RelativeSource.TemplatedParent });
            itemBorder.SetBinding(Border.BorderThicknessProperty, new Binding(nameof(BorderThickness)) { RelativeSource = RelativeSource.TemplatedParent });
            itemBorder.SetBinding(Border.PaddingProperty, new Binding(nameof(Padding)) { RelativeSource = RelativeSource.TemplatedParent });
            itemBorder.SetValue(UIElement.RenderTransformOriginProperty, new Point(0.5, 0.5));

            var scaleTransform = new ScaleTransform(1, 1);
            itemBorder.SetValue(UIElement.RenderTransformProperty, scaleTransform);

            var headerPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            headerPresenter.SetValue(ContentPresenter.ContentSourceProperty, "Header");
            headerPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            headerPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);

            itemBorder.AppendChild(headerPresenter);
            itemTemplate.VisualTree = itemBorder;

            // 渐变画刷
            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 0)
            };
            gradientBrush.GradientStops.Add(new GradientStop(GradientStart, 0));
            gradientBrush.GradientStops.Add(new GradientStop(GradientEnd, 1));

            // IsBrand trigger: 大字体 + 渐变
            var brandTrigger = new Trigger
            {
                Property = AITechPivotItem.IsBrandProperty,
                Value = true
            };
            brandTrigger.Setters.Add(new Setter(AITechPivotItem.FontSizeProperty,
                new Binding(nameof(BrandFontSize)) { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(AITechPivot), 1) }));
            brandTrigger.Setters.Add(new Setter(AITechPivotItem.FontWeightProperty,
                new Binding(nameof(BrandFontWeight)) { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(AITechPivot), 1) }));
            brandTrigger.Setters.Add(new Setter(AITechPivotItem.ForegroundProperty, gradientBrush));
            itemTemplate.Triggers.Add(brandTrigger);

            // IsSelected trigger: 渐变 + 放大字体
            var selectedTrigger = new Trigger
            {
                Property = Selector.IsSelectedProperty,
                Value = true
            };
            selectedTrigger.Setters.Add(new Setter(AITechPivotItem.ForegroundProperty, gradientBrush));
            selectedTrigger.Setters.Add(new Setter(AITechPivotItem.FontSizeProperty,
                new Binding(nameof(SelectedFontSize)) { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(AITechPivot), 1) }));
            itemTemplate.Triggers.Add(selectedTrigger);

            itemStyle.Setters.Add(new Setter(Control.TemplateProperty, itemTemplate));
            style.Setters.Add(new Setter(ItemContainerStyleProperty, itemStyle));
            #endregion

            return style;
        }

        private void ApplySelection()
        {
            if (_contentPresenter == null) return;

            int newIndex = SelectedIndex;
            if (newIndex < 0 || newIndex >= Items.Count) return;
            if (newIndex == _previousIndex) return;

            if (_contentPresenter.Content == null)
            {
                var firstItem = Items[newIndex] as AITechPivotItem;
                _contentPresenter.Content = firstItem?.Content;
                _previousIndex = newIndex;
                return;
            }

            bool slideRight = newIndex > _previousIndex;
            _previousIndex = newIndex;

            if (_contentBorder == null)
            {
                var item = Items[newIndex] as AITechPivotItem;
                _contentPresenter.Content = item?.Content;
                return;
            }

            // 滑动方向：向右切换 → 旧内容左滑，新内容右入
            double slideOutOffset = slideRight ? -50 : 50;
            double slideInStart = slideRight ? 50 : -50;

            var translate = new TranslateTransform(0, 0);
            _contentBorder.RenderTransform = translate;

            // 1. 滑出 + 淡出
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(120))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };
            var slideOut = new DoubleAnimation(0, slideOutOffset, TimeSpan.FromMilliseconds(180))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };

            slideOut.Completed += (s, e) =>
            {
                // 2. 切换内容
                var item = Items[newIndex] as AITechPivotItem;
                _contentPresenter.Content = item?.Content;
                translate.X = slideInStart;

                // 3. 滑入 + 淡入
                var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(120))
                {
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };
                var slideIn = new DoubleAnimation(slideInStart, 0, TimeSpan.FromMilliseconds(180))
                {
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };

                _contentBorder.BeginAnimation(UIElement.OpacityProperty, fadeIn);
                translate.BeginAnimation(TranslateTransform.XProperty, slideIn);
            };

            _contentBorder.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            translate.BeginAnimation(TranslateTransform.XProperty, slideOut);
        }
    }
}