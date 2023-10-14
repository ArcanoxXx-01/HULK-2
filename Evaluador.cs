
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
        return GetValue(expr, asign);
    }
    public static object GetValue(Expresion expr, Dictionary<object, object> asig)
    {
        if (expr is Expresion.ExprUnaria unaria)
        {
            return unaria.EvaluarExprUnaria(GetValue(unaria.Derecha, asig));
        }

        if (expr is Expresion.ExprBinaria binaria)
        {
            return binaria.EvaluarBinaria(GetValue(binaria.Izquierda, asig), GetValue(binaria.Derecha, asig));
        }

        if (expr is Expresion.ExprLiteral literal)
        {
            return literal.EvaluarExprLiteral(literal);
        }

        if (expr is Expresion.If If)
        {
            return If.EvaluarExprIF(GetValue(If.Condicion, asig), GetValue(If.IfCuerpo, asig), GetValue(If.ElseCuerpo, asig));
        }

        if (expr is Expresion.LetIn let)
        {
            Dictionary<object, object> answ = DictLetIn(let.LetCuerpo);
            return GetValue(let.InCuerpo, answ);
        }

        if (expr is Expresion.ExprVariable variable)
        {
            return variable.EvaluarExprVariable(asig, variable.Nombre);
        }

        if (expr is Expresion.Funcion)
        {
            return "la funcion ha sido declarada correctamente";
        }

        if (expr is Expresion.ExprLLamadaFuncion call)
        {
            return call.EvaluarExprLlamada(call, asig);
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
                throw new Exception("Ya se le ha asignado un valor a" + expresion.Nombre.Value);
            }
            else resp.Add(expresion.Nombre.Value, GetValue(expresion.Valor, resp));
        }
        return resp;
    }

}