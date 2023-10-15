namespace HULK;
public class ERROR : Exception
{
    public enum ErrorType
    {
        LexicalError,
        SyntaxError,
        SemanticError,
    }

    public string Mensaje;
    public ErrorType Tipo;
    public static bool hadError = false;

    public ERROR(ErrorType type, string mensaje)
    {
        Tipo = type;
        Mensaje = mensaje;
        hadError = true;
        if (type == ErrorType.SyntaxError)
        {
            System.Console.WriteLine(type + mensaje);
        }

        if (type == ErrorType.SemanticError)
        {
            System.Console.WriteLine(type + mensaje);
        }
    }
    public ERROR(string mensaje)
    {
        Mensaje = mensaje;
        hadError = true;
    }
}