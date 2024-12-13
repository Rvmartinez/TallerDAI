using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using TallerDIA.Models;
using TallerDIA.Utils;


namespace TallerDIA.Views.Dialogs;

public partial class ReparacionDlg : Window
{

    
    public ReparacionDlg(Reparacion r)
    
    {
        InitializeComponent();

        List<String> _clientes = new List<String>();
        for (int i = 0; i < SharedDB.Instance.CarteraClientes.Count; i++)
        {
            _clientes.Add(SharedDB.Instance.CarteraClientes.Get(i).DNI + "_" + SharedDB.Instance.CarteraClientes.Get(i).Nombre);
        }
       
        List<String> _empleados = new List<String>();
        for (int i = 0; i < SharedDB.Instance.RegistroEmpleados.Count; i++)
        {
            _empleados.Add(SharedDB.Instance.RegistroEmpleados.Get(i).Dni + "_" + SharedDB.Instance.RegistroEmpleados.Get(i).Nombre);
        }
        
        AsuntoTb.Text = r.Asunto;
        NotaTb.Text = r.Nota;
        ClienteTb.Text = r.Cliente.DNI + "_" + r.Cliente.Nombre;
        EmpleadoTb.Text = r.Empleado.Dni + "_" + r.Empleado.Nombre;
        BtOk.Click += (_, _) => _ = this.OnAcceptClicked();
        BtCancel.Click += (_, _) => this.OnCancelClicked();
        ClienteTb.ItemsSource = _clientes;
        EmpleadoTb.ItemsSource= _empleados;
    }

   

    public ReparacionDlg()
    {
        List<String> _clientes = new List<String>();
        for (int i = 0; i < SharedDB.Instance.CarteraClientes.Count; i++)
        {
            _clientes.Add(SharedDB.Instance.CarteraClientes.Get(i).DNI + "_" + SharedDB.Instance.CarteraClientes.Get(i).Nombre);
        }
        List<String> _empleados = new List<String>();
        for (int i = 0; i < SharedDB.Instance.RegistroEmpleados.Count; i++)
        {
            _empleados.Add(SharedDB.Instance.RegistroEmpleados.Get(i).Dni + "_" + SharedDB.Instance.RegistroEmpleados.Get(i).Nombre);
        }
        InitializeComponent();
        BtOk.Click += (_, _) => _ = this.OnAcceptClicked();
        BtCancel.Click += (_, _) => this.OnCancelClicked();
        ClienteTb.ItemsSource = _clientes;;
        EmpleadoTb.ItemsSource=  _empleados;
        
        
    }

    void OnCancelClicked()
    {
        this.IsCanceled = true;
        this.OnExit();
    }

    async Task OnAcceptClicked()
    {
        
        Console.WriteLine("Aceptando reparacion, Creando nueva reparacion");
        
        IsAcepted = true;
        this.OnExit();
    }

    void OnExit()
    {
        this.Close();
    }

    public bool IsCanceled
    {
        get;
        private set;
    }
    
    public bool IsAcepted
    {
        get;
        private set;
    }
    private void OnWindowClosed()
    {
        if (IsAcepted == false)
        {
            IsCanceled = true;
        }
    }

   

   
}