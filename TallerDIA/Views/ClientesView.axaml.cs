using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TallerDIA.Utils;
using TallerDIA.ViewModels;

namespace TallerDIA.Views;

public partial class ClientesView : UserControl
{
    ClientesViewModel viewModel;
    public ClientesView()
    {
        InitializeComponent();
        
        DataContext = viewModel = new ClientesViewModel(SharedDB.Instance.Clientes);
    }
}