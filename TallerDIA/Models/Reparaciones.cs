using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Linq;

namespace TallerDIA.Models;

public class Reparaciones
{
     private ObservableCollection<Reparacion> reparaciones;

     public int Count => reparaciones.Count;

    
    public Reparaciones()
    {
        reparaciones = new ObservableCollection<Reparacion>();
    }
    
    public Reparaciones(IEnumerable<Reparacion> c) : this()
    {
        foreach (Reparacion reparacion in c)
        {
            reparaciones.Add(reparacion);
        }
    }

   
    public void AddRange(IEnumerable<Reparacion> c)
    {
        foreach (Reparacion reparacion in c)
        {
            reparaciones.Add(reparacion);
        }
    }

    
    public void Add(Reparacion reparacion)
    {
        reparaciones.Add(reparacion);
    }
    

    
    public Reparacion Get(int i)
    {
        return reparaciones[i];
    }

    

    
    

     


   
}