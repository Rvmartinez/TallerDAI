using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Linq;
namespace TallerDIA.Models;


public class GarajeCoches
{
    private ObservableCollection<Coche> coches;

    public ObservableCollection<Coche> Coches
    {
        get => coches;
    }
    public int Count => coches.Count;

    /// <summary>
    /// Crea un garaje de coches vacio
    /// </summary>
    public GarajeCoches()
    {
        coches = new ObservableCollection<Coche>();
    }
    /// <summary>
    /// Crea el garaje de coches a partir de una coleccion de coches pasada por argumento.
    /// </summary>
    /// <param name="coches"></param>
    public GarajeCoches(IEnumerable<Coche> c) : this()
    {
        foreach (Coche coche in c)
        {
            coches.Add(coche);
        }
    }

    /// <summary>
    /// Aumenta el garaje con la lista de coches pasados como argumento
    /// </summary>
    /// <param name="coches"></param>
    public void AddRange(IEnumerable<Coche> c)
    {
        foreach (Coche coche in c)
        {
            coches.Add(coche);
        }
    }

    /// <summary>
    /// Añade la instancia de Coche pasada como argumento al garaje de coches.
    /// </summary>
    /// <param name="coche"></param>
    public bool Add(Coche coche)
    {
        foreach (Coche c in coches)
        {
            if (c.Matricula == coche.Matricula)
            {
                return false;
            }
        }
        coches.Add(coche);
        return true;
    }


    /// <summary>
    /// Elimina la primera instancia del coche con la matricula asociada.
    /// En caso de encontrar la matricula elimina el coche con ella y devuelve true.
    /// En caso de no encontrar la matricula devuelve false.
    /// </summary>
    /// <param name="matricula"></param>
    public bool RemoveMatricula(string matricula)
    {
        bool borrado = false;
        var mat = matricula.ToUpper();
        for (int i = 0; i < coches.Count && borrado == false; i++)
        {
            if (coches[i].Matricula == mat)
            {
                coches.RemoveAt(i);
                borrado = true;
            }
        }
        return borrado;
    }

    /// <summary>
    /// Devuelve el coche que se encuentre en la i posición.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public Coche Get(int i)
    {
        return coches[i];
    }

    /// <summary>
    /// Devuelve el coche con la matricula que se pasa por parametro, en caso de no encontrarlo devuelve null.
    /// </summary>
    /// <param name="matricula"></param>
    /// <returns></returns>
    public Coche GetMatricula(string matricula)
    {
        Coche c = null;
        bool encontrado = false;
        var mat = matricula.ToUpper();
        for (int i = 0; i < coches.Count && encontrado == false; i++)
        {
            if (coches[i].Matricula == mat)
            {
                c = coches[i];
                encontrado = true;
            }
        }
        return c;
    }

    public bool ModificarMatricula(string matAnt, string matNueva)
    {
        bool encontrado = false;
        var matAntigua = matAnt.ToUpper();
        var matNuev = matNueva.ToUpper();
        for (int i = 0; i < coches.Count && encontrado == false; i++)
        {
            if (coches[i].Matricula == matAntigua)
            {
                coches[i].Matricula = matNueva;
                /*Coche c = coches[i];
                coches.RemoveAt(i);
                coches.Add(new Coche(matNuev, c.Marca, c.Modelo));*/
                encontrado = true;
            }
        }

        return encontrado;
    }

    /// <summary>
    /// Devuelve un XElement raiz donde se guarda toda la informacion de los coches del garaje y el numero de coches en el garaje
    /// </summary>
    /// <returns></returns>
    public XElement ToXML()
    {
        XElement raiz = new XElement("garaje");

        raiz.Add(new XAttribute("count", Count));

        foreach (var coche in this.coches)
        {
            XElement nodoCoche = new XElement("coche");
            XElement mat = new XElement("matricula", coche.Matricula);
            XElement marca = new XElement("marca", coche.Marca);
            XElement modelo = new XElement("modelo", coche.Modelo);

            nodoCoche.Add(mat);
            nodoCoche.Add(marca);
            nodoCoche.Add(modelo);

            raiz.Add(nodoCoche);
        }

        return raiz;
    }


    /// <summary>
    /// Devuelve un string con la informacion de cada coche en el garaje, cada coche en una linea.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        StringBuilder toret = new StringBuilder();
        toret.Append("Numero de coches en el garaje: ");
        toret.Append(Count);
        toret.Append("\n");
        foreach (var coche in this.coches)
        {
            toret.Append(coche.ToString());
            toret.Append("\n");
        }
        return toret.ToString();
    }

}