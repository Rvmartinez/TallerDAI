// DemoAvalonia (c) 2021/23 Baltasar MIT License <jbgarcia@uvigo.es>


using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using DemoAvalonia.UI;
using TallerDIA.Views;
using TallerDIA.Models;

namespace GraficosTaller.UI {
    public partial class DesgloseWindow : Window
    {
        public DesgloseWindow(Reparaciones reparaciones, ConfigChart config)
        {
            InitializeComponent();

           this.Chart = this.GetControl<Chart>( "ChGrf" );
           this.Chart.Type = Chart.ChartType.Bars;


           ConfigChartFunction(config);
           GenerateYearsCombobox(reparaciones);

           
            if (annoSelected == 0) annoSelected = Convert.ToInt32(Annos.Items[0]?.ToString());


            UpdateChart(reparaciones);
            Rango.SelectionChanged += (sender, args) =>
            {
                mostrandoAnuales = (Rango.SelectedIndex == 1);

                if(Rango.SelectedIndex == 0 && clienteFilter is null) GenerateClientCombobox(reparaciones, annoSelected);

                UpdateChart(reparaciones);
            };
            Annos.SelectionChanged += (sender, args) =>
            {  
                annoSelected = Convert.ToInt32(Annos.Items[Annos.SelectedIndex]);

                GenerateClientCombobox(reparaciones, annoSelected);
                UpdateChart(reparaciones);
            };
            Computa.SelectionChanged += (sender, args) =>
            {
                isFechaFin = (Computa.SelectedIndex == 1);
                GenerateClientCombobox(reparaciones, annoSelected);
                UpdateChart(reparaciones);
            };
            
            


        }
        
        private void ConfigChartFunction(ConfigChart? config)
        {
            if (config != null)
            {
                if (config.Modo != null)
                {
                    Rango.IsVisible = false;
                    RangoText.IsVisible = false;
                    rangoFilter = true;
                    mostrandoAnuales = (config.Modo == 0);
                }

                if (config.Anno != null)
                {
                    Annos.IsVisible = false;
                    AnnosText.IsVisible = false;
                    annoSelected = (int)config.Anno;
                }

                if (config.FechaFin != null)
                {
                    isFechaFin = (bool)config.FechaFin;
                    ComputaText.IsVisible = false;
                    Computa.IsVisible = false;
                }

                if (config.Cliente != null)
                {
                    clienteFilter = config.Cliente;
                    ClientesText.IsVisible = false;
                    
                }

                if (config.Modo == ConfigChart.ModoVision.Anual && config.Cliente is not null ||
                    config.Modo == ConfigChart.ModoVision.Mensual && config.Cliente is null)
                {
                    throw new ArgumentException("Esta grafica no soporta ese tipo de filtrado");
                }
            }
        }
        
        private void GenerateYearsCombobox(Reparaciones reparaciones)
        {
            Annos.Items.Clear();
            List<int> annos = new List<int>();
            
            foreach (var anno in reparaciones.GetAnnosReparaciones(true))
            {
                if(!annos.Contains(anno)) annos.Add(anno);
            }

            foreach (var anno in reparaciones.GetAnnosReparaciones(false))
            {
                if(!annos.Contains(anno)) annos.Add(anno);
            }
            
            annos = annos.OrderByDescending(anno => anno).ToList();
            annos.ForEach(anno => Annos.Items.Add(anno));
            
            Annos.SelectedIndex=0;
        }
        
        private void GenerateClientCombobox(Reparaciones reparaciones, int anno)
        {
            RemoveClienteComboBox();
          

            ComboBox clientes = new ComboBox
            {
                Name = "Clientes",
                IsVisible = true
            };

            foreach (var cliente in reparaciones.GetClientesReparaciones())
            {
                if (!clientes.Items.Contains(cliente) && reparaciones.GetReparacionesCliente(cliente).GetReparacionesAnno(anno, isFechaFin) > 0)
                {
                    clientes.Items.Add(cliente);
                }
            }

            if (clientes.Items.Count > 0)
            {
                clientes.SelectedIndex = 0;
            }
            clientes.SelectionChanged += (sender, args) =>
            {
                clienteFilter = clientes.Items[clientes.SelectedIndex]?.ToString();
                UpdateChart(reparaciones);
            };

            Options.Children.Add(clientes);
            _clientes = clientes;
            ClientesText.IsVisible = true;

        }
        
        private void RemoveClienteComboBox()
        {
            if (_clientes != null)
            {
                Options.Children.Remove(_clientes);
            }
            ClientesText.IsVisible = false;

        }

        private void UpdateChart(Reparaciones reparaciones)
        {
            
            if (!mostrandoAnuales)
            {
                ReparacionesMensuales(annoSelected, reparaciones);
                
            }
            else
            {
                RemoveClienteComboBox();
                ReparacionesAnuales(reparaciones, annoSelected);
            }
        }

        private void ReparacionesMensuales(int anno, Reparaciones reparaciones)
        {
            this.Chart.Type = Chart.ChartType.Lines;
            this.Chart.LegendY = "Reparaciones durante el año " + anno;
            this.Chart.LegendX = "Meses";
            List<int> valores = new List<int>();
            Reparaciones? reparacionesCliente;

            if (_clientes != null  && _clientes.Items.Count > 0 && clienteFilter is null)
            {
                reparacionesCliente = reparaciones.GetReparacionesCliente(_clientes.Items[_clientes?.SelectedIndex ?? 0]?.ToString() ?? "");
            }
            else if (clienteFilter is not null)
            {
                reparacionesCliente = reparaciones.GetReparacionesCliente(clienteFilter);
            }
            else 
            {
                reparacionesCliente = null;
            }

            for (int i = 1; i <= 12; i++)
            {
                if(reparacionesCliente == null) valores.Add(0);
                else if((_clientes != null && _clientes.Items.Count != 0) || clienteFilter is not null) valores.Add(reparacionesCliente.GetReparacionesMes(i, anno, isFechaFin)); 
            }

            this.Chart.Values = valores.ToArray();
            this.Chart.Labels = new []{ "En", "Fb", "Ma", "Ab", "My", "Jn", "Jl", "Ag", "Sp", "Oc", "Nv", "Dc" };
            this.Chart.Draw();
        }

        private void ReparacionesAnuales(Reparaciones reparaciones, int anno)
        {
            this.Chart.Type = Chart.ChartType.Bars;
            List<int> valores = new List<int>();
            List<string> clientes = new List<string>();
            foreach (var ncliente in reparaciones.GetClientesReparaciones())
            {
                valores.Add(reparaciones.GetReparacionesCliente(ncliente).GetReparacionesAnno(anno, isFechaFin));
                clientes.Add(ncliente);
            }
            this.Chart.Values = valores.ToArray();
            this.Chart.Labels = clientes.ConvertAll(x => x.ToString()).ToArray();
            this.Chart.LegendY = "Reparaciones por cliente el año " + anno;
            this.Chart.LegendX = "Clientes";
            this.Chart.Draw();
        }

     
        
        private Chart Chart { get; }
        private ComboBox? _clientes = null;
        private int annoSelected;
        private bool mostrandoAnuales;
        private bool rangoFilter = false;
        private bool isFechaFin = true;
        private string? clienteFilter = null;
    }
}
