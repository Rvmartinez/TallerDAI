using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TallerDIA.Models;
using TallerDIA.ViewModels;

namespace TallerDIA.Views.Dialogs;

public partial class ReparacionDlg : Window
{

    
    public ReparacionDlg(Reparacion r)
    {
        InitializeComponent();
        BtOk.Click += (_, _) => this.OnExit();
        BtCancel.Click += (_, _) => this.OnCancelClicked();
        //ClienteTb.ItemsSource = ClientesViewModel.Clientes;
        //TrabajadorTb.ItemsSource= EmpleadosViewModel.Empleados;
    }

   

    public ReparacionDlg()
    {
        InitializeComponent();
        
        BtOk.Click += (_, _) => this.OnExit();
        BtCancel.Click += (_, _) => this.OnCancelClicked();
        
    }

    void OnCancelClicked()
    {
        this.IsCancelled = true;
        this.OnExit();
    }

    void OnExit()
    {
        this.Close();
    }

    public bool IsCancelled
    {
        get;
        private set;
    }

   

   
}