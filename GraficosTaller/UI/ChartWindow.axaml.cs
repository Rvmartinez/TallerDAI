// DemoAvalonia (c) 2021/23 Baltasar MIT License <jbgarcia@uvigo.es>


using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using DemoAvalonia.UI;
using GraficosTaller.Corefake;

namespace GraficosTaller.UI {
    public partial class ChartWindow : Window
    {

        public ChartWindow(Reparaciones reparaciones, ConfigChart? config)
        {
            InitializeComponent();

            var aux = Rango.SelectedIndex;
           this.Chart = this.GetControl<Chart>( "ChGrf" );
           
          
            Boolean isFechaFin = true;


            DesplegableAnnos(reparaciones, isFechaFin);
            Annos.IsVisible = false;
            AnnosText.IsVisible = false;
            ReparacionesAnuales(reparaciones, isFechaFin);
            Rango.SelectionChanged += (sender, args) =>
            {
                UpdateChart(reparaciones, isFechaFin);
            };
            Annos.SelectionChanged += (sender, args) =>
            {  
                UpdateChart(reparaciones, isFechaFin);
            };
            Computa.SelectionChanged += (sender, args) =>
            {
                isFechaFin = (Computa.SelectedIndex == 1);
                UpdateChart(reparaciones, isFechaFin);
            };
            
            ConfigChartFunction(config);
            


        }

        private void ConfigChartFunction(ConfigChart? config)
        {
            if (config != null)
            {
                if (config.Modo == ConfigChart.ModoVision.Anual)
                {
                    this.Chart.Type = Chart.ChartType.Bars;
                }
                else
                {
                    this.Chart.Type = Chart.ChartType.Lines;
                }
            }
        }

        private void DesplegableAnnos(Reparaciones reparaciones, Boolean isFechaFin)
        {
            Annos.Items.Clear();
            foreach (var anno in reparaciones.GetAnnosReparaciones(isFechaFin))
            {
                if(!Annos.Items.Contains(anno)) Annos.Items.Add(anno);
            }
            Annos.SelectedIndex=0;
            Annos.IsVisible = true;
            AnnosText.IsVisible = true;
        }

        private void UpdateChart(Reparaciones reparaciones, Boolean isFechaFin)
        {
            if (Rango.SelectedIndex == 0)
            {
                Annos.IsVisible = true;
                AnnosText.IsVisible = true;

                ReparacionesDelAnno(Convert.ToInt32(Annos.Items[Annos.SelectedIndex]), reparaciones, isFechaFin);
            }
            else
            {
                Annos.IsVisible = false;
                AnnosText.IsVisible = false;
                ReparacionesAnuales(reparaciones, isFechaFin);
            }
        }
        

        private void ReparacionesDelAnno(int anno, Reparaciones reparaciones, Boolean isFechaFin)
        {
            this.Chart.LegendY = "Reparaciones durante el año " + anno;
            this.Chart.LegendX = "Meses";
            List<int> valores = new List<int>();
            for (int i = 1; i <= 12; i++)
            {
               valores.Add(reparaciones.GetReparacionesMes(i, anno, isFechaFin)); 
            }

            this.Chart.Values = valores.ToArray();
            this.Chart.Labels = new []{ "En", "Fb", "Ma", "Ab", "My", "Jn", "Jl", "Ag", "Sp", "Oc", "Nv", "Dc" };
            this.Chart.Draw();
        }

        private void ReparacionesAnuales(Reparaciones reparaciones, Boolean isFechaFin)
        {
            List<int> valores = new List<int>();
            List<int> annos = new List<int>();
            foreach (var anno in reparaciones.GetAnnosReparaciones(isFechaFin))
            {
                valores.Add(reparaciones.GetReparacionesAnno(anno, isFechaFin));
                annos.Add(anno);
            }
            this.Chart.Values = valores.ToArray();
            this.Chart.Labels = annos.ConvertAll(x => x.ToString()).ToArray();
            this.Chart.LegendY = "Reparaciones por año";
            this.Chart.LegendX = "Años";
            this.Chart.Draw();
        }


        private Chart Chart;
    }
}
