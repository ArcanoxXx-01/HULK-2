namespace HULK;
public class Funciones
{
    public static Dictionary<string, Expresion.Funcion> funciones = new();
    
    public Funciones(string name, Expresion.Funcion funcion)
    {
        funciones.Add(name, funcion);
    }
    
    public static bool ContainsFuncion(object name)
    {
        foreach (var nombre in funciones)
        {
            if (funciones.ContainsKey((string)name))return true;
        }
        return false;
    }
    
    public static Expresion.Funcion GetFuncion(string name)
    {
        foreach (var nombre in funciones)
        {
            if (funciones.ContainsKey(name))return funciones[name];
        }
        throw new Exception("Error mango");
    }
    
    public static void FuncionReservada(string name)
    {
        Expresion.Funcion funcion=new();
        funciones.Add(name, funcion);
    }
    
    public static void CrearFuncionesReservadas()
    {
        FuncionReservada("print");
        FuncionReservada("cos");
        FuncionReservada("sen");
        FuncionReservada("sqrt");
        FuncionReservada("log");
    } 
}