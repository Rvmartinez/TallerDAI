// DemoAvalonia (c) 2021/23 Baltasar MIT License <jbgarcia@uvigo.es>


using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using DemoAvalonia.UI;
using GraficosTaller.Corefake;

namespace GraficosTaller.UI {
    public partial class DesgloseWindow : Window
    {
        public DesgloseWindow()
        {
            InitializeComponent();

           this.Chart = this.GetControl<Chart>( "ChGrf" );
           this.Chart.Type = Chart.ChartType.Bars;
           
          
            Boolean isFechaFin = true;

            Reparaciones reparaciones = InicializarReparaciones();
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
            General.Click += (sender, args) =>
            {
                new ChartWindow().Show();
                Close();
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

        private Reparaciones InicializarReparaciones()
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
            
            
            toret.AnadirReparacion(new Reparacion(){Cliente = new Cliente(), FechaInicio = new DateTime(2012, 01, 02), FechaFin = new DateTime(2014, 02, 04)});
            return toret;
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
