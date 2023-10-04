namespace HULK;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            string input = System.Console.ReadLine();
            Tokenizador tokens = new Tokenizador(input);
            Parser parser = new Parser(tokens.AllTokens);
        }
    }
}
