using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TallerDIA.Models;
using TallerDIA.ViewModels;

namespace TallerDIA.Views.Dialogs;

public partial class ReparacionNavegarDlg : Window
{

    
    public ReparacionNavegarDlg(Reparacion r)
    {
        InitializeComponent();
       
        BtCancel.Click += (_, _) => this.OnCancelClicked();
        //ClienteTb.ItemsSource = ClientesViewModel.Clientes;
        //TrabajadorTb.ItemsSource= EmpleadosViewModel.Empleados;
    }

   

    public ReparacionNavegarDlg()
    {
        InitializeComponent();
        
       BtTrabajador.Click += (_, _) => this.OnBtTrabajadorClicked();
        BtCancel.Click += (_, _) => this.OnCancelClicked();
        
    }

     void OnBtTrabajadorClicked()
    {
       MainWindowViewModel mainWindow = Application.Current.ApplicationLifetime as MainWindowViewModel;
       mainWindow.GoToTrabajador();
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