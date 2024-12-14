
using Avalonia.Controls;

using TallerDIA.Models;


namespace TallerDIA.Views.Dialogs;

public partial class ReparacionNavegarDlg : Window
{


    

   

    public ReparacionNavegarDlg()
    {
        InitializeComponent();
        
       BtEmpleado.Click += (_, _) => this.OnBtEmpleadoClicked();
        BtCancel.Click += (_, _) => this.OnCancelClicked();
        BtCliente.Click += (_, _) => this.OnBtClienteClicked();
        
    }

     void OnBtEmpleadoClicked()
     {
         this.VerEmpleado = true;
         this.OnExit();
     }
     void OnBtClienteClicked()
     {
         this.VerCliente = true;
         this.OnExit();
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
    public bool VerEmpleado
    {
        get;
        private set;
    }
    public bool VerCliente
    {
        get;
        private set;
    }

   

   
}