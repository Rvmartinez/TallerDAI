// DemoAvalonia (c) 2021/23 Baltasar MIT License <jbgarcia@uvigo.es>


using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using DemoAvalonia.UI;
using GraficosTaller.Corefake;

namespace GraficosTaller.UI {
    public partial class DesgloseWindow : Window
    {
        public DesgloseWindow(Reparaciones reparaciones)
        {
            InitializeComponent();

           this.Chart = this.GetControl<Chart>( "ChGrf" );
           this.Chart.Type = Chart.ChartType.Bars;
           
          
            Boolean isFechaFin = true;

            
            GenerateYearsCombobox(reparaciones);


            ReparacionesAnuales(reparaciones, isFechaFin, Convert.ToInt32(Annos.Items[Annos.SelectedIndex]));
            Rango.SelectionChanged += (sender, args) =>
            {
                if(Rango.SelectedIndex == 0) GenerateClientCombobox(reparaciones, Convert.ToInt32(Annos.Items[Annos.SelectedIndex]), isFechaFin);

                UpdateChart(reparaciones, isFechaFin);
            };
            Annos.SelectionChanged += (sender, args) =>
            {  
                GenerateClientCombobox(reparaciones, Convert.ToInt32(Annos.Items[Annos.SelectedIndex]), isFechaFin);
                UpdateChart(reparaciones, isFechaFin);
            };
            Computa.SelectionChanged += (sender, args) =>
            {
                isFechaFin = (Computa.SelectedIndex == 1);
                GenerateClientCombobox(reparaciones, Convert.ToInt32(Annos.Items[Annos.SelectedIndex]), isFechaFin);
                UpdateChart(reparaciones, isFechaFin);
            };
            
            


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
        
        private void GenerateClientCombobox(Reparaciones reparaciones, int anno, Boolean isFechaFin = false)
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
                UpdateChart(reparaciones, isFechaFin);
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

        private void UpdateChart(Reparaciones reparaciones, Boolean isFechaFin)
        {
            if (Rango.SelectedIndex == 0)
            {
                ReparacionesMensuales(Convert.ToInt32((object?)Annos.Items[Annos.SelectedIndex]), reparaciones, isFechaFin);
                
            }
            else
            {
                RemoveClienteComboBox();
                ReparacionesAnuales(reparaciones, isFechaFin, Convert.ToInt32((object?)Annos.Items[Annos.SelectedIndex]));
            }
        }

        private void ReparacionesMensuales(int anno, Reparaciones reparaciones, Boolean isFechaFin)
        {
            this.Chart.Type = Chart.ChartType.Lines;
            this.Chart.LegendY = "Reparaciones durante el año " + anno;
            this.Chart.LegendX = "Meses";
            List<int> valores = new List<int>();
            Reparaciones? reparacionesCliente;

            if (_clientes != null  && _clientes.Items.Count > 0)
            {
                reparacionesCliente = reparaciones.GetReparacionesCliente(_clientes.Items[_clientes?.SelectedIndex ?? 0]?.ToString() ?? "");
            }
            else 
            {
                reparacionesCliente = null;
            }

            for (int i = 1; i <= 12; i++)
            {
                if(reparacionesCliente == null) valores.Add(0);
                else if(_clientes != null&& _clientes.Items.Count != 0) valores.Add(reparacionesCliente.GetReparacionesMes(i, anno, isFechaFin)); 
            }

            this.Chart.Values = valores.ToArray();
            this.Chart.Labels = new []{ "En", "Fb", "Ma", "Ab", "My", "Jn", "Jl", "Ag", "Sp", "Oc", "Nv", "Dc" };
            this.Chart.Draw();
        }

        private void ReparacionesAnuales(Reparaciones reparaciones, Boolean isFechaFin, int anno)
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
    }
}
