
namespace HULK;

public class Lexer
{
    public List<Token> AllTokens;
    public List<ERROR> errores;

    public Lexer(string entrada)
    {
        AllTokens = new List<Token>();

        errores = new List<ERROR>();

        MakeTokens(entrada);
    }

    public void MakeTokens(string x)
    {
        for (int i = 0; i < x.Length; i++)
        {
            if (x[i] == ' ') continue;

            if (x[i] == ';')
            {
                AllTokens.Add(new Token(TokenType.PuntoYComa, "Separadores", ""));
                continue;
            }

            if (x[i] == '@')
            {
                AllTokens.Add(new Token(TokenType.Concatenar, "Operadores", ""));
                continue;
            }

            if (x[i] == '(')
            {
                AllTokens.Add(new Token(TokenType.ParentesisAbierto, "Separadores", ""));
                continue;
            }

            if (x[i] == ')')
            {
                AllTokens.Add(new Token(TokenType.ParentesisCerrado, "Separadores", ""));
                continue;
            }

            if (x[i] == '*')
            {
                AllTokens.Add(new Token(TokenType.Multiplicacion, "Operadores", ""));
                continue;
            }

            if (x[i] == '/')
            {
                AllTokens.Add(new Token(TokenType.Division, "Operadores", ""));
                continue;
            }

            if (x[i] == '%' && (i != x.Length - 1))
            {
                AllTokens.Add(new Token(TokenType.Modulo, "Operadores", ""));
                continue;
            }

            if (x[i] == '+')
            {
                AllTokens.Add(new Token(TokenType.Suma, "Operadores", ""));
                continue;
            }

            if (x[i] == '-')
            {
                AllTokens.Add(new Token(TokenType.Resta, "Operadores", ""));
                continue;
            }

            if (x[i] == ',')
            {
                AllTokens.Add(new Token(TokenType.Coma, "Separadores", ""));
                continue;
            }

            if (x[i] == '^')
            {
                AllTokens.Add(new Token(TokenType.Pow, "Operadores", ""));
                continue;
            }

            if (x[i] == '!')
            {
                if (x[i + 1] == '=')
                {
                    AllTokens.Add(new Token(TokenType.NoIgual, "Comparacion", ""));
                    i++;
                    continue;
                }

                else AllTokens.Add(new Token(TokenType.Negacion, "OperadorBooleano", ""));
                continue;
            }

            if (x[i] == '|')
            {
                if (x[i + 1] == '|')
                {
                    AllTokens.Add(new Token(TokenType.Or, "OperadorBooleano", ""));
                    i++;
                    continue;
                }

                else
                {
                    ERROR error = new(ERROR.Tipo.LexicalError, x[i] + " is not a valid token");
                    errores.Add(error);
                    continue;
                }
            }

            if (x[i] == '&')
            {
                if (x[i + 1] == '&')
                {
                    AllTokens.Add(new Token(TokenType.And, "OperadorBooleano", ""));
                    i++;
                    continue;
                }

                else
                {
                    ERROR error = new(ERROR.Tipo.LexicalError, x[i] + " is not a valid token");
                    continue;
                }
            }

            if (x[i] == '>')
            {
                if (x[i + 1] == '=')
                {
                    AllTokens.Add(new Token(TokenType.MayorIgual, "Comparacion", ""));
                    i++;
                    continue;
                }

                else
                {
                    AllTokens.Add(new Token(TokenType.Mayor, "Comparacion", ""));
                    continue;
                }
            }

            if (x[i] == '<')
            {
                if (x[i + 1] == '=')
                {
                    AllTokens.Add(new Token(TokenType.MenorIgual, "Comparacion", ""));
                    i++;
                    continue;
                }

                else
                {
                    AllTokens.Add(new Token(TokenType.Menor, "Comparacion", ""));
                    continue;
                }
            }

            if (x[i] == '=')
            {
                if (x[i + 1] == '=')
                {
                    AllTokens.Add(new Token(TokenType.IgualIgual, "Comparacion", ""));
                    i++;
                    continue;
                }

                if (x[i + 1] == '>')
                {
                    AllTokens.Add(new Token(TokenType.Flechita, "OperadorFunciones", ""));
                    i++;
                    continue;
                }

                else
                {
                    AllTokens.Add(new Token(TokenType.Igual, "Operadores", ""));
                    continue;
                }
            }

            if (x[i] == '"')
            {
                string a = "";
                
                for (int j = i + 1; j < x.Length; j++)
                {

                    if (x[j] == '"')
                    {
                        i = j;
                        break;
                    }

                    else
                    {
                        a += x[j];
                    }

                    if (j == x.Length - 1 && x[j] != '"')
                    {
                        ERROR error = new(ERROR.Tipo.LexicalError, "String " + a + " was declared incorrectly");
                        errores.Add(error);
                    }

                    continue;
                }

                AllTokens.Add(new Token(TokenType.String, "Variables", a));
                continue;
            }

            if (x[i] == '0' || x[i] == '1' || x[i] == '2' || x[i] == '3' || x[i] == '4' || x[i] == '5' || x[i] == '6' || x[i] == '7' || x[i] == '8' || x[i] == '9')
            {
                string numero = "";
                numero += x[i];
                int ContadorDePuntos = 0;
                bool valido = true;

                for (int j = i + 1; j < x.Length; j++)
                {

                    if (x[j] == '0' || x[j] == '1' || x[j] == '2' || x[j] == '3' || x[j] == '4' || x[j] == '5' || x[j] == '6' || x[j] == '7' || x[j] == '8' || x[j] == '9')
                    {
                        numero += x[j];
                        i = j;
                        continue;
                    }

                    if (x[j] == '.')
                    {
                        numero += x[j];
                        i = j;
                        ContadorDePuntos++;
                        continue;
                    }

                    if (x[j] == 'i')
                    {
                        if (x[j + 1] == 'n')
                        {
                            i = j;
                            break;
                        }

                        numero += x[j];
                        valido = false;
                    }

                    if (x[j] == '_' || x[j] == 'A' || x[j] == 'a' || x[j] == 'B' || x[j] == 'b' || x[j] == 'C' || x[j] == 'c' || x[j] == 'D' || x[j] == 'd' || x[j] == 'E' || x[j] == 'e' || x[j] == 'F' || x[j] == 'f' || x[j] == 'G' || x[j] == 'g' || x[j] == 'H' || x[j] == 'h' || x[j] == 'I' || x[j] == 'J' || x[j] == 'j' || x[j] == 'K' || x[j] == 'k' || x[j] == 'L' || x[j] == 'l' || x[j] == 'M' || x[j] == 'm' || x[j] == 'N' || x[j] == 'n' || x[j] == 'O' || x[j] == 'o' || x[j] == 'P' || x[j] == 'p' || x[j] == 'Q' || x[j] == 'q' || x[j] == 'R' || x[j] == 'r' || x[j] == 'S' || x[j] == 's' || x[j] == 'T' || x[j] == 't' || x[j] == 'U' || x[j] == 'u' || x[j] == 'V' || x[j] == 'v' || x[j] == 'W' || x[j] == 'W' || x[j] == 'X' || x[j] == 'x' || x[j] == 'Y' || x[j] == 'y' || x[j] == 'Z' || x[j] == 'z')
                    {
                        valido = false;
                        numero += x[j];
                        i = j;
                        continue;
                    }

                    break;
                }

                if (valido == false)
                {
                    ERROR error = new(ERROR.Tipo.LexicalError, numero + " is not a valid token");
                    errores.Add(error);
                    continue;
                }

                if (ContadorDePuntos > 1)
                {
                    ERROR error = new(ERROR.Tipo.LexicalError, "Number " + numero + " was declared incorrectly");
                    errores.Add(error);
                    continue;
                }

                AllTokens.Add(new Token(TokenType.Number, "Variables", double.Parse(numero)));
                continue;
            }

            if (x[i] == 'A' || x[i] == 'a' || x[i] == 'B' || x[i] == 'b' || x[i] == 'C' || x[i] == 'c' || x[i] == 'D' || x[i] == 'd' || x[i] == 'E' || x[i] == 'e' || x[i] == 'F' || x[i] == 'f' || x[i] == 'G' || x[i] == 'g' || x[i] == 'H' || x[i] == 'h' || x[i] == 'I' || x[i] == 'i' || x[i] == 'J' || x[i] == 'j' || x[i] == 'K' || x[i] == 'k' || x[i] == 'L' || x[i] == 'l' || x[i] == 'M' || x[i] == 'm' || x[i] == 'N' || x[i] == 'n' || x[i] == 'O' || x[i] == 'o' || x[i] == 'P' || x[i] == 'p' || x[i] == 'Q' || x[i] == 'q' || x[i] == 'R' || x[i] == 'r' || x[i] == 'S' || x[i] == 's' || x[i] == 'T' || x[i] == 't' || x[i] == 'U' || x[i] == 'u' || x[i] == 'V' || x[i] == 'v' || x[i] == 'W' || x[i] == 'W' || x[i] == 'X' || x[i] == 'x' || x[i] == 'Y' || x[i] == 'y' || x[i] == 'Z' || x[i] == 'z')
            {
                string a = "";
                a += x[i];

                for (int j = i + 1; j < x.Length; j++)
                {
                    if (x[j] == '_' || x[j] == 'A' || x[j] == 'a' || x[j] == 'B' || x[j] == 'b' || x[j] == 'C' || x[j] == 'c' || x[j] == 'D' || x[j] == 'd' || x[j] == 'E' || x[j] == 'e' || x[j] == 'F' || x[j] == 'f' || x[j] == 'G' || x[j] == 'g' || x[j] == 'H' || x[j] == 'h' || x[j] == 'I' || x[j] == 'i' || x[j] == 'J' || x[j] == 'j' || x[j] == 'K' || x[j] == 'k' || x[j] == 'L' || x[j] == 'l' || x[j] == 'M' || x[j] == 'm' || x[j] == 'N' || x[j] == 'n' || x[j] == 'O' || x[j] == 'o' || x[j] == 'P' || x[j] == 'p' || x[j] == 'Q' || x[j] == 'q' || x[j] == 'R' || x[j] == 'r' || x[j] == 'S' || x[j] == 's' || x[j] == 'T' || x[j] == 't' || x[j] == 'U' || x[j] == 'u' || x[j] == 'V' || x[j] == 'v' || x[j] == 'W' || x[j] == 'W' || x[j] == 'X' || x[j] == 'x' || x[j] == 'Y' || x[j] == 'y' || x[j] == 'Z' || x[j] == 'z' || x[j] == '0' || x[j] == '1' || x[j] == '2' || x[j] == '3' || x[j] == '4' || x[j] == '5' || x[j] == '6' || x[j] == '7' || x[j] == '8' || x[j] == '9')
                    {
                        a += x[j];
                        i = j;
                        continue;
                    }

                    break;
                }

                if (a == "true")
                {
                    AllTokens.Add(new Token(TokenType.True, "Variable", ""));
                    continue;
                }

                if (a == "false")
                {
                    AllTokens.Add(new Token(TokenType.False, "Variable", ""));
                    continue;
                }

                if (a == "PI")
                {
                    AllTokens.Add(new Token(TokenType.PI, "Constante", Math.PI));
                    continue;
                }

                if (a == "EULER")
                {
                    AllTokens.Add(new Token(TokenType.EULER, "Constantes", Math.E));
                    continue;
                }

                if (a == "if")
                {
                    AllTokens.Add(new Token(TokenType.If, "Palabras Reservadas", ""));
                    continue;
                }

                if (a == "else")
                {
                    AllTokens.Add(new Token(TokenType.Else, "Palabras Reservadas", ""));
                    continue;
                }

                if (a == "let")
                {
                    AllTokens.Add(new Token(TokenType.Let, "Palabras Reservadas", ""));
                    continue;
                }

                if (a == "in")
                {
                    AllTokens.Add(new Token(TokenType.In, "Palabras Reservadas", ""));
                    continue;
                }

                if (a == "function")
                {
                    AllTokens.Add(new Token(TokenType.function, "Palabras Reservadas", ""));
                    continue;
                }

                AllTokens.Add(new Token(TokenType.Identificador, "Variable", a));
                continue;
            }

            errores.Add(new(ERROR.Tipo.LexicalError, x[i] + " is not a valid token"));
        }
        
        AllTokens.Add(new Token(TokenType.Final, "", ""));
        if(AllTokens.Count<3)
        {
            errores.Add(new (ERROR.Tipo.LexicalError,"The expression entered is not defined in Hulk"));
        }
    }
}