namespace HULK;
public abstract class Expresion
{
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
        public Funcion()
        {
            Identificador = null!;
            Parametros = null!;
            Cuerpo = null!;
        }

        public static Dictionary<object, object> VisitarFuncion(List<Expresion> argument, List<object> parametros)
        {
            if (argument.Count == parametros.Count)
            {
                int count = 0;
                Dictionary<object, object> ValorDeLosParametros = new Dictionary<object, object>();
                foreach (var x in argument)
                {
                    object valor = Evaluador.Evaluar(x, ValorDeLosParametros);
                    ValorDeLosParametros.Add(parametros[count], valor);
                    count++;
                }

                return ValorDeLosParametros;
            }
            //poner error
            throw new Exception("La cantidad de argumentos que recibe la funcion no es la dada");
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
        public object EvaluarExprLlamada(ExprLLamadaFuncion call, Dictionary<object, object> valor)
        {
            Funcion funcion = call.funcion;

            string name = call.Identificador;

            List<Expresion> argument = call.Argumento;

            return name switch
            {
                "sen" => Sen(argument, valor),
                "cos" => Cos(argument, valor),
                "print" => Print(argument, valor),
                "log" => Log(argument, valor),
                "sqrt" => Sqrt(argument, valor),
                _ => EvaluarFuncion(call),
            };
        }
        public object Sen(List<Expresion> argument, Dictionary<object, object> valor)
        {
            if (argument.Count == 1)
            {
                return Math.Sin((double)Evaluador.Evaluar(argument[0], valor));
            }
            throw new Exception("La funcion sen no admite mas de un argumento");
        }
        public object Cos(List<Expresion> argument, Dictionary<object, object> valor)
        {
            if (argument.Count == 1)
            {
                return Math.Cos((double)Evaluador.Evaluar(argument[0], valor));
            }
            throw new Exception("La funcion cos no admite mas de un argumento");
        }
        public object Sqrt(List<Expresion> argument, Dictionary<object, object> valor)
        {
            if (argument.Count == 1)
            {
                return Math.Sqrt((double)Evaluador.Evaluar(argument[0], valor));
            }
            throw new Exception("La funcion sqrt no admite mas de un argumento");
        }
        public object Log(List<Expresion> argument, Dictionary<object, object> valor)
        {
            if (argument.Count == 1)
            {
                return Math.Log10((double)Evaluador.Evaluar(argument[0], valor));
            }
            throw new Exception("La funcion log no admite mas de un argumento");
        }
        public object Print(List<Expresion> argument, Dictionary<object, object> valor)
        {
            if (argument.Count == 1)
            {
                return Evaluador.Evaluar(argument[0], valor);
            }
            //poner error
            throw new Exception("Una declaracion de print solo recibe un argumento");
        }
        public object EvaluarFuncion(ExprLLamadaFuncion call)
        {
            Dictionary<object, object> asig = Funcion.VisitarFuncion(call.Argumento, call.funcion.Parametros);
            return Evaluador.Evaluar(call.funcion.Cuerpo, asig);
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
        public object EvaluarExprVariable(Dictionary<object, object> asign, Token variable)
        {
            //poner error en el if de abajo
            if (asign is null) throw new Exception("Variable " + variable.Value + " no tiene un valor asignado");

            foreach (var objecto in asign)
            {
                if (asign.ContainsKey(variable.Value)) return asign[variable.Value];
            }
            //poner error
            throw new Exception("Variable " + variable.Value + " no tiene un valor asignado");
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
        public object EvaluarExprIF(object condicion, object ifCuerpo, object elseCuerpo)
        {
            if (condicion is bool Condicion)
            {
                if (Condicion == true)
                {
                    return ifCuerpo;
                }
                else return elseCuerpo;
            }
            throw new Exception("Error");
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
    public class ExprUnaria : Expresion
    {
        public Token Token;
        public Expresion Derecha;

        public ExprUnaria(Token token, Expresion derecha)
        {
            Token = token;
            Derecha = derecha;
        }

        public object EvaluarExprUnaria(object derecha)
        {
            if (Token.Type == TokenType.Resta)
            {
                if (derecha is double v)
                {
                    return -1 * v;
                }
                //poner error
                throw new Exception("El Operador '-' no puede usarse delante de" + " " + derecha);
            }
            if (Token.Type == TokenType.Negacion)
            {
                if (derecha is bool v)
                {
                    return !v;
                }
                //poner error
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
        public object EvaluarBinaria(object izquierda, object derecha)
        {
            if (Operador.Type == TokenType.Suma)
            {
                return Suma(izquierda, derecha);
            }
            if (Operador.Type == TokenType.Resta)
            {
                return Resta(izquierda, derecha);
            }
            if (Operador.Type == TokenType.Multiplicacion)
            {
                return Multiplicacion(izquierda, derecha);
            }
            if (Operador.Type == TokenType.Division)
            {
                return Division(izquierda, derecha);
            }
            if (Operador.Type == TokenType.Pow)
            {
                return Pow(izquierda, derecha);
            }
            if (Operador.Type == TokenType.Modulo)
            {
                return Modulo(izquierda, derecha);
            }
            if (Operador.Type == TokenType.Concatenar)
            {
                return Concatenar(izquierda, derecha);
            }
            if (Operador.Type == TokenType.IgualIgual)
            {
                return IgualIgual(izquierda, derecha);
            }
            if (Operador.Type == TokenType.NoIgual)
            {
                return NoIgual(izquierda, derecha);
            }
            if (Operador.Type == TokenType.Mayor)
            {
                return Mayor(izquierda, derecha);
            }
            if (Operador.Type == TokenType.MayorIgual)
            {
                return MayorIgual(izquierda, derecha);
            }
            if (Operador.Type == TokenType.Menor)
            {
                return Menor(izquierda, derecha);
            }
            if (Operador.Type == TokenType.MenorIgual)
            {
                return MenorIgual(izquierda, derecha);
            }
            if (Operador.Type == TokenType.And)
            {
                return And(izquierda, derecha);
            }
            if (Operador.Type == TokenType.Or)
            {
                return Or(izquierda, derecha);
            }
            return null!;
        }
        public static object IgualIgual(object izquierda, object derecha)
        {
            if (izquierda is double I1 && derecha is double D1)
            {
                if (I1 == D1) return true;
                else return false;
            }
            if (izquierda is bool I2 && derecha is bool D2)
            {
                if (I2 == D2) return true;
                else return false;
            }
            if (izquierda is string I3 && derecha is string D3)
            {
                if (I3 == D3) return true;
                else return false;
            }
            //poner error
            //else throw new Exception("El Operador == no puede estar entre" + " " + izquierda + " " + derecha);
            return -1;
        }
        public static object NoIgual(object izquierda, object derecha)
        {
            if (izquierda is double I1 && derecha is double D1)
            {
                if (I1 != D1) return true;
                return false;
            }
            if (izquierda is bool I2 && derecha is bool D2)
            {
                if (I2 != D2) return true;
                return false;
            }
            if (izquierda is string I3 && derecha is string D3)
            {
                if (I3 != D3) return true;
                return false;
            }
            //throw new Exception("El Operador != no puede estar entre" + " " + izquierda + " " + derecha);
            return -1;
        }

        public static object MenorIgual(object izquierda, object derecha)
        {
            if (izquierda is double I && derecha is double D) return I <= D;
            //poner error
            //throw new Exception("El Operador <= no puede estar entre" + " " + izquierda + " " + derecha);
            return -1;
        }
        public static object MayorIgual(object izquierda, object derecha)
        {
            if (izquierda is double v && derecha is double v1)
                return v >= v1;

            else return -1;//throw new Exception("El Operador >= no puede estar entre" + " " + izquierda + " " + derecha);
        }
        public static object Menor(object izquierda, object derecha)
        {
            if (izquierda is double v && derecha is double v1)
                return v < v1;

            else return -1;//throw new Exception("El Operador < no puede estar entre" + " " + izquierda + " " + derecha);
        }
        public static object Mayor(object izquierda, object derecha)
        {
            if (izquierda is double v && derecha is double v1)
                return v > v1;

            else return -1;//throw new Exception("El Operador > no puede estar entre" + " " + izquierda + " " + derecha);
        }
        public static object And(object izquierda, object derecha)
        {
            if (izquierda is bool v && derecha is bool v1)
                return v && v1;

            else return -1;//throw new Exception("El Operador && no puede estar entre" + " " + izquierda + " " + derecha);
        }
        public static object Or(object izquierda, object derecha)
        {
            if (izquierda is bool v && derecha is bool v1)
                return v || v1;

            else return -1;//throw new Exception("El Operador || no puede estar entre" + " " + izquierda + " " + derecha);
        }
        public static object Suma(object izquierda, object derecha)
        {
            if (izquierda is double v && derecha is double v1)
                return v + v1;
            else return -1;//throw new Exception("El Operador + no puede estar entre" + " " + izquierda + " " + derecha);
        }
        public static object Resta(object izquierda, object derecha)
        {
            if (izquierda is double v && derecha is double v1)
                return v - v1;
            else return -1;//throw new Exception("El Operador - no puede estar entre" + " " + izquierda + " " + derecha);
        }
        public static object Multiplicacion(object izquierda, object derecha)
        {
            if (izquierda is double v && derecha is double v1)
                return v * v1;
            else return -1;//throw new Exception("El Operador * no puede estar entre" + " " + izquierda + " " + derecha);
        }
        public static object Division(object izquierda, object derecha)
        {
            if (izquierda is double v && derecha is double v1)
                return v / v1;
            else return -1;//throw new Exception("El Operador / no puede estar entre" + " " + izquierda + " " + derecha);
        }
        public static object Pow(object izquierda, object derecha)
        {
            if (izquierda is double v && derecha is double v1)
                return Math.Pow(v, v1);
            else return -1;//throw new Exception("El Operador ^ no puede estar entre" + " " + izquierda + " " + derecha);
        }
        public static object Modulo(object izquierda, object derecha)
        {
            if (izquierda is double v && derecha is double v1)
                return v % v1;
            else return -1;//throw new Exception("El Operador + no puede estar entre" + " " + izquierda + " " + derecha);
        }
        public static object Concatenar(object izquierda, object derecha)
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
            }
            if (derecha is double v2)
            {
                resultado += v2;
            }
            if (derecha is bool v3)
            {
                resultado += v3;
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
        public object EvaluarExprLiteral(ExprLiteral expr)
        {
            return expr.literal;
        }
    }
}