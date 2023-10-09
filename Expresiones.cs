using System.Diagnostics.Contracts;

namespace HULK;
public abstract class Expresion
{
    public class ExprUnaria : Expresion
    {
        public Token Token;
        public Expresion Derecha;
        public ExprUnaria(Token token, Expresion derecha)
        {
            Token = token;
            Derecha = derecha;
        }
        public object VisitExprUnaria(object derecha)
        {
            if (Token.tipo == TokenType.resta)
            {
                if (derecha is double)
                {
                    return -1 * (double)derecha;
                }
                throw new Exception("El Operador '-' no puede usarse delante de" + " " + derecha);
            }
            if (Token.tipo == TokenType.negacion)
            {
                if (derecha is bool)
                {
                    return !(bool)derecha;
                }
                throw new Exception("El Operador '!' no puede usarse delante de" + " " + derecha);
            }
            return null!;
        }
    }
    public class ExprBinaria : Expresion
    {
        public Expresion Izquierda;
        public Token Operador;
        public Expresion Derecha;
        public ExprBinaria(Expresion izquierda, Token operador, Expresion derecha)
        {
            Izquierda = izquierda;
            Operador = operador;
            Derecha = derecha;
        }
        public object VisitExprBinaria(object izquierda, object derecha)
        {
            if (Operador.tipo == TokenType.dobleIgual)
            {
                return IgualIgual(izquierda, derecha);
            }
            if (Operador.tipo == TokenType.desigual)
            {
                return NoIgual(izquierda, derecha);
            }
            if (Operador.tipo == TokenType.menorIgual)
            {
                return MenorIgual(izquierda, derecha);
            }
            if (Operador.tipo == TokenType.mayorIgual)
            {
                return MayorIgual(izquierda, derecha);
            }
            if (Operador.tipo == TokenType.menor)
            {
                return Menor(izquierda, derecha);
            }
            if (Operador.tipo == TokenType.mayor)
            {
                return Mayor(izquierda, derecha);
            }
            if (Operador.tipo == TokenType.and)
            {
                return And(izquierda, derecha);
            }
            if (Operador.tipo == TokenType.or)
            {
                return Or(izquierda, derecha);
            }
            if (Operador.tipo == TokenType.suma)
            {
                return Suma(izquierda, derecha);
            }
            if (Operador.tipo == TokenType.resta)
            {
                return Resta(izquierda, derecha);
            }
            if (Operador.tipo == TokenType.Concatenar)
            {
                return Concatenar(izquierda, derecha);
            }
            if (Operador.tipo == TokenType.multiplicacion)
            {
                return Multiplicacion(izquierda, derecha);
            }
            if (Operador.tipo == TokenType.division)
            {
                return Division(izquierda, derecha);
            }
            if (Operador.tipo == TokenType.Pow)
            {
                return Pow(izquierda, derecha);
            }
            if (Operador.tipo == TokenType.modulo)
            {
                return Modulo(izquierda, derecha);
            }
            return null!;
        }
        public object IgualIgual(object izquierda, object derecha)
        {
            if (izquierda is double && derecha is double)
            {
                if (izquierda == derecha) return true;
                else return false;
            }
            if (izquierda is bool && derecha is bool)
            {
                if (izquierda == derecha) return true;
                else return false;
            }
            if (izquierda is string && derecha is string)
            {
                if (izquierda == derecha) return true;
                else return false;
            }
            else throw new Exception("No se puede efectuar la operacion '==' entre:" + " " + izquierda + " " + "y" + " " + derecha);
        }
        public object NoIgual(object izquierda, object derecha)
        {
            if (izquierda is double && derecha is double)
            {
                if (izquierda != derecha) return true;
                else return false;
            }
            if (izquierda is bool && derecha is bool)
            {
                if (izquierda != derecha) return true;
                else return false;
            }
            if (izquierda is string && derecha is string)
            {
                if (izquierda != derecha) return true;
                else return false;
            }
            else throw new Exception("No se puede efectuar la operacion '!=' entre:" + " " + izquierda + " " + "y" + " " + derecha);
        }

