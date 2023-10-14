namespace HULK;
public class Token
{
    public TokenType Type;
    public string Grupo;
    public object Value;
    public Token(TokenType type, string grupo, object value)
    {
        Type = type;
        Grupo = grupo;
        Value = value;
    }
}
