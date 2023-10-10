namespace HULK;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Tokenizador a = new Tokenizador(System.Console.ReadLine());
            foreach (Token b in a.AllTokens)
            {
                System.Console.WriteLine("{0} : {1},",b.tipo,b.valor);
            }
        }  
    }
}
