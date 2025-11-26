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
        char startingCharacter = Advance();
        switch (startingCharacter)
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
            case '!': AddToken(Match('=') ? TokenType.BangEqual : TokenType.Bang); break;
            case '=': AddToken(Match('=') ? TokenType.EqualEqual : TokenType.Equal); break;
            case '<': AddToken(Match('=') ? TokenType.LessEqual : TokenType.Less); break;
            case '>': AddToken(Match('=') ? TokenType.GreaterEqual : TokenType.Greater); break;
            case '/':
                if (Match('/'))
                {
                    // scan a comment until end of line
                    while (!IsAtEnd() && Peek() != '\n')
                        Advance();
                }
                else
                {
                    AddToken(TokenType.Slash);
                }
                break;

            case ' ':
            case '\r':
            case '\t':
                // ignore whitespace
                break;

            case '\n':
                _line++;
                break;

            case '"':
                // scan a string literal
                while (!IsAtEnd())
                {
                    char c = Advance();

                    // keep updating the line number
                    if (c == '\n')
                        _line++;
                    
                    // check for end of string
                    if (c == '"')
                    {
                        AddToken(TokenType.String);
                        break;
                    }
                }
                // TODO: Error: unterminated string
                break;

            default:
                if (IsDigit(startingCharacter))
                {
                    // Consume whole-number part
                    while (IsDigit(Peek())) Advance();

                    // Consume fractional dot
                    if (Peek() == '.') Advance();

                    if (!IsDigit(Peek()))
                    {
                        // TODO: Error: expected digit after fractional dot
                    }

                    // Consume the fractional part
                    while (IsDigit(Peek())) Advance();

                    AddToken(TokenType.Number);
                }
                else
                {
                    // ignore unrecognized character
                }
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

    /// <summary>
    /// Return the current character and advance to the next one.
    /// </summary>
    /// <returns>The current character</returns>
    private char Advance()
    {
        return _source[_current++];
    }

    /// <summary>
    /// Peek at the current character without advancing.
    /// </summary>
    /// <returns>The current character</returns>
    private char Peek()
    {
        return _source[_current];
    }

    /// <summary>
    /// Checks if the current character is the specified one, and if so, advance.
    /// </summary>
    /// <param name="expectedChar">The expected character</param>
    /// <returns>true, iff the current character was the expected one</returns>
    private bool Match(char expectedChar)
    {
        if (IsAtEnd())
            return false;

        if (expectedChar != Peek())
            return false;

        // The current character is equal to c.
        // Consume it
        Advance();

        // The character matched the expected one
        return true;
    }

    private static bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    /// <summary>
    /// Check if the stream has ended.
    /// </summary>
    /// <returns>true iff the stream has ended</returns>
    private bool IsAtEnd()
    {
        return _current >= _source.Length;
    }

}
