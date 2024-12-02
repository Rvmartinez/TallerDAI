using Avalonia.Controls;

using TallerDIA.Models;


namespace TallerDIA.Views.Dialogs;

public partial class ReparacionDlg : Window
{

    
    public ReparacionDlg(Reparacion r)
    {
        
        InitializeComponent();
        BtOk.Click += (_, _) => this.OnExit();
        BtCancel.Click += (_, _) => this.OnCancelClicked();
        //ClienteTb.ItemsSource = ClientesViewModel.Clientes;
        //EmpleadoTb.ItemsSource= EmpleadosViewModel.Empleados;
    }

   

    public ReparacionDlg()
    {
       
        InitializeComponent();
        BtOk.Click += (_, _) => this.OnExit();
        BtCancel.Click += (_, _) => this.OnCancelClicked();
        //ClienteTb.ItemsSource = ClientesViewModel.Clientes;
        //EmpleadoTb.ItemsSource= EmpleadosViewModel.Empleados;
        
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