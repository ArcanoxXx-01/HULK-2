namespace HULK;

    class Program
    {
        static void Main(string[] args)
        {
            string a =System.Console.ReadLine();
            Tokenizador Monga = new Tokenizador(a);
            foreach (var b in Monga.AllTokens)
            {
                System.Console.WriteLine("{0}:{1}",b.tipo,b.valor);
            }
        }
    }
