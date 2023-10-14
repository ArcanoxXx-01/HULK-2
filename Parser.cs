namespace HULK;
public class Parser
{

    public List<Token> Tokens;
    private int current = 0;
    public List<ERROR> errores = new ();


    public Parser(List<Token> tokens)
    {
        Tokens = tokens;
        Parsear();
    }


    //Comprueba si el tipo del Token en el que estoy parado es uno de los que le paso como parametro
    private bool Match(TokenType[] types)
    {
        for (int i = 0; i < types.Length; i++)
        {
            if (Actual().Type == types[i])
            {
                return true;
            }
        }
        return false;
    }
        
    //Comprueba si el tipo del Token en el que estoy parado es el mismo del que le paso como parametro
    private bool Match(TokenType type)
    {
       return Actual().Type== type;
    }

    //Si el token en el que estoy parado es el esperado lo devuelvo y adelanto en uno la posicion, sino creo un error
    private Token Consume(TokenType type, string mensaje)
    {
        if (Actual().Type == type) return Siguiente();

        errores.Add(new ERROR(ERROR.ErrorType.SyntaxError, mensaje));

        return null!;
    }
    
    //Si no estoy en el final de la lista devuelvo el Token actual y aumento en uno la posicion
    private Token Siguiente()
    {
        if (Actual().Type != TokenType.Final) current++;
        return Anterior();
    }

    //Retorna el Token en la posicion anterior
    private Token Anterior()
    {
        return Tokens[current - 1];
    }
    
    //Retorna el token en la posicion actual
    private Token Actual()
    {
        return Tokens[current];
    }

    //Este metodo se encarga de recorrer la lista de tokens para crear la expresion que en caso de no tener errores  se va a evaluar
    public Expresion Parsear()
    {
        while (Match(TokenType.function))
        {
            current++;

            Token nombre = Consume(TokenType.Identificador, "An identifier was expected as the function name");
            
            string name = (string)nombre.Value;

            if (Funciones.ContainsFuncion(name))
            {
                errores.Add(new ERROR(ERROR.ErrorType.SyntaxError, "function " + name + " cannot be redefined"));
            }

            Consume(TokenType.ParentesisAbierto, "Expected '(' after expression " + Anterior().Type + " " + Anterior().Value);

            List<object> argument = new List<object>();

            while (Match(TokenType.Identificador))
            {
                argument.Add(Siguiente().Value);

                if (!Match(TokenType.ParentesisCerrado))
                {
                    Consume(TokenType.Coma, "Expected ',' before expression " + Actual().Type + " " + Actual().Value);
                }
            }

            Consume(TokenType.ParentesisCerrado, "Missing ')' after expression " + Anterior().Type + " " + Anterior().Value);

            Consume(TokenType.Flechita, "Expect '=>' to declare the function");

            Funciones.nullfunctions(name);

            Expresion funcionCuerpo = expression();
            // if (match(TokenType.PuntoYComa))

            Consume(TokenType.PuntoYComa, "Expected ';' at the end of expression declaration after " + Anterior().Type + " " + Anterior().Value);

            Expresion expres = new Expresion.Funcion(name, argument, funcionCuerpo);

            if (Match(TokenType.Final))
                return expres;

            errores.Add(new ERROR(ERROR.ErrorType.SyntaxError, "Expression was not declared correctly after " + Anterior().Type + " " + Anterior().Value));

            return null!;
        }
        Expresion expr = expression();

        Consume(TokenType.PuntoYComa, "Expected ';' at the end of expression declaration after " + Anterior().Type + " " + Anterior().Value);

        if (Match(TokenType.Final))

            return expr;

        else
            errores.Add(new ERROR(ERROR.ErrorType.SyntaxError, "Expression was not declared correctly after " + Anterior().Type + " " + Anterior().Value));


        return null!;
    }
    private Expresion expression()
    {
        return logical();
    }

    private Expresion logical()
    {
        Expresion expr = equality();

        TokenType[] a = { TokenType.And, TokenType.Or };

        while (Match(a))
        {
            Token operador = Siguiente();

            Expresion rigth = equality();

            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }

        return expr;
    }

    private Expresion equality()
    {

        Expresion expr = comparison();

        TokenType[] a = { TokenType.IgualIgual, TokenType.NoIgual };

        while (Match(a))
        {
            Token operador = Siguiente();

            Expresion rigth = comparison();

            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }

        return expr;
    }

    private Expresion comparison()
    {
        Expresion expr = concat();

        TokenType[] a = { TokenType.Mayor, TokenType.MayorIgual, TokenType.Menor, TokenType.MenorIgual };

        while (Match(a))
        {
            Token operador = Siguiente();

            Expresion rigth = concat();

            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }

        return expr;
    }

