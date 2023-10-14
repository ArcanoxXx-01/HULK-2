namespace HULK;
class Program
{
  static void Main(string[] args)
  {

    Funciones.CrearFuncionesReservadas();
    while (true)
    {
      System.Console.Write(">");
      string input = Console.ReadLine();
      if (input == null!)
      {
        continue;
      }
      else Run(input);
    }

  }
  public static void Run(string input)
  {
    Lexer tokens = new (input);
    
    
    if(tokens.errores.Count == 0)
    {
      Parser parser = new (tokens.AllTokens);

      Expresion expresion = parser.Parsear();

      foreach (var error in parser.errores)
      {
        System.Console.WriteLine(error.Tipo + " : " + error.Mensaje);
      }
      Dictionary<object, object> xd = new ();
    
    }
    else
    {
      foreach (var error in tokens.errores)
      {
        System.Console.WriteLine(error.Tipo + " : " + error.Mensaje);
      }
    }
  }
}

