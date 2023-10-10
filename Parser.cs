using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace HULK;
public class Parser
{
    public List<Token> Tokens;

    //Posicion de Tokens en la que estoy parado 
    private int actual = 0;

    //constructor
    public Parser(List<Token> tokens)
    {
        Tokens = tokens;
    }

    //En la posicion actual va a ver que tipo de expresion es y la va a construir
    private Expresion Parsear()
    {
        return Logica();
    }

    private bool Match(TokenType[] posibleTipo)
    {
        for (int i = 0; i < posibleTipo.Length; i++)
        {
            if(Tokens[actual].tipo == TokenType.puntoYcoma)return false;
            if(Tokens[actual].tipo == posibleTipo[i])
            {
                Advance();
                return true;
            }
        }
        return false;
    }
    private bool Match(TokenType type)
    {
        if(Tokens[actual].tipo == TokenType.puntoYcoma)return false;
        if(Tokens[actual].tipo == type)
        {
            Advance();
            return true;
        }
        else return false;
    }
    //Retorna verdadero si el TokenType que se le inserta es del mismo tipo del que estamos parados en la lista
    /*private bool Check(TokenType type)
    {
        if (Tokens[actual].tipo == TokenType.puntoYcoma) return false;
        return Tokens[actual].tipo == type;
    }*/
    //Consume el token actual y lo devuelve 
    private Token Advance()
    {
        if (Tokens[actual].tipo != TokenType.puntoYcoma)
        {
        actual++;
        return Previous();
        }
        else throw new Exception();
    }
    
    //Retorna el Token en la posicion anterior
    private Token Previous()
    {
        return Tokens[actual - 1];
    }

    //Retorna verdadero si el Token es PuntoYComa , o sea que esta al final de la linea
    /*private bool IAE()
    {
        return Tokens[current].tipo == TokenType.puntoYcoma;
    }*/

    //Retorna el token en la posicion actual
    /*private Token Peek()
    {
        return Tokens[actual];
    }*/
    private Expresion Logica()
    {
        Expresion expr = Igualdad();
        TokenType[] a = { TokenType.and, TokenType.or };
        while (Match(a))
        {
            Token operador = Advance();
            Expresion rigth = Igualdad();
            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }
        return expr;
    }
    private Expresion Igualdad()
    {
        Expresion expr = Comparacion();
        TokenType[] a = { TokenType.dobleIgual, TokenType.desigual };
        while (Match(a))
        {
            Token operador = Advance();
            Expresion rigth = Comparacion();
            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }
        return expr;
    }
    private Expresion Comparacion()
    {
        Expresion expr = Concatenar();
        TokenType[] a = { TokenType.mayor, TokenType.mayorIgual, TokenType.menor, TokenType.menorIgual };
        while (Match(a))
        {
            Token operador = Advance();
            Expresion rigth = Concatenar();
            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }
        return expr;
    }
    private Expresion Concatenar()
    {
        Expresion expr=Suma_Resta();
        while(Match(TokenType.Concatenar))
        {
            Token operador=Advance();
            Expresion rigth=Suma_Resta();
            expr=new Expresion.ExprBinaria(expr, operador,rigth);
        }
        return expr;
    }
    private Expresion Suma_Resta()
    {
        Expresion expr = Multiplicacion_Division();
        TokenType[] a = { TokenType.suma, TokenType.resta};
        while (Match(a))
        {
            Token operador = Advance();
            Expresion rigth = Multiplicacion_Division();
            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }
        return expr;
    }
    private Expresion Multiplicacion_Division()
    {
        Expresion expr = Potencia();
        TokenType[] a = { TokenType.multiplicacion, TokenType.division };
        while (Match(a))
        {
            Token operador = Advance();
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
            Token operador = Advance();
            Expresion rigth = Modulo();
            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }
        return expr;
    }
    private Expresion Modulo()
    {
        Expresion expr = Unary();
        while (Match(TokenType.modulo))
        {
            Token operador = Advance();
            Expresion rigth = Unary();
            expr = new Expresion.ExprBinaria(expr, operador, rigth);
        }
        return expr;
    }
    private Expresion Unary()
    {
        TokenType[] a = { TokenType.resta, TokenType.negacion };
        while (Match(a))
        {
            Token operador = Advance();
            Expresion rigth = Unary();
            return new Expresion.ExprUnaria(operador, rigth);
        }
        return primary();
    }
    private Expresion primary()
    {
        if (Match(TokenType.False))
        {
            return new Expresion.ExprLiteral(false);
        }
        if (Match(TokenType.True))
        {
            return new Expresion.ExprLiteral(true);
        }
        TokenType[] a = { TokenType.numero, TokenType.String };
        if (Match(a))
        {
            return new Expresion.ExprLiteral(Advance().valor);
        }
        if (Match(TokenType.parentesisAbierto))
        {
            Expresion expr = Parsear();
            Consume(TokenType.parentesisCerrado, "Se esperaba  de  expresion");
            return new Expresion.ExprGrouping(expr);
        }
        throw new Exception("");
    }
    private Token Consume(TokenType type, string mensaje)
    {
        if (Tokens[actual].tipo != TokenType.puntoYcoma && Tokens[actual].tipo == type) return Advance();
        throw new Exception(" Error");
    }
}