
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Linq;


using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;


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

   
    
    

    public int GetReparacionesAnno(int anno, Boolean fin)
    {
        return reparaciones.Count(reparacion => reparacion.GetAnno(fin) == anno);
    }
    
    public int GetReparacionesMes(int mes, int anno, Boolean fin)
    {
        return reparaciones.Count(reparacion => reparacion.GetMes(fin) == mes && reparacion.GetAnno(fin) == anno);
    }
    
    
    public Reparaciones GetReparacionesCliente(String cliente)
    {
        Reparaciones toret = new Reparaciones(this.reparaciones.Where(reparacion => reparacion.Cliente.Nombre == cliente).ToList());
        return toret;
    }
    
    public string[] GetClientesReparaciones()
    {
        List<string> clientes = new List<string>();
        foreach(var reparacion in reparaciones)
        {
            if (!clientes.Contains(reparacion.Cliente.Nombre))
            {
                clientes.Add(reparacion.Cliente.Nombre);
            }
        }

        return clientes.ToArray();
    }

    public int[] GetAnnosReparaciones(Boolean fin)
    {
        List<int> annos = new List<int>();
        foreach(var reparacion in reparaciones)
        {
            var anno = reparacion.GetAnno(fin);
            if (anno.HasValue && !annos.Contains(anno.Value))
            {
                annos.Add(anno.Value);
            }
        }

        return annos.OrderDescending().ToArray();


    }

}