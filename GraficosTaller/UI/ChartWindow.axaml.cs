// DemoAvalonia (c) 2021/23 Baltasar MIT License <jbgarcia@uvigo.es>


using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using Avalonia.Media;
using GraficosTaller.Corefake;

namespace DemoAvalonia.UI {
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    

    public partial class ChartWindow : Window
    {
        public ChartWindow()
        {
            InitializeComponent();

            var aux = Rango.SelectedIndex;
           this.Chart = this.GetControl<Chart>( "ChGrf" );
           
          
            Boolean isFechaFin = true;

            Reparaciones reparaciones = inicializarReparaciones();
            desplegableAnnos(reparaciones, isFechaFin);

            reparacionesAnuales(reparaciones, isFechaFin);
            Rango.SelectionChanged += (sender, args) =>
            {
                updateChart(reparaciones, isFechaFin);
            };
            Annos.SelectionChanged += (sender, args) =>
            {  
                updateChart(reparaciones, isFechaFin);
            };
            Computa.SelectionChanged += (sender, args) =>
            {
                isFechaFin = (Computa.SelectedIndex == 1);
                updateChart(reparaciones, isFechaFin);
            };
            Desglose.Click += (sender, args) =>
            {
                new DesgloseWindow().Show();
                Close();
            };


        }
        
        private void desplegableAnnos(Reparaciones reparaciones, Boolean isFechaFin)
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

        private void updateChart(Reparaciones reparaciones, Boolean isFechaFin)
        {
            if (Rango.SelectedIndex == 0)
            {
                reparacionesDelAnno(Convert.ToInt32(Annos.Items[Annos.SelectedIndex]), reparaciones, isFechaFin);
            }
            else
            {
                Annos.IsVisible = false;
                AnnosText.IsVisible = false;
                reparacionesAnuales(reparaciones, isFechaFin);
            }
        }

