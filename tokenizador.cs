using System.Globalization;
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
            if (a[i] == ' ') continue;
            if (a[i] == '(')
            {
                AllTokens.Add(new Token(TokenType.parentesisAbierto, "separador"));
                continue;
            }
            if (a[i] == ')')
            {
                AllTokens.Add(new Token(TokenType.parentesisCerrado, "separador"));
                continue;
            }
            if (a[i] == '@')
            {
                AllTokens.Add(new Token(TokenType.Concatenar, "operador"));
                continue;
            }
            if (a[i] == '=')
            {
                if (i < a.Length - 1)
                {
                    if (a[i + 1] == '=')
                    {
                        AllTokens.Add(new Token(TokenType.dobleIgual, "comparador"));
                        i++;
                        continue;
                    }
                    else if (a[i + 1] == '>')
                    {
                        AllTokens.Add(new Token(TokenType.flechita, "comparador"));
                        i++;
                        continue;
                    }
                    else AllTokens.Add(new Token(TokenType.igual, "comparador"));
                    continue;
                }//AQUI TENGO Q PONER UNA EXCEPTION XQ EL SIGNO = SERIA EL ULTIMO TOKEN Y EL ULTIMO SIEMPRE TIENE Q SER ;
            }
            if (a[i] == '!')
            {
                if (i < a.Length - 1)
                {
                    if (a[i + 1] == '=')
                    {
                        AllTokens.Add(new Token(TokenType.desigual, "comparador"));
                        i++;
                        continue;
                    }
                    else AllTokens.Add(new Token(TokenType.negacion, "comparador"));
                    continue;
                }//AQUI TENGO Q PONER UNA EXCEPTION XQ EL SIGNO ! SERIA EL ULTIMO TOKEN Y EL ULTIMO SIEMPRE TIENE Q SER ;
            }
            if (a[i] == '<')
            {
                if (i < a.Length - 1)
                {
                    if (a[i + 1] == '=')
                    {
                        AllTokens.Add(new Token(TokenType.menorIgual, "comparador"));
                        i++;
                        continue;
                    }
                    else AllTokens.Add(new Token(TokenType.menor, "comparador"));
                    continue;
                }
                //AQUI TENGO Q PONER UNA EXCEPTION XQ EL SIGNO < SERIA EL ULTIMO TOKEN Y EL ULTIMO SIEMPRE TIENE Q SER ;
            }
            if (a[i] == '>')
                if (i < a.Length - 1)
                {
                    {
                        if (a[i + 1] == '=')
                        {
                            AllTokens.Add(new Token(TokenType.mayorIgual, "comaprador"));
                            i++;
                            continue;
                        }
                        else AllTokens.Add(new Token(TokenType.mayor, "comparador"));
                        continue;
                    }//AQUI TENGO Q PONER UNA EXCEPTION XQ EL SIGNO > SERIA EL ULTIMO TOKEN Y EL ULTIMO SIEMPRE TIENE Q SER ;            
                }
            if(a[i]=='|'&& i<a.Length-1)
            {
                if(a[i+1]=='|')
                {
                    AllTokens.Add(new Token(TokenType.or,""));
                    i++;
                    continue;
                }
                //PONER ERROR '|' NO ES UNA EXPRESION VALIDA
            }
            if(a[i]=='|')//Poner error xq ; no seria el ultimo token   
            if(a[i]=='&'&& i<a.Length-1)
            {
                if(a[i+1]=='&')
                {
                    AllTokens.Add(new Token(TokenType.and,""));
                    i++;
                    continue;
                }
                //PONER ERROR '|' NO ES UNA EXPRESION VALIDA
            }
            if(a[i]=='&')//Poner error xq ; no seria el ultimo token              
            if (a[i] == '+')
            {
                AllTokens.Add(new Token(TokenType.suma, "operador"));
                continue;
            }
            if (a[i] == '-')
            {
                AllTokens.Add(new Token(TokenType.resta, "operador"));
                continue;
            }
            if (a[i] == '*')
            {
                AllTokens.Add(new Token(TokenType.multiplicacion, "operador"));
                continue;
            }
            if (a[i] == '/')
            {
                AllTokens.Add(new Token(TokenType.division, "operador"));
                continue;
            }
            if (a[i] == '%')
            {
                AllTokens.Add(new Token(TokenType.modulo, "operador"));
                continue;
            }
            if (a[i] == '^')
            {
                AllTokens.Add(new Token(TokenType.Pow, "Operadores"));
                continue;
            }            
            if (a[i] == ',')
            {
                AllTokens.Add(new Token(TokenType.coma, "separador"));
                continue;
            }
            if (a[i] == ';')
            {
                AllTokens.Add(new Token(TokenType.puntoYcoma, "separador"));
                continue;
            }
            if (a[i] == '"')
            {
                if (i == a.Length - 1)
                {
                    //PONER EXCEPTION XQ " SERIA EL ULTIMO TOKEN Y NO ';'
                }
                else
                {
                    string valor = "";
                    for (int j = i + 1; j < a.Length; j++)
                    {
                        if (a[j] == '"')
                        {
                            AllTokens.Add(new Token(TokenType.String, valor));
                            i = j;
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
                if (i == a.Length - 1)
                {
                    //PONER UNA EXCEPTION
                }
                else
                {
                    string valor = "";
                    int contadorDePuntos = 0;
                    valor += a[i];

                    for (int j = i + 1; j < a.Length; j++)
                    {
                        if (j == a.Length - 1)
                        {
                            if (a[j] == ' ' || a[j] == '+' || a[j] == '-' || a[j] == '*' || a[j] == '/' || a[j] == ')' || a[j] == '=' || a[j] == ',' || a[j] == ';')
                            {
                                AllTokens.Add(new Token(TokenType.numero, valor));
                                i = j - 1;
                                continue;
                            }
                            else
                            {
                                //Exception
                            }
                        }
                        if (a[j] == '0' || a[j] == '1' || a[j] == '2' || a[j] == '3' || a[j] == '4' || a[j] == '5' || a[j] == '6' || a[j] == '7' || a[j] == '8' || a[j] == '9')
                        {
                            valor += a[j];
                            i = j;
                            continue;
                        }
                        else if (a[j] == '.')
                        {
                            contadorDePuntos++;
                            if (contadorDePuntos > 1)
                            {
                            }
                            valor += '.';
                        }
                        else if (a[j] == ' ' || a[j] == '+' || a[j] == '-' || a[j] == '*' || a[j] == '/' || a[j] == ')' || a[j] == '=' || a[j] == ',' || a[j] == ';')
                        {
                            AllTokens.Add(new Token(TokenType.numero, valor));
                            i = j - 1; break;
                        }
                    }
                }
            }
            if (a[i] == 'a' || a[i] == 'A' || a[i] == 'b' || a[i] == 'B' || a[i] == 'c' || a[i] == 'C' || a[i] == 'd' || a[i] == 'D' || a[i] == 'e' || a[i] == 'E' || a[i] == 'f' || a[i] == 'F' || a[i] == 'g' || a[i] == 'G' || a[i] == 'h' || a[i] == 'H' || a[i] == 'i' || a[i] == 'I' || a[i] == 'j' || a[i] == 'J' || a[i] == 'k' || a[i] == 'K' || a[i] == 'l' || a[i] == 'L' || a[i] == 'm' || a[i] == 'M' || a[i] == 'n' || a[i] == 'N' || a[i] == 'o' || a[i] == 'O' || a[i] == 'p' || a[i] == 'P' || a[i] == 'q' || a[i] == 'Q' || a[i] == 'r' || a[i] == 'R' || a[i] == 's' || a[i] == 'S' || a[i] == 't' || a[i] == 'T' || a[i] == 'u' || a[i] == 'U' || a[i] == 'v' || a[i] == 'V' || a[i] == 'w' || a[i] == 'W' || a[i] == 'x' || a[i] == 'X' || a[i] == 'y' || a[i] == 'Y' || a[i] == 'z' || a[i] == 'Z' || a[i] == '_')
            {
                if (i < a.Length - 1)
                {
                    string valor = "";
                    valor += a[i];

                    for (int j = i + 1; j < a.Length; j++)
                    {
                        if (a[j] == 'a' || a[j] == 'A' || a[j] == 'b' || a[j] == 'B' || a[j] == 'c' || a[j] == 'C' || a[j] == 'd' || a[j] == 'D' || a[j] == 'e' || a[j] == 'E' || a[j] == 'f' || a[j] == 'F' || a[j] == 'g' || a[j] == 'G' || a[j] == 'h' || a[j] == 'H' || a[j] == 'i' || a[j] == 'I' || a[j] == 'j' || a[j] == 'J' || a[j] == 'k' || a[j] == 'K' || a[j] == 'l' || a[j] == 'L' || a[j] == 'm' || a[j] == 'M' || a[j] == 'n' || a[j] == 'N' || a[j] == 'o' || a[j] == 'O' || a[j] == 'p' || a[j] == 'P' || a[j] == 'q' || a[j] == 'Q' || a[j] == 'r' || a[j] == 'R' || a[j] == 's' || a[j] == 'S' || a[j] == 't' || a[j] == 'T' || a[j] == 'u' || a[j] == 'U' || a[j] == 'v' || a[j] == 'V' || a[j] == 'w' || a[j] == 'W' || a[j] == 'x' || a[j] == 'X' || a[j] == 'y' || a[j] == 'Y' || a[j] == 'z' || a[j] == 'Z' || a[j] == '_' || a[j] == '0' || a[j] == '1' || a[j] == '2' || a[j] == '3' || a[j] == '4' || a[j] == '5' || a[j] == '6' || a[j] == '7' || a[j] == '8' || a[j] == '9')
                        {
                            valor += a[j];
                        }
                        else if (a[j] == ' ' || a[j] == ';' || a[j] == '(' || a[j] == ')' || a[j] == '+' || a[j] == '-' || a[j] == '*' || a[j] == '/' || a[j] == ',' || a[j] == '=')
                        {
                            if (valor == "print") AllTokens.Add(new Token(TokenType.print, ""));
                            else if (valor == "funtion") AllTokens.Add(new Token(TokenType.funtion, ""));
                            else if (valor == "let") AllTokens.Add(new Token(TokenType.Let, ""));
                            else if (valor == "in") AllTokens.Add(new Token(TokenType.In, ""));
                            else if (valor == "True") AllTokens.Add(new Token(TokenType.True, ""));
                            else if (valor == "False") AllTokens.Add(new Token(TokenType.False, ""));
                            else if (valor == "PI") AllTokens.Add(new Token(TokenType.PI, "constante"));
                            else if (valor == "Euler") AllTokens.Add(new Token(TokenType.Euler, "constante"));
                            else { AllTokens.Add(new Token(TokenType.identificador, valor)); }
                            i = j - 1;
                            break;
                        }
                        else
                        {
                            //CREAR EXCEPTION UN ID NO PUEDE TENER CARACTERES DIFERENTES DE LETRAS ,NUMEROS O BARRA BAJA
                        }
                    }
                }
            }
        }
    }
}