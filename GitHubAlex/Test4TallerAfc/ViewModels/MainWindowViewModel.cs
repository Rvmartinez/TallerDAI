using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ConsoleApp1;

namespace Test4TallerAfc.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<Empleado> Empleados { get; set; }
    public MainWindowViewModel()
    {
        List<Empleado> empleados = new List<Empleado> 
        {
            new Empleado("12345678A","Abelardo","averelardo@hotcorreo.coom",["Ticket Provisional 1","Ticket Provisional 2"]),
            new Empleado("22345678B","Luffy","onepieceismid@ymail.com",["Ticket Provisional 3","Ticket Provisional 4","Ticket Provisional 5"]),
            new Empleado("32345678C","Rudero","rudero@ubigo.es",["Ticket Provisional 8"]),
            new Empleado("42345678D","John Doe","invented@fakemail.net",["Ticket Provisional 9","Ticket Provisional 10"]),
            new Empleado("52345678E","Notch","soldsoul@tomojang.us",[])
        };
        Empleados = new ObservableCollection<Empleado>(empleados);/**/
    }
}