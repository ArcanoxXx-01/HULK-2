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

            Funciones.Nullfunctions(name);

            Expresion funcionCuerpo = Expression();
            // if (match(TokenType.PuntoYComa))

            Consume(TokenType.PuntoYComa, "Expected ';' at the end of expression declaration after " + Anterior().Type + " " + Anterior().Value);

            Expresion expres = new Expresion.Funcion(name, argument, funcionCuerpo);

            if (Match(TokenType.Final))
                return expres;

            errores.Add(new ERROR(ERROR.ErrorType.SyntaxError, "Expression was not declared correctly after " + Anterior().Type + " " + Anterior().Value));

            return null!;
        }
        Expresion expr = Expression();

        Consume(TokenType.PuntoYComa, "Expected ';' at the end of expression declaration after " + Anterior().Type + " " + Anterior().Value);

        if (Match(TokenType.Final))

            return expr;

        else
            errores.Add(new ERROR(ERROR.ErrorType.SyntaxError, "Expression was not declared correctly after " + Anterior().Type + " " + Anterior().Value));


        return null!;
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
            Token operador = Siguiente();

            Expresion rigth = Igualdad();

            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }

        return expr;
    }

    private Expresion Igualdad()
    {

        Expresion expr = Comparasion();

        TokenType[] a = { TokenType.IgualIgual, TokenType.NoIgual };

        while (Match(a))
        {
            Token operador = Siguiente();

            Expresion rigth = Comparasion();

            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }

        return expr;
    }

    private Expresion Comparasion()
    {
        Expresion expr = Concatenar();

        TokenType[] a = { TokenType.Mayor, TokenType.MayorIgual, TokenType.Menor, TokenType.MenorIgual };

        while (Match(a))
        {
            Token operador = Siguiente();

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
            Token operador = Siguiente();

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
            Token operador = Siguiente();

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
            Token operador = Siguiente();

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
            Token operador = Siguiente();

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
            Token operador = Siguiente();

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
            Token operador = Siguiente();

            Expresion rigth = Unaria();

            return new Expresion.ExprUnaria(operador, rigth);
        }
        return IF_LET();
    }

    private Expresion IF_LET()
    {
        if (Match(TokenType.If))
        {
            current++;

            Expresion condicion = Primaria();

            Expresion ifcuerpo = Expression();

            Consume(TokenType.Else, "Expect 'else' after if statement");

            Expresion elsecuerpo = Expression();

            Expresion expr = new Expresion.If(condicion, ifcuerpo, elsecuerpo);

            return expr;
        }

        if (Match(TokenType.Let))
        {
            current++;
            //aqui revisar por si acaso
            List<Expresion.ExprAsignar> letCuerpo = Asign();

            Consume(TokenType.In, "Expected 'in' after arguments in let-in statement");

            Expresion inCuerpo = Expression();

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
                    Expresion expresion = Expression();

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

        return Primaria();
    }

    private List<Expresion.ExprAsignar> Asign()
    {
        List<Expresion.ExprAsignar> answer = new ();

        while (!Match(TokenType.In))
        {
            Token nombre = Siguiente();

            Consume(TokenType.Igual, "Expected '=' before expression" + Actual().Type + " " + Actual().Value + " in the let-in declaration");

            Expresion expr = Expression();

            if (!Match(TokenType.In))

                Consume(TokenType.Coma, "Expected ',' before expression " + Actual().Type + " : " + Actual().Value);

            answer.Add(new Expresion.ExprAsignar(nombre, expr));
        }

        return answer;
    }

    private Expresion Primaria()
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

            Expresion expr = Expression();

            Consume(TokenType.ParentesisCerrado, "Missing ) after expression " + Anterior().Type + " " + Anterior().Value);

            return expr;
        }
        if (Match(TokenType.Final)) return null!;
        //poner error
        errores.Add(new ERROR(ERROR.ErrorType.SyntaxError, "Invalid syntax in " + Actual().Type + " " + Actual().Value));
        return null!;
    }
}