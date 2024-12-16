using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TallerDIA.ViewModels;

namespace TallerDIA.Views;

public partial class ReparacionesView : UserControl
{
    public static ReparacionesViewModel viewModel;
    public ReparacionesView()
    {
        InitializeComponent();
        DataContext = viewModel = new ReparacionesViewModel();
    }

   
}