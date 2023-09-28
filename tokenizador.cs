using System.Linq.Expressions;

namespace HULK;

public class Tokenizador
{
    public List<Token> AllTokens;
    public Tokenizador(string imput)
    {
        AllTokens = new List<Token>();
        CrearTokens(imput);
    }
    void CrearTokens(string a)
    {
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] == '(')
            {
                AllTokens.Add(new Token("parentesisAbierto", " "));
                continue;
            }
            if (a[i] == ')')
            {
                AllTokens.Add(new Token("parentesisCerrado", " "));
                continue;
            }
            if (a[i] == '=')
                if (i < a.Length - 1)
                {
                    if (a[i + 1] == '=')
                    {
                        AllTokens.Add(new Token("dobleIgual", " "));
                        i++;
                        continue;
                    }
                    else if (a[i + 1] == '>')
                    {
                        AllTokens.Add(new Token("flechita", " "));
                        i++;
                        continue;
                    }
                    else AllTokens.Add(new Token("igual", " "));
                    continue;
                }//AQUI TENGO Q PONER UNA EXCEPTION XQ EL SIGNO = SERIA EL ULTIMO TOKEN Y EL ULTIMO SIEMPRE TIENE Q SER ;
            if (a[i] == '!')
                if (i < a.Length - 1)
                {
                    if (a[i + 1] == '=')
                    {
                        AllTokens.Add(new Token("desigual", " "));
                        i++;
                        continue;
                    }
                    else AllTokens.Add(new Token("negacion", " "));
                    continue;
                }//AQUI TENGO Q PONER UNA EXCEPTION XQ EL SIGNO ! SERIA EL ULTIMO TOKEN Y EL ULTIMO SIEMPRE TIENE Q SER ;
            if (a[i] == '<')
                if (i < a.Length - 1)
                {
                    if (a[i + 1] == '=')
                    {
                        AllTokens.Add(new Token("menorIgual", " "));
                        i++;
                        continue;
                    }
                    else AllTokens.Add(new Token("menor", " "));
                    continue;
                }//AQUI TENGO Q PONER UNA EXCEPTION XQ EL SIGNO < SERIA EL ULTIMO TOKEN Y EL ULTIMO SIEMPRE TIENE Q SER ;
            if (a[i] == '>')
                if (i < a.Length - 1)
                {
                    {
                        if (a[i + 1] == '=')
                        {
                            AllTokens.Add(new Token("mayorIgual", " "));
                            i++;
                            continue;
                        }
                        else AllTokens.Add(new Token("mayor", " "));
                        continue;
                    }//AQUI TENGO Q PONER UNA EXCEPTION XQ EL SIGNO > SERIA EL ULTIMO TOKEN Y EL ULTIMO SIEMPRE TIENE Q SER ;            
                }
            if (a[i] == '+')
            {
                AllTokens.Add(new Token("mas", " "));
                continue;
            }
            if (a[i] == '-')
            {
                AllTokens.Add(new Token("menos", " "));
                continue;
            }
            if (a[i] == '*')
            {
                AllTokens.Add(new Token("por", " "));
                continue;
            }
            if (a[i] == '/')
            {
                AllTokens.Add(new Token("entre", " "));
                continue;
            }
            if (a[i] == '%')
            {
                AllTokens.Add(new Token("modulo", " "));
                continue;
            }
            if (a[i] == '.')
            {
                AllTokens.Add(new Token("punto", " "));
                continue;
            }
            if (a[i] == ',')
            {
                AllTokens.Add(new Token("coma", " "));
                continue;
            }
            if (a[i] == ';')
            {
                AllTokens.Add(new Token("puntoYcoma", " "));
                continue;
            }
            if (a[i] == '"')
            {
                if (i == a.Length - 1) { }//PONER EXCEPTION XQ " SERIA EL ULTIMO TOKEN Y NO ';'
                else
                {
                    string valor = "";
                    for (int j = i+1; j < a.Length; j++)
                    {
                        if (a[j] == '"')
                        {
                            AllTokens.Add(new Token("string", valor));
                            i=j;
                            break;
                        }
                        else
                        {
                            valor += a[j];
                        }
                    }
                    //PONER UNA EXCEPTION XQ NO ENCONTRO NUNCA LAS " PARA CERRAR EL STRING 
                }
            }
            if (a[i] == '0' || a[i] == '1' || a[i] == '2' || a[i] == '3' || a[i] == '4' || a[i] == '5' || a[i] == '6' || a[i] == '7' || a[i] == '8' || a[i] == '9')
            {
                string valor = "";
                int contadorDePuntos = 0;

                for (int j = i; j < a.Length; j++)
                {
                    if (a[j] == '0' || a[i] == '1' || a[i] == '2' || a[i] == '3' || a[i] == '4' || a[i] == '5' || a[i] == '6' || a[i] == '7' || a[i] == '8' || a[i] == '9')
                    {
                        valor += a[j];
                        continue;
                    }
                    else if (a[j] == '.')
                    {
                        contadorDePuntos++;
                        if (contadorDePuntos >= 2)
                        {
                            System.Console.WriteLine("ERROR");
                            //PONER UNA EXCEPTION XQ LOS NUMEROS NO PUEDEN TENER DOS O MAS PUNTOS
                        }
                        else
                        {
                            valor += a[j];
                            continue;
                        }
                    }
                    else if (a[j] == ' ' || a[j] == ';' || a[j] == ',' || a[j] == '+' || a[j] == '-' || a[j] == '*' || a[j] == '/' || a[j] == ')' || a[j] == '%')
                    {
                        AllTokens.Add(new Token("numero", valor));
                        i = j;
                        break;
                    }
                    else 
                    {
                        //crear una exception
                    }
                }
            }
        }
    }
}


