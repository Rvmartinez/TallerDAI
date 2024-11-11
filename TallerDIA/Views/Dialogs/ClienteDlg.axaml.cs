using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Diagnostics;
using TallerDIA.Models;

namespace TallerDIA.Views.Dialogs;

public partial class ClienteDlg : Window
{
    public ClienteDlg(Cliente c)
    {
        InitializeComponent();
        EmailTB.Text = c.Email;
        NombreTB.Text = c.Nombre;
        DniTB.Text = c.DNI;
        this.IsCancelled = false;

        BtOk.Click += (_, _) => this.OnExit();
        BtCancel.Click += (_, _) => this.OnCancelClicked();

    }

    public ClienteDlg()
    {
        InitializeComponent();
        EmailTB.Text = "Email";
        NombreTB.Text = "Nombre";
        DniTB.Text = "DNI";
        this.IsCancelled = false;

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