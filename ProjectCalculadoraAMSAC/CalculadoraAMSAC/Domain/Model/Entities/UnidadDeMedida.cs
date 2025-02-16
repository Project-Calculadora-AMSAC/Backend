namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

public class UnidadDeMedida
{
    public int Id { get; set; }
    public string Nombre {get; set;}
    public string Simbolo {get; set;}
    
    private UnidadDeMedida() { }

    public UnidadDeMedida(string nombre, string simbolo)
    {
        Nombre = nombre;
        Simbolo = simbolo;
    }
}