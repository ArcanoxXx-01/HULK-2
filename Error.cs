
namespace HULK;
public class ERROR : Exception
{
    public enum Tipo
    {
        LexicalError,
        SyntaxError,
        SemanticError,
    }

    public string mensaje;
    public Tipo type;

    public ERROR(Tipo type, string mensaje)
    {
        this.type = type;
        this.mensaje = mensaje;

        if (type != Tipo.LexicalError)
        {
            System.Console.WriteLine(type + mensaje);
        }
    }
}