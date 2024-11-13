using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ConsoleApp1;

namespace Test4TallerAfc.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{public ObservableCollection<Empleado> Empleados { get; set; }

    public MainWindowViewModel()
    {
        
        List<Empleado> empleados = new List<Empleado> 
        {
            // !!! - PROVISIONAL, AQUÍ SE CARGARÍA EL XML - !!!
            new Empleado("12345678A","Abelardo","averelardo@hotcorreo.coom",["1","2"]),
            new Empleado("22345678B","Luffy","onepieceismid@ymail.com",["3","4"]),
            new Empleado("32345678C","Rudero","rudero@ubigo.es",["5","6"])
        };
        Empleados = new ObservableCollection<Empleado>(empleados);/**/
    }
}