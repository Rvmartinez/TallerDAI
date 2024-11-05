using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TallerDAI.ViewModels;

namespace TallerDAI.Views;

public partial class ClientesView : UserControl
{
    ClientesViewModel viewModel;
    public ClientesView()
    {
        InitializeComponent();
        DataContext = viewModel = new ClientesViewModel();
    }
}