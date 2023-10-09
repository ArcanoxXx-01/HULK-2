namespace HULK;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            System.Console.Write(">");
            Tokenizador a = new Tokenizador(System.Console.ReadLine());
            Parser b = new Parser(a.AllTokens);
        }
    }
}
