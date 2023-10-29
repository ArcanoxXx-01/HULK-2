
namespace HULK;
public class Evaluador
{
    public Expresion Parser;

    public static List<ERROR>errores=new();
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
            return unaria.VisitExprUnaria(Evaluar(unaria.Derecha, asig));
        }

        if (expr is Expresion.ExprBinaria binaria)
        {
            return binaria.VisitExprBinaria(Evaluar(binaria.izquierda, asig), Evaluar(binaria.derecha, asig));
        }

        if (expr is Expresion.ExprLiteral literal)
        {
            return literal.EvaluarLiteral(literal);
        }

        if (expr is Expresion.If If)
        {
            object x = Evaluar(If.Condicion, asig);
            if ((x is not bool v))
            {
                errores.Add(new ERROR(ERROR.Tipo.SemanticError, " if condition must return a bool"));
                //throw new ERROR(ERROR.Tipo.SemanticError, " if condition must return a bool");
            }

            else
            {
                if (v == true)
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
            return variable.EvaluarVariable(asig, variable.Nombre);
        }

        if (expr is Expresion.Funcion)
        {
            return null!;
        }

        if (expr is Expresion.ExprLLamadaFuncion call)
        {
            return call.EvaluarLlamada(call, asig);
        }

        return null!;
    }

    private static Dictionary<object, object> DictLetIn(List<Expresion.ExprAsignar> asignar)
    {
        Dictionary<object, object> resp = new();

        foreach (var expresion in asignar)
        {
            if (resp.ContainsKey(expresion.Nombre.Value))
            {
                errores.Add(new ERROR(ERROR.Tipo.SemanticError, " variable " + expresion.Nombre.Value + " already has a value assigned"));
                //throw new ERROR(ERROR.Tipo.SemanticError, " variable " + expresion.Nombre.Value + " already has a value assigned");
            }
            else resp.Add(expresion.Nombre.Value, Evaluar(expresion.Valor, resp));
        }
        return resp;
    }
}