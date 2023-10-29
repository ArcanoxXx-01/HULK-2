
namespace HULK;
class Program
{
  static void Main(string[] args)
  {
    Funciones.CrearReservadas();

    while (true)
    {
      Console.Write(">");

      string input = Console.ReadLine()!;

      if (input.Length == 0)
      {
        Console.WriteLine("An empty line has been entered");
      }
      else
      {
        Run(input);
      }

    }
  }
  public static void Run(string input)
  {
    Lexer tokens = new(input);

    if (tokens.errores.Count != 0)
    {
      foreach (var error in tokens.errores)
      {
        System.Console.WriteLine(error.type + " : " + error.mensaje);
      }
    }
    else
    {
      Parser parser = new(tokens.AllTokens);

      Expresion expresion = parser.Parsear();

      Dictionary<object, object> value = new();

      Evaluador evaluador = new(expresion);

      object respuesta = evaluador.Run(expresion, value);

      if(Evaluador.errores.Count==0)Console.WriteLine(respuesta);


    }
  }
}