    private Expresion concat()
    {
        Expresion expr = Term();

        while (Match(TokenType.Concatenar))
        {
            Token operador = Siguiente();

            Expresion rigth = Term();

            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }

        return expr;
    }
    private Expresion Term()
    {
        Expresion expr = factor();

        TokenType[] a = { TokenType.Resta, TokenType.Suma };

        while (Match(a))
        {
            Token operador = Siguiente();

            Expresion rigth = factor();

            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }

        return expr;
    }
    private Expresion factor()
    {
        Expresion expr = pow();

        TokenType[] a = { TokenType.Division, TokenType.Multiplicacion };

        while (Match(a))
        {
            Token operador = Siguiente();

            Expresion rigth = pow();

            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }

        return expr;
    }
    private Expresion pow()
    {
        Expresion expr = mod();

        while (Match(TokenType.Pow))
        {
            Token operador = Siguiente();

            Expresion rigth = mod();

            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }

        return expr;
    }
    private Expresion mod()
    {
        Expresion expr = unary();

        while (Match(TokenType.Modulo))
        {
            Token operador = Siguiente();

            Expresion rigth = unary();

            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }

        return expr;
    }

    private Expresion unary()
    {
        TokenType[] a = { TokenType.Resta, TokenType.Negacion };

        while (Match(a))
        {
            Token operador = Siguiente();

            Expresion rigth = unary();

            return new Expresion.ExprUnaria(operador, rigth);
        }

        return IFORLET();
    }

    private Expresion IFORLET()
    {
        if (Match(TokenType.If))
        {
            current++;

            Expresion condicion = primary();

            Expresion ifcuerpo = expression();

            Consume(TokenType.Else, "Expect 'else' after if statement");

            Expresion elsecuerpo = expression();

            Expresion expr = new Expresion.If(condicion, ifcuerpo, elsecuerpo);

            return expr;
        }

        if (Match(TokenType.Let))
        {
            current++;

            List<Expresion.ExprAsignar> letCuerpo = Asign();

            Consume(TokenType.In, "Expected 'in' after arguments in let-in statement");

            Expresion inCuerpo = expression();

            return new Expresion.LetIn(letCuerpo, inCuerpo);
        }

        if (Match(TokenType.Identificador))
        {
            if (Funciones.ContainsFuncion(Actual().Value))
            {
                string name = (string)Siguiente().Value;

                Consume(TokenType.ParentesisAbierto, "Expected '(' after expression " + Anterior().Type + " " + Anterior().Value);

                TokenType[] a = { TokenType.Coma, TokenType.ParentesisCerrado };

                List<Expresion> argument = new ();

                while (!Match(a))
                {
                    Expresion expresion = expression();

                    if (!Match(TokenType.ParentesisCerrado))
                    {
                        Consume(TokenType.Coma, "Expected ',' before expression " + Actual().Type + " " + Actual().Value);
                    }

                    argument.Add(expresion);
                }

                Consume(TokenType.ParentesisCerrado, "Missing ')' after expression " + Anterior().Type + " " + Anterior().Value);

                Expresion.Funcion funcion = Funciones.GetFuncion(name);

                return new Expresion.ExprLLamadaFuncion(name, argument, funcion);
            }

            Expresion.ExprVariable expr = new (Siguiente());

            return expr;
        }

        return primary();
    }

    private List<Expresion.ExprAsignar> Asign()
    {
        List<Expresion.ExprAsignar> answer = new ();

        while (!Match(TokenType.In))
        {
            Token nombre = Siguiente();

            Consume(TokenType.Igual, "Expected '=' before expression" + Actual().Type + " " + Actual().Value + " in the let-in declaration");

            Expresion expr = expression();

            if (!Match(TokenType.In))

                Consume(TokenType.Coma, "Expected ',' before expression " + Actual().Type + " : " + Actual().Value);

            answer.Add(new Expresion.ExprAsignar(nombre, expr));
        }

        return answer;
    }

    private Expresion primary()
    {
        if (Match(TokenType.False))
        {
            current++;

            return new Expresion.ExprLiteral(false);
        }

        if (Match(TokenType.True))
        {
            current++;

            return new Expresion.ExprLiteral(true);
        }

        if (Match(TokenType.PI))
        {
            current++;

            return new Expresion.ExprLiteral(Math.PI);
        }

        if (Match(TokenType.EULER))
        {
            current++;

            return new Expresion.ExprLiteral(Math.E);
        }

        TokenType[] a = { TokenType.Number, TokenType.String };

        if (Match(a))
        {
            return new Expresion.ExprLiteral(Siguiente().Value);
        }

        if (Match(TokenType.ParentesisAbierto))
        {
            current++;

            Expresion expr = expression();

            Consume(TokenType.ParentesisCerrado, "Missing ) after expression " + Anterior().Type + " " + Anterior().Value);

            return expr;
        }
        if (Match(TokenType.Final)) return null!;
        errores.Add(new ERROR(ERROR.ErrorType.SyntaxError, "Invalid syntax in " + Actual().Type + " " + Actual().Value));
        return null!;
    }
}