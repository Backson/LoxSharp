namespace LoxSharp;

class Scanner
{
    private string _source = "";
    private List<Token> _tokens = [];

    private int _start = 0;
    private int _current = 0;
    private int _line = 1;

    public Scanner(string source)
    {
        _source = source;
    }

    public List<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            // start of new lexeme
            _start = _current;
            ScanToken();
        }

        _tokens.Add(new Token(TokenType.EOF)
        {
            Line = _line,
        });
        return _tokens;
    }

    private void ScanToken()
    {
        char c = Advance();
        switch (c)
        {
            case '(': AddToken(TokenType.LeftParen); break;
            case ')': AddToken(TokenType.RightParen); break;
            case '{': AddToken(TokenType.LeftBrace); break;
            case '}': AddToken(TokenType.RightBrace); break;
            case ',': AddToken(TokenType.Comma); break;
            case '.': AddToken(TokenType.Dot); break;
            case '-': AddToken(TokenType.Minus); break;
            case '+': AddToken(TokenType.Plus); break;
            case ';': AddToken(TokenType.Semicolon); break;
            case '*': AddToken(TokenType.Star); break;
            default:
                // skip it
                break;
        }
    }

    private void AddToken(TokenType tokenType)
    {
        _tokens.Add(new Token(tokenType)
        {
            Text = _source[_start.._current],
            Line = _line,
        });
    }

    private char Advance()
    {
        return _source[_current++];
    }

    private bool IsAtEnd()
    {
        return _current >= _source.Length;
    }

}
