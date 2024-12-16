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
        Title = "Editar reparación";
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
        ClienteTb.IsEnabled = false;
        EmpleadoTb.Text = r.Empleado.Dni + "_" + r.Empleado.Nombre;
        EmpleadoTb.SelectedItem = r.Empleado.Dni + "_" + r.Empleado.Nombre;
        EmpleadoTb.IsEnabled = false;
        EmpleadoTbNuevo.IsVisible = true;
        LabelNuevo.IsVisible = true;
        BtOk.Click += (_, _) => this.OnAcceptClicked();
        BtCancel.Click += (_, _) => this.OnCancelClicked();
        ClienteTb.ItemsSource = _clientes;
        EmpleadoTb.ItemsSource= _empleados;
        EmpleadoTbNuevo.ItemsSource = _empleados;
    }

   

    public ReparacionDlg()
    {
        Title = "Añadir reparación";
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
        BtOk.Click += (_, _) => this.OnAcceptClicked();
        BtCancel.Click += (_, _) => this.OnCancelClicked();
        ClienteTb.ItemsSource = _clientes;;
        EmpleadoTb.ItemsSource=  _empleados;
        
        
    }

    void OnCancelClicked()
    {
        this.IsCanceled = true;
        this.OnExit();
    }

    void OnAcceptClicked()
    {
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