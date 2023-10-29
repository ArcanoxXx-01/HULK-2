
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
            if (Token.Type == TokenType.Resta)
            {
                if (derecha is double v)
                {
                    return -v;
                }

                Evaluador.errores.Add( new ERROR(ERROR.Tipo.SemanticError, " Operator '-' cannot be used before " + derecha));
            
            }

            if (Token.Type == TokenType.Negacion)
            {
                if (derecha is bool v)
                {
                    return !v;
                }

                Evaluador.errores.Add( new ERROR(ERROR.Tipo.SemanticError, " Operator '!' cannot be used before " + derecha));
            }

            return null!;
        }
    }

    public class ExprBinaria : Expresion
    {
        public Expresion izquierda;
        public Token operador;
        public Expresion derecha;

        public ExprBinaria(Expresion izquierda, Token operador, Expresion derecha)
        {
            this.izquierda = izquierda;
            this.operador = operador;
            this.derecha = derecha;
        }

        public object VisitExprBinaria(object izquierda, object derecha)
        {
            if (operador.Type == TokenType.IgualIgual)
            {
                return IgualIgual(izquierda, derecha);
            }

            if (operador.Type == TokenType.NoIgual)
            {
                return NoIgual(izquierda, derecha);
            }

            if (operador.Type == TokenType.MenorIgual)
            {
                return MenorIgual(izquierda, derecha);
            }

            if (operador.Type == TokenType.MayorIgual)
            {
                return MayorIgual(izquierda, derecha);
            }

            if (operador.Type == TokenType.Menor)
            {
                return Menor(izquierda, derecha);
            }

            if (operador.Type == TokenType.Mayor)
            {
                return Mayor(izquierda, derecha);
            }

            if (operador.Type == TokenType.And)
            {
                return And(izquierda, derecha);
            }

            if (operador.Type == TokenType.Or)
            {
                return Or(izquierda, derecha);
            }

            if (operador.Type == TokenType.Suma)
            {
                return Suma(izquierda, derecha);
            }

            if (operador.Type == TokenType.Resta)
            {
                return Resta(izquierda, derecha);
            }

            if (operador.Type == TokenType.Concatenar)
            {
                return Concatenar(izquierda, derecha);
            }

            if (operador.Type == TokenType.Multiplicacion)
            {
                return Multiplicacion(izquierda, derecha);
            }

            if (operador.Type == TokenType.Division)
            {
                return Division(izquierda, derecha);
            }

            if (operador.Type == TokenType.Pow)
            {
                return Pow(izquierda, derecha);
            }

            if (operador.Type == TokenType.Modulo)
            {
                return Modulo(izquierda, derecha);
            }

            return null!;
        }
        public object IgualIgual(object izquierda, object derecha)
        {
            if (izquierda is double v && derecha is double v1)
            {
                if (v == v1) return true;
                else return false;
            }

            if (izquierda is bool v2 && derecha is bool v3)
            {
                if (v2 == v3) return true;
                else return false;
            }

            if (izquierda is string v4 && derecha is string v5)
            {
                if (v4 == v5) return true;
                else return false;
            }

            Evaluador.errores.Add( new ERROR(ERROR.Tipo.SemanticError, " Operator '==' cannot be used between " + izquierda + " and " + derecha));
            return null!;
        }

        public object NoIgual(object izquierda, object derecha)
        {
            if (izquierda is double v && derecha is double v1)
            {
                if (v != v1) return true;
                else return false;
            }

            if (izquierda is bool v2 && derecha is bool v3)
            {
                if (v2 != v3) return true;
                else return false;
            }

            if (izquierda is string v4 && derecha is string v5)
            {
                if (v4 != v5) return true;
                else return false;
            }

            Evaluador.errores.Add( new ERROR(ERROR.Tipo.SemanticError, " Operator '!=' cannot be used between " + izquierda + " and " + derecha));
            return null!;
        }

        public object MenorIgual(object izquierda, object derecha)
        {
            if (izquierda is double v && derecha is double v1)

                return v <= v1;

            Evaluador.errores.Add( new ERROR(ERROR.Tipo.SemanticError, " Operator '<=' cannot be used between " + izquierda + " and " + derecha));
            return null!;
        }

        public object MayorIgual(object izquierda, object derecha)
        {
            if (izquierda is double v && derecha is double v1)

                return v >= v1;

            Evaluador.errores.Add( new ERROR(ERROR.Tipo.SemanticError, " Operator '>=' cannot be used between " + izquierda + " and " + derecha));
            return null!;
        }

        public object Menor(object izquierda, object derecha)
        {
            if (izquierda is double v && derecha is double v1)

                return v < v1;

            Evaluador.errores.Add( new ERROR(ERROR.Tipo.SemanticError, " Operator '<' cannot be used between " + izquierda + " and " + derecha));
            return null!;
        }

        public object Mayor(object izquierda, object derecha)
        {
            if (izquierda is double v && derecha is double v1)

                return v > v1;

            Evaluador.errores.Add( new ERROR(ERROR.Tipo.SemanticError, " Operator '>' cannot be used between " + izquierda + " and " + derecha));
            return null!;

        }

        public object And(object izquierda, object derecha)
        {
            if (izquierda is bool v && derecha is bool v1)

                return v && v1;

            Evaluador.errores.Add( new ERROR(ERROR.Tipo.SemanticError, " Operator '&&' cannot be used between " + izquierda + " and " + derecha));
            return null!;
        }

        public object Or(object izquierda, object derecha)
        {
            if (izquierda is bool v && derecha is bool v1)

                return v || v1;

            Evaluador.errores.Add( new ERROR(ERROR.Tipo.SemanticError, " Operator '||' cannot be used between " + izquierda + " and " + derecha));
            return null!;
        }

        public object Suma(object izquierda, object derecha)
        {
            if (izquierda is double v && derecha is double v1)

                return v + v1;

            Evaluador.errores.Add( new ERROR(ERROR.Tipo.SemanticError, " Operator '+' cannot be used between " + izquierda + " and " + derecha));
            return null!;
        }

        public object Resta(object izquierda, object derecha)
        {
            if (izquierda is double v && derecha is double)

                return v - (double)derecha;

            Evaluador.errores.Add( new ERROR(ERROR.Tipo.SemanticError, " Operator '-' cannot be used between " + izquierda + " and " + derecha));
            return null!;

        }

        public object Multiplicacion(object izquierda, object derecha)
        {
            if (izquierda is double v && derecha is double v1)

                return v * v1;

            Evaluador.errores.Add( new ERROR(ERROR.Tipo.SemanticError, " Operator '*' cannot be used between " + izquierda + " and " + derecha));
            return null!;
        }

        public object Division(object izquierda, object derecha)
        {
            if (izquierda is double v && derecha is double v1)

                return v / v1;

            Evaluador.errores.Add( new ERROR(ERROR.Tipo.SemanticError, " Operator '/' cannot be used between " + izquierda + " and " + derecha));
            return null!;
        }

        public object Pow(object izquierda, object derecha)
        {
            if (izquierda is double v && derecha is double v1)

                return Math.Pow(v, v1);

            Evaluador.errores.Add( new ERROR(ERROR.Tipo.SemanticError, " Operator '^' cannot be used between " + izquierda + " and " + derecha));
            return null!;

        }

        public object Modulo(object izquierda, object derecha)
        {
            if (izquierda is double v && derecha is double v1)

                return v % v1;

            Evaluador.errores.Add( new ERROR(ERROR.Tipo.SemanticError, " Operator '%' cannot be used between " + izquierda + " and " + derecha));
            return null!;

        }

        public object Concatenar(object izquierda, object derecha)
        {
            string resultado = "";

            if (izquierda is string)
            {
                resultado += izquierda;
            }
            if (izquierda is double v)
            {
                resultado += v;
            }
            if (izquierda is bool v1)
            {
                resultado += v1;
            }
            if (derecha is string)
            {
                resultado += derecha;
                return resultado;
            }
            if (derecha is double v2)
            {
                resultado += v2;
                return resultado;
            }
            if (derecha is bool v3)
            {
                resultado += v3;
                return resultado;
            }


            Evaluador.errores.Add( new ERROR(ERROR.Tipo.SemanticError, " Operator '@' cannot be used between " + izquierda + " and " + derecha));
            return null!;

        }
    }

    public class ExprLiteral : Expresion
    {
        public object literal;
        public ExprLiteral(object literal)
        {
            this.literal = literal;
        }

        public object EvaluarLiteral(ExprLiteral expr)
        {
            return expr.literal;
        }
    }

    public class ExprAsignar : Expresion
    {
        public Token Nombre;
        public Expresion Valor;

        public ExprAsignar(Token nombre, Expresion valor)
        {
            Nombre = nombre;
            Valor = valor;
        }
    }

    public class ExprVariable : Expresion
    {
        public Token Nombre;
        public ExprVariable(Token nombre)
        {
            Nombre = nombre;
        }
        public object EvaluarVariable(Dictionary<object, object> asign, Token variable)
        {
            if (asign is null)
            {
                Evaluador.errores.Add( new ERROR(ERROR.Tipo.SemanticError, " Variable " + variable.Value + " does not have a value assigned"));
            }

            foreach (var objecto in asign!)
            {
                if (asign.ContainsKey(variable.Value))

                    return asign[variable.Value];
            }

            Evaluador.errores.Add( new ERROR(ERROR.Tipo.SemanticError, " Variable " + variable.Value + " does not have a value assigned"));
            return null!;
        }
    }

    public class ExprLLamadaFuncion : Expresion
    {
        public string Identificador;
        public List<Expresion> Argumento;
        public Funcion funcion;
        public ExprLLamadaFuncion(string identificador, List<Expresion> argumento, Funcion funcion)
        {
            Identificador = identificador;
            Argumento = argumento;
            this.funcion = funcion;
        }
        public  object EvaluarLlamada(ExprLLamadaFuncion call, Dictionary<object, object> valor)
        {
            List<object> parametros = new();

            foreach (Expresion args in call.Argumento)
            {
                parametros.Add(Evaluador.Evaluar(args, valor));
            }

            switch (Identificador)
            {
                case "sin":
                    return Sin(parametros);

                case "cos":
                    return Cos(parametros);

                case "print":
                    return Print(parametros);

                case "log":
                    return Log(parametros);

                case "sqrt":
                    return Sqrt(parametros);

                default:
                    Funcion funcion = Funciones.GetFuncion(Identificador);

                    return EvaluarFuncion(parametros, funcion);
            }
        }

        public static object Sin(List<object> argument)
        {
            if (argument.Count != 1)
            {
                throw new ERROR(ERROR.Tipo.SyntaxError, " Function 'sin' only receives one parameter as an argument");
            }

            else
            {
                if (argument[0] is double v)
                {
                    return Math.Sin(v);
                }

                Evaluador.errores.Add(new ERROR(ERROR.Tipo.SemanticError, " function sin only receives numbers as parameters"));
                
                return null!;
            }

        }

        public object Cos(List<object> argument)
        {
            if (argument.Count != 1)
            {
                throw new ERROR(ERROR.Tipo.SyntaxError, " Function 'cos' only receives one parameter as an argument");
            }

            else
            {
                if (argument[0] is double v)
                {
                    return Math.Cos(v);
                }

                Evaluador.errores.Add( new ERROR(ERROR.Tipo.SemanticError, " function 'cos' only receives numbers as parameters"));
                return null!;
            }
        }

        public object Sqrt(List<object> argument)
        {
            if (argument.Count != 1)
            {
                throw new ERROR(ERROR.Tipo.SyntaxError, " Function 'sqrt' only receives one parameter as an argument");
            }

            else
            {
                if (argument[0] is double v)
                {
                    return Math.Sqrt(v);
                }

                Evaluador.errores.Add( new ERROR(ERROR.Tipo.SemanticError, " function 'sqrt' only receives numbers as parameters"));
                return null!;
            }
        }

        public object Log(List<object> argument)
        {
            if (argument.Count != 2)
            {
                throw new ERROR(ERROR.Tipo.SyntaxError, " Function 'log' only receives two parameter as arguments");
            }

            else
            {
                if (argument[0] is double v && argument[1] is double v1)
                {
                    return Math.Log(v, v1);
                }

                Evaluador.errores.Add(new ERROR(ERROR.Tipo.SemanticError, " Function 'log' only receives numbers as arguments"));
                return null!;
            }
        }

        public object Print(List<object> argument)
        {
            if (argument.Count != 1)
            {
                throw new ERROR(ERROR.Tipo.SyntaxError, " Function 'print' only receives one parameter as an argument");
            }
            else
            {
                System.Console.WriteLine(argument[0]);
                return argument[0];
            }
        }

        public object EvaluarFuncion(List<object> valores, Funcion funcion)
        {
            Dictionary<object, object> value = new();

            if (funcion.Parametros.Count == valores.Count)
            {
                for (int i = 0; i < valores.Count; i++)
                {
                    value.Add(funcion.Parametros[i], valores[i]);
                }

                return Evaluador.Evaluar(funcion.Cuerpo, value);
            }

            else
            {
                Evaluador.errores.Add(new ERROR(ERROR.Tipo.SemanticError, " function " + funcion.Identificador + " only receives " + funcion.Parametros.Count + " parameter as arguments"));
                return null!;
            }
        }

    }

    public class Funcion : Expresion
    {
        public string Identificador;
        public List<object> Parametros;
        public Expresion Cuerpo;
        public Funcion(string identificador, List<object> parametros, Expresion cuerpo)
        {
            Identificador = identificador;
            Parametros = parametros;
            Cuerpo = cuerpo;
        }

    }

    public class If : Expresion
    {
        public Expresion Condicion;
        public Expresion IfCuerpo;
        public Expresion ElseCuerpo;
        public If(Expresion condicion, Expresion ifCuerpo, Expresion elseCuerpo)
        {
            Condicion = condicion;
            IfCuerpo = ifCuerpo;
            ElseCuerpo = elseCuerpo;
        }
    }

    public class LetIn : Expresion
    {
        public List<ExprAsignar> LetCuerpo;
        public Expresion InCuerpo;
        public LetIn(List<ExprAsignar> letCuerpo, Expresion inCuerpo)
        {
            LetCuerpo = letCuerpo;
            InCuerpo = inCuerpo;
        }

    }
    public class Vacia : Expresion
    {
    }

}