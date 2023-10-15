
namespace HULK;
public class Evaluador
{
    public Expresion Parser;
    public Evaluador(Expresion parser)
    {
        Parser = parser;
    }
    public object Run(Expresion expr, Dictionary<object, object> asign)
    {
        return Evaluar(expr, asign);
    }

    public static object Evaluar(Expresion expr, Dictionary<object, object> asig)
    {
        if (expr is Expresion.ExprUnaria unaria)
        {
            return unaria.EvaluarExprUnaria(Evaluar(unaria.Derecha, asig));
        }

        if (expr is Expresion.ExprBinaria binaria)
        {
            return binaria.EvaluarBinaria(Evaluar(binaria.Izquierda, asig), Evaluar(binaria.Derecha, asig));
        }

        if (expr is Expresion.ExprLiteral literal)
        {
            return literal.EvaluarExprLiteral(literal);
        }

        if (expr is Expresion.If If)
        {
            object x = Evaluar(If.Condicion, asig);
            if (x is not bool)
            {
                //throw new ERROR(ERROR.ErrorType.SemanticError," if condition must return a bool");
            }

            else
            {
                if ((bool)x == true)
                {
                    return Evaluar(If.IfCuerpo, asig);
                }

                else return Evaluar(If.ElseCuerpo, asig);
            }

        }

        if (expr is Expresion.LetIn let)
        {
            Dictionary<object, object> answ = DictLetIn(let.LetCuerpo);
            return Evaluar(let.InCuerpo, answ);
        }

        if (expr is Expresion.ExprVariable variable)
        {
            return variable.EvaluarExprVariable(asig, variable.Nombre);
        }

        if (expr is Expresion.Funcion)
        {
            return null!;
        }

        if (expr is Expresion.ExprLLamadaFuncion call)
        {
            return call.EvaluarExprLlamada(call, asig);
        }

        return null!;
    }

    private static Dictionary<object, object> DictLetIn(List<Expresion.ExprAsignar> asignar)
    {
        Dictionary<object, object> resp = new ();

        foreach (var expresion in asignar)
        {
            if (resp.ContainsKey(expresion.Nombre.Value))
            {
                //throw new ERROR("variable " + expresion.Nombre.Value+ " already has a value assigned");
            }

            else resp.Add(expresion.Nombre.Value, Evaluar(expresion.Valor, resp));
        }
        return resp;
    }
}