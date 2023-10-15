namespace HULK;
class Program
{
  static void Main(string[] args)
  {

    Funciones.CrearFuncionesReservadas();
    while (true)
    {
      System.Console.Write(">");
      string input = Console.ReadLine()!;
      if (input == null!)
      {
        Console.WriteLine("Se ha ingresado una linea vacia");
        break;
      }
      else Run(input);
    }

  }
  public static void Run(string input)
  {
    Lexer tokens = new (input);
    /*foreach (var token in tokens.Tokens)
    {
      Console.WriteLine(token.Type + " " + token.Grupo + " " + token.Value);
    }*/

    if (tokens.errores.Count != 0)
    {
      foreach (var error in tokens.errores)
      {
        System.Console.WriteLine(error.Tipo + " " + error.Mensaje);
      }
    }

    else
    {
      Parser parser = new (tokens.AllTokens);

      Expresion expresion = parser.Parsear();

      if (ERROR.hadError == true) return;

      Dictionary<object, object> xd = new ();

      Evaluador evaluador = new (expresion);

      object respuesta = evaluador.Run(expresion, xd);

      if(ERROR.hadError==true) return;

      System.Console.WriteLine(respuesta);

    }
  }
}
