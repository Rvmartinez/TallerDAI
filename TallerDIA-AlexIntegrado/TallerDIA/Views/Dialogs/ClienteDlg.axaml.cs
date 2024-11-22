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
        this.IsCancelled = false;
        DniErrorTB.IsVisible = false;
        EmailErrorTB.IsVisible = false;
        BtOk.IsEnabled = false;
        BtOk.Click += (_, _) => this.OnExit();
        BtCancel.Click += (_, _) => this.OnCancelClicked();

    }

    public ClienteDlg()
    {
        InitializeComponent();
        this.IsCancelled = false;
        EmailErrorTB.IsVisible = false;
        DniErrorTB.IsVisible = false;
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
            if (!DniErrorTB.IsVisible && DniTB.Text != "")
                BtOk.IsEnabled = true;
        }
        else
        {
            DniErrorTB.IsVisible = true;
            BtOk.IsEnabled = false;
        }
    }
}