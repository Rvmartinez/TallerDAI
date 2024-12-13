using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TallerDIA.ViewModels;

namespace TallerDIA.Views;

public partial class ReparacionesView : UserControl
{
    public static ReparacionesViewModel dataContext;
    public ReparacionesView()
    {
        InitializeComponent();
        dataContext = new ReparacionesViewModel();
    }

    public  void Toogle()
    {
        this.CheckTerminados = this.GetControl<CheckBox>( "CheckTerminados" );
        this.CheckNoTerminados = this.GetControl<CheckBox>( "CheckNoTerminados" );

        if (CheckTerminados.IsChecked == true)
        {
            CheckNoTerminados.IsChecked = false;
        }else if (CheckNoTerminados.IsChecked == true)
        {
            CheckTerminados.IsChecked = false;
        }
    }
}