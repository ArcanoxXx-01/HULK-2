
namespace HULK;
public class Funciones
{
    private static Dictionary<string, Expresion.Funcion> funciones = new();

    public Funciones(string name, Expresion.Funcion funcion)
    {
        funciones.Add(name, funcion);
    }

    public static bool ContainsFuncion(object name)
    {
        foreach (var nombre in funciones)
        {
            if (funciones.ContainsKey((string)name))
                return true;
        }
        return false;
    }

    public static Expresion.Funcion GetFuncion(string name)
    {
        foreach (var nombre in funciones)
        {
            if (funciones.ContainsKey(name))
                return funciones[name];
        }
        throw new ERROR(ERROR.Tipo.SemanticError, " Function " + name + " is not defined");
    }

    public static void Reservadas(string name, Expresion.Funcion funcion = null!)
    {
        if (funciones.ContainsKey(name))
            funciones[name] = funcion;

        else funciones.Add(name, funcion);
    }

    public static void CrearReservadas()
    {
        Reservadas("print");
        Reservadas("sin");
        Reservadas("cos");
        Reservadas("sqrt");
        Reservadas("log");
    }

}