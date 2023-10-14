
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
        if (expr is Expresion.ExprUnaria)
        {
            Expresion.ExprUnaria unaria = (Expresion.ExprUnaria)expr;
            return unaria.VisitExprUnaria(GetValue(unaria.Derecha, asig));
        }

        if (expr is Expresion.ExprBinaria)
        {
            Expresion.ExprBinaria binaria = (Expresion.ExprBinaria)expr;
            return binaria.VisitExprBinaria(GetValue(binaria.Izquierda, asig), GetValue(binaria.Derecha, asig));
        }

        if (expr is Expresion.ExprLiteral)
        {
            Expresion.ExprLiteral literal = (Expresion.ExprLiteral)expr;
            return literal.VisitExprLiteral(literal);
        }

        if (expr is Expresion.If)
        {
            Expresion.If If = (Expresion.If)expr;
            return If.VisitExprIF(GetValue(If.Condicion, asig), GetValue(If.IfCuerpo, asig), GetValue(If.ElseCuerpo, asig));
        }

        if (expr is Expresion.LetIn)
        {
            Expresion.LetIn let = (Expresion.LetIn)expr;
            Dictionary<object, object> answ = DictLetIn(let.LetCuerpo);
            return GetValue(let.InCuerpo, answ);
        }

        if (expr is Expresion.ExprVariable)
        {
            Expresion.ExprVariable variable = (Expresion.ExprVariable)expr;
            return variable.VisitExprVariable(asig, variable.Nombre);
        }

        if (expr is Expresion.Funcion)
        {
            return "la funcion ha sido declarada correctamente";
        }

        if (expr is Expresion.ExprLLamadaFuncion)
        {
            Expresion.ExprLLamadaFuncion call = (Expresion.ExprLLamadaFuncion)expr;
            return call.VisitExprLlamada(call, asig);
        }

        return null!;
    }
    private static Dictionary<object, object> DictLetIn(List<Expresion.ExprAsignar> asignar)
    {
        Dictionary<object, object> resp = new Dictionary<object, object>();

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