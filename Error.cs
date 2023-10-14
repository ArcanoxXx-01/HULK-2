
namespace HULK;
public class ERROR
{
    public enum ErrorType
    {
        LexicalError,
        SyntaxError,
        SemanticError,
    }

    public ErrorType Tipo;
    public string Mensaje;

    public ERROR(ErrorType tipo, string mensaje)
    {
        Tipo = tipo;
        Mensaje = mensaje;
    }

}