        public object MenorIgual(object izquierda, object derecha)
        {
            if (izquierda is double && derecha is double)return (double)izquierda <= (double)derecha;
            throw new Exception("No se puede efectuar la operacion '<=' entre:" + " " + izquierda + " " + "y" + " " + derecha);
        }
        public object MayorIgual(object izquierda, object derecha)
        {
            if (izquierda is double && derecha is double)return (double)izquierda >= (double)derecha;
            throw new Exception("No se puede efectuar la operacion '>=' entre:" + " " + izquierda + " " + "y" + " " + derecha);
        }
        public object Menor(object izquierda, object derecha)
        {
            if (izquierda is double && derecha is double)return (double)izquierda < (double)derecha;
            throw new Exception("No se puede efectuar la operacion '<' entre:" + " " + izquierda + " " + "y" + " " + derecha);
        }
        public object Mayor(object izquierda, object derecha)
        {
            if (izquierda is double && derecha is double)return (double)izquierda > (double)derecha;
            throw new Exception("No se puede efectuar la operacion '>' entre:" + " " + izquierda + " " + "y" + " " + derecha);
        }
        public object And(object izquierda, object derecha)
        {
            if (izquierda is bool && derecha is bool)return (bool)izquierda && (bool)derecha;
            throw new Exception("No se puede efectuar la operacion '&&' entre:" + " " + izquierda + " " + "y" + " " + derecha);
        }
        public object Or(object izquierda, object derecha)
        {
            if (izquierda is bool && derecha is bool)return (bool)izquierda || (bool)derecha;
            throw new Exception("No se puede efectuar la operacion '||' entre:" + " " + izquierda + " " + "y" + " " + derecha);
        }
        public object Suma(object izquierda, object derecha)
        {
            if (izquierda is double && derecha is double)return (double)izquierda + (double)derecha;
            throw new Exception("No se puede efectuar la operacion '+' entre:" + " " + izquierda + " " + "y" + " " + derecha);
        }
        public object Resta(object izquierda, object derecha)
        {
            if (izquierda is double && derecha is double)return (double)izquierda - (double)derecha;
            throw new Exception("No se puede efectuar la operacion '-' entre:" + " " + izquierda + " " + "y" + " " + derecha);
        }
        public object Multiplicacion(object izquierda, object derecha)
        {
            if (izquierda is double && derecha is double)return (double)izquierda * (double)derecha;
            throw new Exception("No se puede efectuar la operacion '*' entre:" + " " + izquierda + " " + "y" + " " + derecha);
        }
        public object Division(object izquierda, object derecha)
        {
            if (izquierda is double && derecha is double)return (double)izquierda / (double)derecha;
            throw new Exception("No se puede efectuar la operacion '/' entre:" + " " + izquierda + " " + "y" + " " + derecha);        }
        public object Pow(object izquierda, object derecha)
        {
            if (izquierda is double && derecha is double)return Math.Pow((double)izquierda, (double)derecha);
            throw new Exception("No se puede efectuar la operacion '^' entre:" + " " + izquierda + " " + "y" + " " + derecha);        }
        public object Modulo(object izquierda, object derecha)
        {
            if (izquierda is double && derecha is double)return (double)izquierda % (double)derecha;
            throw new Exception("No se puede efectuar la operacion '%' entre:" + " " + izquierda + " " + "y" + " " + derecha);        }
        public object Concatenar(object izquierda, object derecha)
        {
            string resultado="";
            if(izquierda is string)
            {
                resultado+=izquierda;
            }
            if(izquierda is double)
            {
                resultado+=(double)izquierda;
            }
            if(izquierda is bool)
            {
                resultado+=(bool)izquierda;
            }
            if(derecha is string)
            {
                resultado+=derecha;
            }
            if(derecha is double)
            {
                resultado+=(double)derecha;
            }
            if(derecha is bool)
            {
                resultado+=(bool)derecha;
            }
            return resultado;
        }
    }
    public class ExprLiteral : Expresion
    {
        public object literal;
        public ExprLiteral(object literal)
        {
            this.literal = literal;
        }
        public object VisitEXprLiteral(ExprLiteral expr)
        {
            return expr.literal;
        }
    }
    class ExprAsignar : Expresion
    {
        public Token Nombre;
        public Expresion Valor;
        ExprAsignar(Token nombre, Expresion valor)
        {
            Nombre = nombre;
            Valor = valor;
        }
    }
    class ExprVariable : Expresion
    {
        public Token Nombre;
        ExprVariable(Token nombre)
        {
            Nombre = nombre;
        }
    }
    class ExprLLamadaFuncion : Expresion
    {
        public string Identificador;
        public List<Token> Argumento;
        public Funcion funcion;
        ExprLLamadaFuncion(string identificador, List<Token> argumento, Funcion funcion)
        {
            Identificador = identificador;
            Argumento = argumento;
            this.funcion = funcion;
        }

    }
    class If : Expresion
    {
        public Expresion Condicion;
        public Expresion IfCuerpo;
        public Expresion ElseCuerpo;
        If(Expresion condicion, Expresion ifCuerpo, Expresion elseCuerpo)
        {
            Condicion = condicion;
            IfCuerpo = ifCuerpo;
            ElseCuerpo = elseCuerpo;
        }
    }
    class Funcion : Expresion
    {
        public string Identificador;
        public List<Token> Parametros;
        public Expresion Cuerpo;
        Funcion(string identificador, List<Token> parametros, Expresion cuerpo)
        {
            Identificador = identificador;
            Parametros = parametros;
            Cuerpo = cuerpo;
        }
    }
    class LetIn : Expresion
    {
        public Expresion LetCuerpo;
        public Expresion InCuerpo;
        LetIn(Expresion letCuerpo, Expresion inCuerpo)
        {
            LetCuerpo = letCuerpo;
            InCuerpo = inCuerpo;
        }

    }
    public class ExprGrouping : Expresion
    {
        public Expresion Expr;
        public ExprGrouping(Expresion expr)
        {
            Expr = expr;
        }
    }
}