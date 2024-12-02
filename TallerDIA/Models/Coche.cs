namespace TallerDIA.Models;

public class Coche
{
    public enum Marcas
    {
        Renault,
        Hyundai,
        Bmw,
        Citroen,
        Ferrari,
        Lamborghini,
        Subaru
    }

    private string matricula;
    private Marcas marca;
    private string modelo;

    public string Matricula
    {
        get => matricula;
        set
        {
            matricula = value.ToUpper();
        }
    }
    public Marcas Marca
    {
        get => marca;
        set
        {
            marca = value;
        }
    }
    public string Modelo { get => modelo; set => modelo = value.ToUpper(); }

    /// <summary>
    /// Crea un coche con su matricula, su marca y su modelo.
    /// La marca a insertar tiene que ser uno de los valores que estan en el enumerado
    ///     Marcas de la propia clase.
    /// </summary>
    /// <param name="matricula"></param>
    /// <param name="marca"></param>
    /// <param name="modelo"></param>
    public Coche(string matricula, Marcas marca, string modelo)
    {
        Matricula = matricula.ToUpper();
        Marca = marca;
        Modelo = modelo.ToUpper();
    }


    /// <summary>
    /// Devuelve la informacion del coche en un string
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"Coche: [Matricula: {this.matricula}, Marca: {this.marca}, Modelo: {this.modelo}]";
    }



}