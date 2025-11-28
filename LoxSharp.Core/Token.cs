namespace LoxSharp.Core;

public class Token
{
    public TokenType Type { get; }
    public string Text { get; init; } = string.Empty;
    public int Line { get; init; } = -1;

    public Token(TokenType type)
    {
        Type = type;
    }

    public override string ToString() => $"{Type} {Text}";
}