        private Reparaciones inicializarReparaciones()
        {
            Reparaciones toret = new Reparaciones();
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2023, 01, 02), FechaFin = new DateTime(2023, 02, 04)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2023, 03, 10), FechaFin = new DateTime(2023, 03, 25)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2023, 04, 15), FechaFin = new DateTime(2023, 05, 20)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2023, 06, 05), FechaFin = new DateTime(2023, 06, 18)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2023, 07, 21), FechaFin = new DateTime(2023, 08, 12)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2023, 09, 15), FechaFin = new DateTime(2023, 10, 01)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2023, 11, 08), FechaFin = new DateTime(2023, 11, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2023, 12, 02), FechaFin = new DateTime(2023, 12, 28)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 01, 10), FechaFin = new DateTime(2024, 01, 20)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 02, 05), FechaFin = new DateTime(2024, 02, 25)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 03, 08), FechaFin = new DateTime(2024, 03, 28)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 04, 12), FechaFin = null});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 05, 17), FechaFin = new DateTime(2024, 06, 01)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 06, 07), FechaFin = new DateTime(2024, 06, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 07, 19), FechaFin = new DateTime(2024, 08, 04)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 08, 23), FechaFin = new DateTime(2024, 09, 07)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 09, 15), FechaFin = new DateTime(2024, 10, 01)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 10, 19), FechaFin = new DateTime(2024, 11, 05)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 11, 10), FechaFin = new DateTime(2024, 11, 24)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 12, 01), FechaFin = new DateTime(2024, 12, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 01, 10), FechaFin = new DateTime(2024, 01, 20)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 02, 05), FechaFin = new DateTime(2024, 02, 25)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 03, 08), FechaFin = new DateTime(2024, 03, 28)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 04, 12), FechaFin = new DateTime(2024, 04, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 05, 17), FechaFin = new DateTime(2024, 06, 01)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2023, 06, 07), FechaFin = new DateTime(2024, 06, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2022, 07, 19), FechaFin = new DateTime(2024, 08, 04)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2021, 08, 23), FechaFin = new DateTime(2024, 09, 07)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 09, 15), FechaFin = new DateTime(2024, 10, 01)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 10, 19), FechaFin = new DateTime(2024, 11, 05)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2023, 11, 10), FechaFin = new DateTime(2024, 11, 24)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2023, 12, 01), FechaFin = new DateTime(2024, 12, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2023, 01, 10), FechaFin = new DateTime(2024, 01, 20)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2022, 02, 05), FechaFin = new DateTime(2024, 02, 25)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2022, 03, 08), FechaFin = new DateTime(2024, 03, 28)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2022, 04, 12), FechaFin = new DateTime(2024, 04, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2022, 05, 17), FechaFin = new DateTime(2024, 06, 01)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2022, 06, 07), FechaFin = new DateTime(2024, 06, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2022, 07, 19), FechaFin = new DateTime(2024, 08, 04)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 08, 23), FechaFin = new DateTime(2024, 09, 07)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 09, 15), FechaFin = new DateTime(2024, 10, 01)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 10, 19), FechaFin = null});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 11, 10), FechaFin = new DateTime(2024, 11, 24)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2024, 12, 01), FechaFin = new DateTime(2024, 12, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2016, 01, 10), FechaFin = new DateTime(2016, 01, 20)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2016, 02, 05), FechaFin = new DateTime(2016, 02, 25)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2016, 03, 08), FechaFin = new DateTime(2016, 03, 28)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2016, 04, 12), FechaFin = new DateTime(2016, 04, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2016, 05, 17), FechaFin = new DateTime(2016, 06, 01)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2016, 06, 07), FechaFin = new DateTime(2016, 06, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2016, 07, 19), FechaFin = new DateTime(2016, 08, 04)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2016, 08, 23), FechaFin = new DateTime(2016, 09, 07)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2017, 09, 15), FechaFin = new DateTime(2016, 10, 01)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2017, 10, 19), FechaFin = new DateTime(2016, 11, 05)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2017, 11, 10), FechaFin = new DateTime(2016, 11, 24)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2017, 12, 01), FechaFin = new DateTime(2016, 12, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2017, 05, 10), FechaFin = new DateTime(2017, 01, 20)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2017, 05, 05), FechaFin = new DateTime(2017, 02, 25)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2017, 05, 08), FechaFin = new DateTime(2017, 03, 28)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2017, 05, 12), FechaFin = new DateTime(2020, 04, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2017, 05, 17), FechaFin = new DateTime(2020, 06, 01)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2017, 05, 07), FechaFin = new DateTime(2020, 06, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2017, 05, 19), FechaFin = new DateTime(2020, 08, 04)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2017, 08, 23), FechaFin = new DateTime(2020, 09, 07)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2017, 09, 15), FechaFin = new DateTime(2020, 10, 01)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2018, 10, 19), FechaFin = new DateTime(2020, 11, 05)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2018, 11, 10), FechaFin = new DateTime(2020, 11, 24)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2019, 12, 01), FechaFin = new DateTime(2020, 12, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2018, 01, 10), FechaFin = new DateTime(2020, 01, 20)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2018, 02, 05), FechaFin = new DateTime(2020, 02, 25)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2018, 03, 08), FechaFin = new DateTime(2020, 03, 28)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2020, 04, 12), FechaFin = new DateTime(2020, 04, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2021, 05, 17), FechaFin = new DateTime(2020, 06, 01)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2022, 06, 07), FechaFin = new DateTime(2020, 06, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2021, 07, 19), FechaFin = new DateTime(2020, 08, 04)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2018, 08, 23), FechaFin = new DateTime(2020, 09, 07)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2019, 09, 15), FechaFin = new DateTime(2020, 10, 01)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2016, 10, 19), FechaFin = new DateTime(2020, 11, 05)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2017, 11, 10), FechaFin = new DateTime(2020, 11, 24)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2017, 12, 01), FechaFin = new DateTime(2020, 12, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2017, 01, 10), FechaFin = new DateTime(2020, 01, 20)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2016, 02, 05), FechaFin = new DateTime(2020, 02, 25)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2019, 03, 08), FechaFin = new DateTime(2020, 03, 28)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2019, 04, 12), FechaFin = new DateTime(2020, 12, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2019, 05, 17), FechaFin = new DateTime(2020, 12, 01)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2019, 06, 07), FechaFin = new DateTime(2019, 12, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2019, 07, 19), FechaFin = new DateTime(2019, 12, 04)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2018, 08, 23), FechaFin = new DateTime(2019, 12, 07)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2018, 09, 15), FechaFin = new DateTime(2019, 12, 01)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2018, 12, 19), FechaFin = new DateTime(2019, 12, 05)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2019, 12, 10), FechaFin = new DateTime(2019, 12, 24)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2019, 12, 01), FechaFin = new DateTime(2019, 12, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2017, 12, 10), FechaFin = new DateTime(2020, 12, 20)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2017, 12, 05), FechaFin = new DateTime(2020, 12, 25)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2017, 12, 08), FechaFin = new DateTime(2020, 12, 28)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2017, 12, 12), FechaFin = new DateTime(2020, 12, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2017, 12, 17), FechaFin = new DateTime(2020, 12, 01)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2017, 12, 07), FechaFin = new DateTime(2020, 12, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2016, 12, 19), FechaFin = new DateTime(2020, 12, 04)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2016, 12, 23), FechaFin = new DateTime(2020, 12, 07)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2016, 12, 15), FechaFin = new DateTime(2020, 12, 01)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2016, 12, 19), FechaFin = new DateTime(2020, 12, 05)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2016, 12, 10), FechaFin = new DateTime(2020, 12, 24)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2016, 12, 01), FechaFin = new DateTime(2020, 12, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2021, 12, 10), FechaFin = new DateTime(2021, 12, 20)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2021, 12, 05), FechaFin = new DateTime(2021, 12, 25)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2021, 03, 08), FechaFin = new DateTime(2021, 12, 28)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2021, 04, 12), FechaFin = new DateTime(2021, 12, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2021, 05, 17), FechaFin = new DateTime(2021, 12, 01)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2021, 06, 07), FechaFin = new DateTime(2021, 12, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2021, 07, 19), FechaFin = new DateTime(2021, 12, 04)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2021, 08, 23), FechaFin = new DateTime(2021, 12, 07)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2021, 09, 15), FechaFin = new DateTime(2021, 12, 01)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2021, 10, 19), FechaFin = new DateTime(2021, 12, 05)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2021, 11, 10), FechaFin = new DateTime(2021, 12, 24)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2021, 12, 01), FechaFin = new DateTime(2021, 12, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2022, 01, 10), FechaFin = new DateTime(2022, 12, 20)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2022, 02, 05), FechaFin = new DateTime(2022, 12, 25)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2022, 03, 08), FechaFin = new DateTime(2022, 12, 28)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2022, 04, 12), FechaFin = new DateTime(2022, 04, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2020, 05, 17), FechaFin = new DateTime(2022, 06, 01)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2020, 06, 07), FechaFin = new DateTime(2022, 06, 22)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2020, 07, 19), FechaFin = new DateTime(2022, 08, 04)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2020, 08, 23), FechaFin = new DateTime(2022, 09, 07)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2020, 09, 15), FechaFin = new DateTime(2022, 10, 01)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2022, 10, 19), FechaFin = new DateTime(2022, 11, 05)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2022, 11, 10), FechaFin = new DateTime(2022, 11, 24)});
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2022, 12, 01), FechaFin = new DateTime(2022, 12, 22)});
            
            return toret;
        }

        private void reparacionesDelAnno(int anno, Reparaciones reparaciones, Boolean isFechaFin)
        {
            this.Chart.LegendY = "Reparaciones últimos 12 meses";
            this.Chart.LegendX = "Months";
            List<int> valores = new List<int>();
            for (int i = 1; i <= 12; i++)
            {
               valores.Add(reparaciones.GetReparacionesMes(i, anno, isFechaFin)); 
            }

            this.Chart.Values = valores.ToArray();
            this.Chart.Labels = new []{ "En", "Fb", "Ma", "Ab", "My", "Jn", "Jl", "Ag", "Sp", "Oc", "Nv", "Dc" };
            this.Chart.Draw();
        }

        private void reparacionesAnuales(Reparaciones reparaciones, Boolean isFechaFin)
        {
            List<int> valores = new List<int>();
            List<int> annos = new List<int>();
            foreach (var anno in reparaciones.GetAnnosReparaciones())
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

        void OnChartFormatChanged()
        {
            var rbBars = this.GetControl<RadioButton>( "RbBars" );
            var rbLine = this.GetControl<RadioButton>( "RbLine" );
            var edThickness = this.GetControl<NumericUpDown>( "EdThickness" );

            if ( rbBars.IsChecked.HasValue
             && rbBars.IsChecked.Value )
            {
                this._chartType = Chart.ChartType.Bars;
            }
            else
            if ( rbLine.IsChecked.HasValue
              && rbLine.IsChecked.Value )
            {
                this._chartType = Chart.ChartType.Lines;
            }
            
            if ( this._chartType == Chart.ChartType.Lines ) {
                this.Chart.Type = Chart.ChartType.Lines;
                this.Chart.DataPen = new Pen( Brushes.Red, 2 * ( ( (double?) edThickness.Value ) ?? 1 ) );
            } else {
                this.Chart.Type = Chart.ChartType.Bars;
                this.Chart.DataPen = new Pen( Brushes.Navy, 20 * ( ( (double?) edThickness.Value ?? 1 ) ) );
            }
            
            this.Chart.Draw();
        }

        void OnChartThicknessChanged(double thickness)
        {
            if ( this.Chart.Type == Chart.ChartType.Bars ) {
                this.Chart.DataPen = new Pen( this.Chart.DataPen.Brush, 20 * thickness );
            } else {
                this.Chart.DataPen = new Pen( this.Chart.DataPen.Brush, 2 * thickness );
            }
            
            this.Chart.AxisPen = new Pen( this.Chart.AxisPen.Brush, 4 * thickness );
            this.Chart.Draw();
        }

        void OnFontsStyleChanged()
        {
            var cbBold = this.GetControl<CheckBox>( "CbBold" );
            var cbItalic = this.GetControl<CheckBox>( "CbItalic" );
            bool italic = cbItalic.IsChecked ?? false;
            bool bold = cbBold.IsChecked ?? false;
            FontStyle style = italic ? FontStyle.Italic : FontStyle.Normal;
            FontWeight weight = bold ? FontWeight.Bold : FontWeight.Normal;

            this.Chart.DataFont = new Chart.Font( this.Chart.DataFont.Size ) {
                Family = this.Chart.DataFont.Family,
                Style = style,
                Weight = weight
            };
            
            this.Chart.LabelFont = new Chart.Font( this.Chart.LabelFont.Size ) {
                Family = this.Chart.LabelFont.Family,
                Style = style,
                Weight = weight
            };
            
            this.Chart.LegendFont = new Chart.Font( this.Chart.LegendFont.Size ) {
                Family = this.Chart.LegendFont.Family,
                Style = style,
                Weight = weight
            };
            
            this.Chart.Draw();
        }

        /*void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }*/
        
        private Chart Chart { get; }
        private Chart.ChartType _chartType;
    }
}
