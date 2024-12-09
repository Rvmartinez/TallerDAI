using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Linq;

namespace TallerDIA.Models;

public class Reparaciones
{
     private ObservableCollection<Reparacion> reparaciones;

     public int Count => reparaciones.Count;

    
     public Reparaciones(IEnumerable<Reparacion> r)
     {
         reparaciones = new ObservableCollection<Reparacion>(r);
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
    
    public void Remove(Reparacion reparacion)
    {
        reparaciones.Remove(reparacion);
    }
    

    
    public Reparacion Get(int i)
    {
        return reparaciones[i];
    }
    
    public Reparacion Get(Reparacion reparacion)
    {
        
        foreach (Reparacion rep in reparaciones)
        {
            if (rep == reparacion)
            {
                return rep;
            }
        }
        return null;
    }

    

    
    

     


   
}