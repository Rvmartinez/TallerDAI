using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using TallerDIA.Models;


namespace TallerDIA.Views.Dialogs;

public partial class ReparacionNavegarDlg : Window
{

    public ReparacionNavegarDlg(Reparacion r)
    {
        InitializeComponent();
        
        BtEmpleado.Click += async (_, _) => await this.OnBtEmpleadoClicked(r);
        BtCancel.Click += (_, _) => this.OnCancelClicked();
        BtCliente.Click += async  (_, _) => await this.OnBtClienteClicked(r);
    }

    public ReparacionNavegarDlg() { }

    async Task OnBtEmpleadoClicked(Reparacion r)
    {
        var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;
                if (mainWindow == null) return;
        var empleadoDlg = new EmpleadoDlg(r.Empleado, true);
        await empleadoDlg.ShowDialog(mainWindow);
        this.OnExit();
    }

    async Task OnBtClienteClicked(Reparacion r)
    { 
        var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;
                if (mainWindow == null) return;
        var clientedlg = new ClienteDlg(r.Cliente,true);
        await clientedlg.ShowDialog(mainWindow);
        
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