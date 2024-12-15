using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TallerDIA.ViewModels;

namespace TallerDIA.Views;

public partial class ConfigurationView : UserControl
{
    ConfigurationViewModel viewModel;
    public ConfigurationView()
    {
        InitializeComponent();
        DataContext = viewModel = new ConfigurationViewModel();
    }
}