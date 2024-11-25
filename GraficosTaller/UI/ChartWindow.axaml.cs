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
           
          
           ConfigChartFunction(config);

            DesplegableAnnos(reparaciones);

            Annos.IsVisible = false;
            AnnosText.IsVisible = false;
            ReparacionesAnuales(reparaciones);
            Rango.SelectionChanged += (sender, args) =>
            {
                mostrandoAnuales = (Rango.SelectedIndex == 1);
                UpdateChart(reparaciones);
            };
            Annos.SelectionChanged += (sender, args) =>
            {
                annoSelected = Convert.ToInt32(Annos.Items[Annos.SelectedIndex]);
                UpdateChart(reparaciones);
            };
            Computa.SelectionChanged += (sender, args) =>
            {
                isFechaFin = (Computa.SelectedIndex == 1);
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
                    annosFilter = true;
                }

                if (config.FechaFin != null)
                {
                    isFechaFin = (bool)config.FechaFin;
                    ComputaText.IsVisible = false;
                    Computa.IsVisible = false;
                }
            }
        }

        private void DesplegableAnnos(Reparaciones reparaciones)
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

        private void UpdateChart(Reparaciones reparaciones)
        {
            if (!mostrandoAnuales)
            {
                Annos.IsVisible = !annosFilter;
                AnnosText.IsVisible = !annosFilter;

                ReparacionesDelAnno(annoSelected, reparaciones);
            }
            else
            {
                Annos.IsVisible = false;
                AnnosText.IsVisible = false;
                ReparacionesAnuales(reparaciones);
            }
        }
        

        private void ReparacionesDelAnno(int anno, Reparaciones reparaciones)
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

        private void ReparacionesAnuales(Reparaciones reparaciones)
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
        private int annoSelected;
        private bool annosFilter = false;
        private bool mostrandoAnuales;
        private bool rangoFilter = false;
        private bool isFechaFin = true;
    }
}
