
namespace HULK;
public class Parser
{
    public List<Token> Tokens;

    //Para llevar la posicion de la lista
    private int actual = 0;

    public Parser(List<Token> tokens)
    {
        Tokens = tokens;
    }

    //Comprueba si el tipo del Token en el que estoy parado es uno de los que le paso como parametro
    private bool Match(TokenType[] types)
    {
        for (int i = 0; i < types.Length; i++)
        {
            if (Tokens[actual].Type == types[i])
            {
                return true;
            }
        }
        return false;
    }

    //Comprueba si el tipo del Token en el que estoy parado es el mismo del que le paso como parametro   
    private bool Match(TokenType type)
    {
        if (Tokens[actual].Type == type)
        {
            return true;
        }
        else return false;
    }

    //Consume el token actual y lo devuelve ademas aumenta el valor del current
    private Token Avanzar()
    {
        if (Tokens[actual].Type != TokenType.Final) actual++;
        return Anterior();
    }
    //Retorna el Token en la posicion anterior
    private Token Anterior()
    {
        return Tokens[actual - 1];
    }

    //Si el token en el que estoy parado es el esperado lo devuelvo y adelanto en uno la posicion, sino creo un error
    private Token Verificar(TokenType type, string mensaje)
    {
        if (Tokens[actual].Type == type) return Avanzar();

        throw new ERROR(ERROR.Tipo.SyntaxError, mensaje);
    }

    public Expresion Parsear()
    {
        while (Match(TokenType.function))
        {
            actual++;

            Token nombre = Verificar(TokenType.Identificador, "Identifier was expected as the function name in " + actual);

            string name = (string)nombre.Value;

            Verificar(TokenType.ParentesisAbierto, "Expected '(' after identifier " + Anterior().Value + " in " + actual);

            List<object> argument = new();

            while (Match(TokenType.Identificador))
            {
                argument.Add(Avanzar().Value);

                if (!Match(TokenType.ParentesisCerrado))
                {
                    Verificar(TokenType.Coma, "Expected ',' or ')' after expression " + Anterior().Type + " " + Anterior().Value + " in " + actual);
                }

            }

            Verificar(TokenType.ParentesisCerrado, "Missing ')' after the function arguments in " + actual);

            Verificar(TokenType.Flechita, "Expected '=>' in the function declaration in " + actual);

            if (Funciones.ContainsFuncion(name))
            {
                throw new ERROR(ERROR.Tipo.SemanticError, "Functions cannot be redefined");
            }

            Funciones.Reservadas(name);

            Expresion funcionCuerpo = Expression();

            Verificar(TokenType.PuntoYComa, " Expected ';' at the end of the expression after " + Anterior().Type + " " + Anterior().Value + " in " + actual);

            Expresion.Funcion expres = new(name, argument, funcionCuerpo);

            Funciones.Reservadas(name, expres);

            if (Match(TokenType.Final))
                return expres;

            else
            {
                throw new ERROR(ERROR.Tipo.SyntaxError, " Invalid expression after ';' in " + actual);
            }

        }

        Expresion expr = Expression();

        Verificar(TokenType.PuntoYComa, "Expected ';' at the end of the expression after " + Anterior().Type + " " + Anterior().Value + " in " + actual);

        if (Match(TokenType.Final))

            return expr;

        else

            throw new ERROR(ERROR.Tipo.SyntaxError, " Invalid espression after ';' in " + actual);

    }

    private Expresion Expression()
    {
        return Logica();
    }

    private Expresion Logica()
    {
        Expresion expr = Igualdad();

        TokenType[] a = { TokenType.And, TokenType.Or };

        while (Match(a))
        {
            Token operador = Avanzar();
            Expresion rigth = Igualdad();
            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }

        return expr;
    }

    private Expresion Igualdad()
    {
        Expresion expr = Comparacion();

        TokenType[] a = { TokenType.IgualIgual, TokenType.NoIgual };

        while (Match(a))
        {
            Token operador = Avanzar();
            Expresion rigth = Comparacion();
            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }

        return expr;
    }

    private Expresion Comparacion()
    {
        Expresion expr = Concatenar();

        TokenType[] a = { TokenType.Mayor, TokenType.MayorIgual, TokenType.Menor, TokenType.MenorIgual };

        while (Match(a))
        {
            Token operador = Avanzar();
            Expresion rigth = Concatenar();
            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }

        return expr;
    }

    private Expresion Concatenar()
    {
        Expresion expr = Suma_Resta();

        while (Match(TokenType.Concatenar))
        {
            Token operador = Avanzar();
            Expresion rigth = Suma_Resta();
            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }

        return expr;
    }

    private Expresion Suma_Resta()
    {
        Expresion expr = Multiplicacion_Division();

        TokenType[] a = { TokenType.Resta, TokenType.Suma };

        while (Match(a))
        {
            Token operador = Avanzar();
            Expresion rigth = Multiplicacion_Division();
            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }

        return expr;
    }

