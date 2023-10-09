namespace HULK;
public class Token
{
    public TokenType tipo ;
    public string valor ;

    public Token(TokenType tipo, string valor)
    {
        this.tipo = tipo;
        this.valor = valor;
    }
    public Token Clonar()
    {
        return new Token(tipo, valor);
    }
}
