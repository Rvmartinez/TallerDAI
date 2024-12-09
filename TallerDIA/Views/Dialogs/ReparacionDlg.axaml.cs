using System;
using System.Collections.Generic;
using Avalonia.Controls;

using TallerDIA.Models;
using TallerDIA.Utils;


namespace TallerDIA.Views.Dialogs;

public partial class ReparacionDlg : Window
{

    
    public ReparacionDlg(Reparacion r)
    
    {
        List<String> _clientes = new List<String>();
        for (int i = 0; i < SharedDB.Instance.CarteraClientes.Count; i++)
        {
            _clientes.Add(SharedDB.Instance.CarteraClientes.Get(i).DNI + "_" + SharedDB.Instance.CarteraClientes.Get(i).Nombre);
        }
        /*
        List<String> _empleados = new List<String>();
        for (int i = 0; i < SharedDB.Instance.CarteraClientes.Count; i++)
        {
            _clientes.Add(SharedDB.Instance.CarteraClientes.Get(i).DNI + "_" + SharedDB.Instance.CarteraClientes.Get(i).Nombre);
        }
        */
        AsuntoTb.Text = r.Asunto;
        NotaTb.Text = r.Nota;
        ClienteTb.Text = r.Cliente.DNI + "_" + r.Cliente.Nombre;
        //EmpleadoTb.Text = r.Empleado.DNI + "_" + r.Empleado.Nombre;
        InitializeComponent();
        BtOk.Click += (_, _) => this.OnExit();
        BtCancel.Click += (_, _) => this.OnCancelClicked();
        ClienteTb.ItemsSource = _clientes;
        //EmpleadoTb.ItemsSource= _empleados;
    }

   

    public ReparacionDlg()
    {
        List<String> _clientes = new List<String>();
        for (int i = 0; i < SharedDB.Instance.CarteraClientes.Count; i++)
        {
            _clientes.Add(SharedDB.Instance.CarteraClientes.Get(i).DNI + "_" + SharedDB.Instance.CarteraClientes.Get(i).Nombre);
        }
        /*
       List<String> _empleados = new List<String>();
       for (int i = 0; i < SharedDB.Instance.CarteraClientes.Count; i++)
       {
           _clientes.Add(SharedDB.Instance.CarteraClientes.Get(i).DNI + "_" + SharedDB.Instance.CarteraClientes.Get(i).Nombre);
       }
       */
        InitializeComponent();
        BtOk.Click += (_, _) => this.OnExit();
        BtCancel.Click += (_, _) => this.OnCancelClicked();
        ClienteTb.ItemsSource = _clientes;;
        //EmpleadoTb.ItemsSource= EmpleadosViewModel.Empleados;
        //EmpleadoTb.ItemsSource= _empleados;
        
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