    private Expresion Multiplicacion_Division()
    {
        Expresion expr = Potencia();

        TokenType[] a = { TokenType.Division, TokenType.Multiplicacion };

        while (Match(a))
        {
            Token operador = Avanzar();
            Expresion rigth = Potencia();
            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }

        return expr;
    }

    private Expresion Potencia()
    {
        Expresion expr = Modulo();

        while (Match(TokenType.Pow))
        {
            Token operador = Avanzar();
            Expresion rigth = Modulo();
            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }

        return expr;
    }

    private Expresion Modulo()
    {
        Expresion expr = Unaria();

        while (Match(TokenType.Modulo))
        {
            Token operador = Avanzar();
            Expresion rigth = Unaria();
            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }

        return expr;
    }

    private Expresion Unaria()
    {
        TokenType[] a = { TokenType.Resta, TokenType.Negacion };

        while (Match(a))
        {
            Token operador = Avanzar();
            Expresion rigth = Unaria();
            return new Expresion.ExprUnaria(operador, rigth);
        }

        return If_Let();
    }

    private Expresion If_Let()
    {
        if (Match(TokenType.If))
        {
            actual++;

            Expresion condicion = Primaria();

            Expresion ifcuerpo = Expression();

            Verificar(TokenType.Else, " Expected else after if statement in " + actual);

            Expresion elsecuerpo = Expression();

            Expresion expr = new Expresion.If(condicion, ifcuerpo, elsecuerpo);

            return expr;
        }

        if (Match(TokenType.Let))
        {
            actual++;

            List<Expresion.ExprAsignar> letCuerpo = Asignacion();

            Verificar(TokenType.In, " Expected 'in' after let statement in " + actual);

            Expresion inCuerpo = Expression();

            return new Expresion.LetIn(letCuerpo, inCuerpo);
        }

        if (Match(TokenType.Identificador))
        {
            if (Funciones.ContainsFuncion(Tokens[actual].Value))
            {
                string name = (string)Avanzar().Value;

                Verificar(TokenType.ParentesisAbierto, " Expected '(' in " + actual);

                TokenType[] a = { TokenType.Coma, TokenType.ParentesisCerrado };

                List<Expresion> argument = new();

                while (!Match(a))
                {
                    Expresion expresion = Expression();

                    if (!Match(TokenType.ParentesisCerrado))
                    {
                        Verificar(TokenType.Coma, " Expected ',' or ')' after expression " + Anterior().Type + " " + Anterior().Value + " in " + actual);
                    }

                    argument.Add(expresion);
                }
                Verificar(TokenType.ParentesisCerrado, " Missing ')' after expression in " + actual);

                return new Expresion.ExprLLamadaFuncion(name, argument, Funciones.GetFuncion(name));
            }

            Expresion.ExprVariable expr = new(Avanzar());

            return expr;
        }

        return Primaria();
    }

    private List<Expresion.ExprAsignar> Asignacion()
    {
        List<Expresion.ExprAsignar> answer = new();

        while (!Match(TokenType.In))
        {
            Token name = new(TokenType.Identificador, "", "");

            if (Match(TokenType.Identificador))
            {
                name = Avanzar();
            }

            else throw new ERROR(ERROR.Tipo.SyntaxError, " Expect a variable name after " + Anterior().Type + " " + Anterior().Value + " in " + actual);

            Verificar(TokenType.Igual, " Expect '=' after variable name in " + actual);

            Expresion expr = Expression();

            if (!Match(TokenType.In))
            {
                Verificar(TokenType.Coma, " Expect ',' or 'in' after expression " + Anterior().Type + " " + Anterior().Value + " in " + actual);

                if (Match(TokenType.In)) throw new ERROR(ERROR.Tipo.LexicalError, " Invalid token 'in' after ',' in " + actual);

            }

            answer.Add(new Expresion.ExprAsignar(name, expr));
        }

        return answer;
    }

    private Expresion Primaria()
    {
        if (Match(TokenType.False))
        {
            actual++;

            return new Expresion.ExprLiteral(false);
        }

        if (Match(TokenType.True))
        {
            actual++;

            return new Expresion.ExprLiteral(true);
        }

        if (Match(TokenType.PI))
        {
            actual++;

            return new Expresion.ExprLiteral(Math.PI);
        }

        if (Match(TokenType.EULER))
        {
            actual++;

            return new Expresion.ExprLiteral(Math.E);
        }

        TokenType[] a = { TokenType.Number, TokenType.String };

        if (Match(a))
        {
            return new Expresion.ExprLiteral(Avanzar().Value);
        }

        if (Match(TokenType.ParentesisAbierto))
        {
            actual++;

            Expresion expr = Expression();

            Verificar(TokenType.ParentesisCerrado, " missing ')' after expresion in " + actual);
            return expr;
        }
        if (Tokens[actual].Type == TokenType.PuntoYComa)
        {
            Verificar(TokenType.PuntoYComa, " Invalid token ");
            return new Expresion.Vacia();
        }
        throw new ERROR(ERROR.Tipo.LexicalError, " Invalid token " + Tokens[actual].Type + " " + Tokens[actual].Value + " in " + actual);

    }

}