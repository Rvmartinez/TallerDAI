using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TallerDIA.Models;

namespace TallerDIA.Views.Dialogs;

public partial class ClienteDlg : Window
{

    private const string DniRegex = @"^\d{8}[A-HJ-NP-TV-Z]$";
    private const string EmailRegex = @"^[^@]+@[^@]+\.[^@]+$";
    public ClienteDlg(Cliente c)
    {
        InitializeComponent();
        EmailTB.Text = c.Email;
        NombreTB.Text = c.Nombre;
        DniTB.Text = c.DNI;
        this.IsCancelled = true;
        DniErrorTB.IsVisible = false;
        EmailErrorTB.IsVisible = false;
        BtOk.IsEnabled = false;
        BtOk.Click += (_, _) => this.OnAcceptClicked();
        BtCancel.Click += (_, _) => this.OnExit();

    }
    
    public ClienteDlg(Cliente c,bool toret)
    {
        InitializeComponent();
        EmailTB.Text = c.Email;
        NombreTB.Text = c.Nombre;
        DniTB.Text = c.DNI;
        this.IsCancelled = true;
        DniErrorTB.IsVisible = false;
        EmailErrorTB.IsVisible = false;
        BtOk.IsEnabled = false;
        if (toret)
        {
           EmailTB.IsReadOnly = true;
           NombreTB.IsReadOnly = true;
           DniTB.IsReadOnly = true;
            BtOk.IsEnabled = false;
        }
       
        BtCancel.Click += (_, _) => this.OnExit();

    }

    public ClienteDlg()
    {
        InitializeComponent();
        this.IsCancelled = true;
        EmailErrorTB.IsVisible = false;
        BtOk.IsEnabled = false;

        DniErrorTB.IsVisible = false;
        BtOk.Click += (_, _) => this.OnAcceptClicked();
        BtCancel.Click += (_, _) => this.OnExit();
    }

    void OnAcceptClicked()
    {
        this.IsCancelled = false;
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

    private void Email_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        if (EmailTB.Text == "" ||  EmailTB.Text.Length < 8)
        {
            EmailErrorTB.IsVisible = false;
        }
        if (Regex.IsMatch(EmailTB.Text ?? string.Empty, EmailRegex, RegexOptions.IgnoreCase))
        {

            EmailErrorTB.IsVisible = false;

            if (!DniErrorTB.IsVisible && DniTB.Text != "")
                BtOk.IsEnabled = true;
        }
        else
        {
            EmailErrorTB.IsVisible = true;
            BtOk.IsEnabled = false;
        }
    }

    private void DNI_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        if (DniTB.Text == "" ||  DniTB.Text.Length < 8)
        {
            DniErrorTB.IsVisible = false;
        }
        if (Regex.IsMatch(DniTB.Text ?? string.Empty, DniRegex, RegexOptions.IgnoreCase))
        {
            DniErrorTB.IsVisible = false;
            if (!EmailErrorTB.IsVisible && EmailTB.Text != "" )
                BtOk.IsEnabled = true;
        }
        else
        {
            DniErrorTB.IsVisible = true;
            BtOk.IsEnabled = false;
        }
    }
}