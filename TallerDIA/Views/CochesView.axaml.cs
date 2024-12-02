using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using TallerDIA.ViewModels;

namespace TallerDIA.Views;

public partial class CochesView : UserControl
{
    CochesViewModel viewModel;
    public CochesView()
    {
        InitializeComponent();
        DataContext = viewModel = new CochesViewModel();
    }

}