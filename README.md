# AITechUI.WPF
A WPF custom control library featuring a blue-purple gradient design style. Built on .NET 10, all control styles and templates are constructed dynamically in code with no XAML resource dictionary references required.

## Project Info
| Item | Value |
|---|---|
| NuGet Package | `AITechUI.WPF` |
| Namespace | `AITechControls` |
| Target Framework | `net10.0-windows` |
| Author | KS.STUDIO |
| WPF Support | Enabled (`UseWPF=true`) |

## Design Features
- **Unified Brand Color**: Blue-purple gradient `RGB(37,99,235) → RGB(147,51,234)` applied consistently across all controls
- **Code-Only Styling**: Uses `FrameworkElementFactory` + `Binding` + `Trigger` to dynamically generate `Style` and `ControlTemplate` in C# code, with zero XAML resource dictionary dependencies
- **Property-Driven Design**: All visual parameters (corner radius, gradient colors, borders, shadows, etc.) are exposed as dependency properties — changes trigger an immediate style rebuild
- **Hover & Focus Animations**: Built-in effects including card scaling, border gradient transitions, and content slide animations

## Control Catalog
| Control | Class Name | Base Class | Description |
|---|---|---|---|
| Button | `AITechButton` | `Button` | Gradient background button with normal/hover template switching, customizable corner radius and gradient colors |
| Text | `AITechText` | `ContentControl` | Gradient text control with diagonal gradient foreground |
| TextBox | `AITechEdit` | `TextBox` | Focus-state gradient border with optional clear button on the right |
| CheckBox | `AITechCheckBox` | `CheckBox` | Rounded checkbox with gradient fill and checkmark on selection |
| ComboBox | `AITechComboBox` | `ComboBox` | Rounded body, editable mode, fade-in dropdown animation |
| Slider | `AITechSlider` | `Slider` | Custom drag logic, gradient-filled track with rounded thumb |
| ProgressBar | `AITechProgressBar` | `ProgressBar` | Thin rounded bar with gradient fill |
| ListBox | `AITechListBox` | `ListBox` | Full border container with hover/selected/pressed item states |
| List View | `AITechListView` | `ListBox` | Card-style layout with 1.01x hover scale and border gradient |
| Grid View | `AITechGridView` | `ListBox` | `UniformGrid` auto-column layout with 1.03x hover scale |
| Panel | `AITechPanel` | `ContentControl` | Rounded container with drop shadow and hover border animation |
| Pivot | `AITechPivot` | `Selector` | Header + content area with slide + fade transition animations, supports brand items |

## Installation

### Via NuGet
```powershell
dotnet add package AITechUI.WPF
```

## Usage Examples
### App.xaml references fluent theme (strongly recommended)
```xml
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="pack://application:,,,/PresentationFramework.Fluent;component/Themes/Fluent.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
```

### XAML Namespace Import
```xml
<Window x:Class="Demo.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:ai="clr-namespace:AITechControls;assembly=AITechControls"
        Background="#FF0E0F2D">
    <StackPanel Margin="20" Width="320">
        <!-- Gradient Button -->
        <ai:AITechButton Content="Click Me" Height="40" Margin="0,0,0,12" />
        <!-- Custom Color Button -->
        <ai:AITechButton Content="Green Button"
                         GradientStart="#16A34A" GradientEnd="#22D3EE"
                         HoverGradientStart="#15803D" HoverGradientEnd="#06B6D4"
                         Height="40" Margin="0,0,0,12" />
        <!-- Gradient Text -->
        <ai:AITechText Content="Gradient Title" FontSize="24" FontWeight="Bold"
                       HorizontalContentAlign="Center" Margin="0,0,0,12" />
        <!-- TextBox with Clear Button -->
        <ai:AITechEdit ShowClearButton="True" Height="40" Margin="0,0,0,12" />
        <!-- Progress Bar -->
        <ai:AITechProgressBar Value="60" Height="8" Margin="0,0,0,12" />
        <!-- Slider -->
        <ai:AITechSlider Value="60" Height="8" Margin="0,0,0,12" />
        <!-- Pivot Tabs -->
        <ai:AITechPivot Height="320">
            <ai:AITechPivotItem Header="Home">
                <TextBlock Text="Home Content" HorizontalAlignment="Center" VerticalAlignment="Center" />
            </ai:AITechPivotItem>
            <ai:AITechPivotItem Header="Settings">
                <TextBlock Text="Settings Content" HorizontalAlignment="Center" VerticalAlignment="Center" />
            </ai:AITechPivotItem>
        </ai:AITechPivot>
    </StackPanel>
</Window>
```

### C# Code Usage
```csharp
using AITechControls;

var button = new AITechButton
{
    Content = "Button Created in Code",
    Height = 40,
    CornerRadius = new CornerRadius(12),
    GradientStart = Color.FromRgb(37, 99, 235),
    GradientEnd = Color.FromRgb(147, 51, 234)
};
```

## Project Structure
```
UI4Controls/
├── AITechControls.csproj     # Project file
├── AssemblyInfo.cs           # Assembly info (WPF theme config)
├── AITechButton.cs           # Button
├── AITechText.cs             # Gradient text
├── AITech4Edit.cs            # TextBox
├── AITechCheckBox.cs         # CheckBox
├── AITechComboBox.cs         # ComboBox
├── AITechSlider.cs           # Slider
├── AITechProgressBar.cs      # ProgressBar
├── AITechListBox.cs          # ListBox
├── AITechListView.cs         # Card list view
├── AITechGridView.cs         # Grid view
├── AITechPanel.cs            # Panel container
└── AITechPivot.cs            # Pivot tabs
```

## Dependencies
- .NET 10.0 SDK
- Windows OS (WPF)
