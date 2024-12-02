using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ColorTextBlock.Avalonia;
using TallerDIA.Models;

namespace TallerDIA.Views.Dialogs;

public partial class CocheDlg : Window
{

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

    public CocheDlg()
    {
        InitializeComponent();
        var opciones = Enum.GetValues(typeof(Coche.Marcas)).Cast<Coche.Marcas>().ToList();
        this.MarcasCb.ItemsSource = opciones;
        this.IsCanceled = false;
        this.IsAcepted = false;
        BtOk.IsEnabled = false;
        BtOk.Click += (_, _) => this.Acept();
        BtCancel.Click += (_, _) => this.Cancel();

        this.Closed += (_, _) => this.OnWindowClosed();
    }

    private void OnWindowClosed()
    {
        if (IsAcepted == false)
        {
            IsCanceled = true;
        }
    }

    public CocheDlg(Coche car)
    {
        InitializeComponent();
        var opciones = Enum.GetValues(typeof(Coche.Marcas)).Cast<Coche.Marcas>().ToList();
        this.MarcasCb.ItemsSource = opciones;
        this.IsCanceled = false;
        BtOk.IsEnabled = false;
        BtOk.Click += (_, _) => this.Acept();
        BtCancel.Click += (_, _) => this.Cancel();
        MatriculaTb.Text = car.Matricula;
        ModeloTb.Text = car.Modelo;
        MarcasCb.SelectedValue = car.Marca;

        this.Closed += (_, _) => this.OnWindowClosed();
    }

    private void matriculaValida(object? sender, TextChangedEventArgs textChangedEventArgs)
    {
        if (MatriculaTb.Text == null || MatriculaTb.Text == "")
        {
            ErrorMat.IsVisible = true;
            BtOk.IsEnabled = false;
        }
        else
        {
            if (ErrorMod.IsVisible == false && ErrorMarc.IsVisible == false)
            {
                BtOk.IsEnabled = true;
            }
            ErrorMat.IsVisible = false;
        }
    }

    private void marcaValida(object? sender, SelectionChangedEventArgs selectionChangedEventArgs)
    {
        if (MarcasCb.SelectedItem == null)
        {
            ErrorMarc.IsVisible = true;
            BtOk.IsEnabled = false;
        }
        else
        {
            if (ErrorMod.IsVisible == false && ErrorMat.IsVisible == false)
            {
                BtOk.IsEnabled = true;
            }
            ErrorMarc.IsVisible = false;
        }
    }

    private void modeloValido(object? sender, TextChangedEventArgs textChangedEventArgs)
    {
        if (ModeloTb.Text == null || ModeloTb.Text == "")
        {
            ErrorMod.IsVisible = true;
            BtOk.IsEnabled = false;
        }
        else
        {
            if (ErrorMarc.IsVisible == false && ErrorMarc.IsVisible == false)
            {
                BtOk.IsEnabled = true;
            }
            ErrorMod.IsVisible = false;
        }
    }

    public void Cancel()
    {
        IsCanceled = true;
        this.Close();
    }

    public void Acept()
    {
        IsAcepted = true;
        this.Close();
    }
}