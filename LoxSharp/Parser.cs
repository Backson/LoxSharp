using LoxSharp.Ast;

namespace LoxSharp;

class Parser
{
    class ParseException : Exception
    {
        public Token? Token { get; init; }
        public ParseException() { }
        public ParseException(string message) : base(message) { }
        public ParseException(string message, Exception innerException) : base(message, innerException) { }
    }

    private readonly List<Token> _tokens;
    private int _current = 0;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
    }

    private Expression ParseExpression()
    {
        return ParseEquality();
    }

    private Expression ParseEquality()
    {
        Expression expr = ParseComparison();
        while (Match(TokenType.BangEqual, TokenType.EqualEqual))
        {
            Token op = Previous();
            Expression expr2 = ParseComparison();
            expr = new BinaryExpression
            {
                Left = expr,
                Operator = op,
                Right = expr2,
            };
        }
        return expr;
    }

    private Expression ParseComparison()
    {
        Expression expr = ParseAdditive();
        while (Match(TokenType.Greater, TokenType.GreaterEqual, TokenType.Less, TokenType.LessEqual))
        {
            Token op = Previous();
            Expression expr2 = ParseAdditive();
            expr = new BinaryExpression
            {
                Left = expr,
                Operator = op,
                Right = expr2,
            };
        }
        return expr;
    }

    private Expression ParseAdditive()
    {
        Expression expr = ParseMultiplicative();
        while (Match(TokenType.Plus, TokenType.Minus))
        {
            Token op = Previous();
            Expression expr2 = ParseMultiplicative();
            expr = new BinaryExpression
            {
                Left = expr,
                Operator = op,
                Right = expr2,
            };
        }
        return expr;
    }

    private Expression ParseMultiplicative()
    {
        Expression expr = ParseUnary();
        while (Match(TokenType.Star, TokenType.Slash))
        {
            Token op = Previous();
            Expression expr2 = ParseUnary();
            expr = new BinaryExpression
            {
                Left = expr,
                Operator = op,
                Right = expr2,
            };
        }
        return expr;
    }

    private Expression ParseUnary()
    {
        if (Match(TokenType.Bang, TokenType.Minus))
        {
            Token op = Previous();
            Expression right = ParseUnary();
            return new UnaryExpression
            {
                Operator = op,
                Child = right,
            };
        }
        return ParsePrimary();
    }

    private Expression ParsePrimary()
    {
        if (Match(TokenType.False, TokenType.True, TokenType.Nil, TokenType.Number, TokenType.String))
            return new LiteralExpression { Value = Previous() };

        if (Match(TokenType.LeftParen))
        {
            Expression expr = ParseExpression();
            _ = Consume(TokenType.RightParen, "Expect ')' after expression.");
            return new GroupingExpression
            {
                Child = expr,
            };
        }

        throw new ParseException("Expected expression.") { Token = Peek() };
    }

    private bool Match(params TokenType[] types)
    {
        foreach (TokenType type in types)
        {
            if (Check(type))
            {
                Advance();
                return true;
            }
        }
        return false;
    }

    private Token Consume(TokenType type, string message)
    {
        if (Check(type))
            return Advance();
        throw new ParseException(message) { Token = Peek() };
    }

    private bool Check(TokenType type)
    {
        if (IsAtEnd()) return false;
        return Peek().Type == type;
    }

    private Token Advance()
    {
        if (!IsAtEnd())
            _current++;
        return Previous();
    }

    private bool IsAtEnd()
    {
        return Peek().Type == TokenType.EOF;
    }

    private Token Peek()
    {
        return _tokens[_current];
    }

    private Token Previous()
    {
        return _tokens[_current - 1];
    }
}
