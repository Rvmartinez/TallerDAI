using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using TallerDIA.Models;
using TallerDIA.Utils;
using TallerDIA.ViewModels;

namespace TallerDIA.Views;

public partial class ClientesView : UserControl
{
    ClientesViewModel viewModel;
    public ClientesView()
    {
        InitializeComponent();

        viewModel = (ClientesViewModel)DataContext;
    }

}