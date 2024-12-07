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


    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        viewModel?.ForceUpdateUI();

    }

   /* protected override void OnGotFocus(GotFocusEventArgs e)
    {
        base.OnGotFocus(e);
        ObservableCollection<Cliente> c = (ObservableCollection<Cliente>)this.dgClientes.ItemsSource;
        int x = c.Count;


        viewModel = (ClientesViewModel)DataContext;
        viewModel?.ForceUpdateUI();
    }*/
}