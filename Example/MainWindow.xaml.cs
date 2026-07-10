using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AITechExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<MyCardItem> Items { get; set; } = new();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadItems();
        }

        private void AITechListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is MyCardItem item)
            {
                MessageBox.Show($"点击了: {item.Title}\n描述: {item.Description}", "AITechListView 点击测试", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AITechGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is MyCardItem item)
            {
                MessageBox.Show($"点击了: {item.Title}\n描述: {item.Description}", "AITechGridView 点击测试", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void LoadItems()
        {
            Items.Add(new MyCardItem
            {
                Title = "AITechButton",
                Description = "支持渐变背景、圆角、悬停动画的现代化按钮控件",
                IconColor = "#2563EB",
                TagText = "按钮组件"
            });
            Items.Add(new MyCardItem
            {
                Title = "AITechEdit",
                Description = "带清除按钮、聚焦渐变边框的自定义输入框控件",
                IconColor = "#9333EA",
                TagText = "输入组件"
            });
            Items.Add(new MyCardItem
            {
                Title = "AITechText",
                Description = "支持渐变色、圆角背景的富文本显示控件",
                IconColor = "#FF3366",
                TagText = "文本组件"
            });
            Items.Add(new MyCardItem
            {
                Title = "AITechCheckBox",
                Description = "自定义渐变勾选框，支持多种状态的动画效果",
                IconColor = "#10B981",
                TagText = "选择组件"
            });
            Items.Add(new MyCardItem
            {
                Title = "AITechComboBox",
                Description = "现代化下拉框，支持圆角边框和渐变高亮",
                IconColor = "#F59E0B",
                TagText = "选择组件"
            });
            Items.Add(new MyCardItem
            {
                Title = "AITechListBox",
                Description = "自定义样式列表框，带选中渐变动画效果",
                IconColor = "#6366F1",
                TagText = "列表组件"
            });
            Items.Add(new MyCardItem
            {
                Title = "AITechListView",
                Description = "卡片式列表视图，支持悬停边框动画和自定义模板",
                IconColor = "#EC4899",
                TagText = "视图组件"
            });
            Items.Add(new MyCardItem
            {
                Title = "More Controls",
                Description = "更多AITech自定义控件正在开发中，敬请期待...",
                IconColor = "#8B5CF6",
                TagText = "即将推出"
            });
        }

        private void AITechButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show((sender as AITechControls.AITechButton).Content.ToString());
        }
    }

    public class MyCardItem : INotifyPropertyChanged
    {
        private string _title;
        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        private string _iconColor;
        public string IconColor
        {
            get => _iconColor;
            set { _iconColor = value; OnPropertyChanged(); }
        }

        private string _tagText;
        public string TagText
        {
            get => _tagText;
            set { _tagText = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}