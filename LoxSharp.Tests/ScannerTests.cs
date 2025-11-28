using System.Linq;
using Xunit;
using LoxSharp.Core;

namespace LoxSharp.Tests;

public class ScannerTests
{
    [Fact]
    public void Scan_SingleCharTokens()
    {
        string source = "(){},.-+;*";
        List<Token> tokens = new Scanner(source).ScanTokens();

        TokenType[] expectedTypes = new[] {
            TokenType.LeftParen,
            TokenType.RightParen,
            TokenType.LeftBrace,
            TokenType.RightBrace,
            TokenType.Comma,
            TokenType.Dot,
            TokenType.Minus,
            TokenType.Plus,
            TokenType.Semicolon,
            TokenType.Star,
            TokenType.EOF,
        };

        Assert.Equal(expectedTypes, tokens.Select(t => t.Type));

        string[] expectedTexts = new[] {"(", ")", "{", "}", ",", ".", "-", "+", ";", "*", ""};
        Assert.Equal(expectedTexts, tokens.Select(t => t.Text));
    }

    [Fact]
    public void Scan_Numbers()
    {
        string source = "123 45.67";
        List<Token> tokens = new Scanner(source).ScanTokens();

        Assert.Equal(TokenType.Number, tokens[0].Type);
        Assert.Equal("123", tokens[0].Text);

        Assert.Equal(TokenType.Number, tokens[1].Type);
        Assert.Equal("45.67", tokens[1].Text);

        Assert.Equal(TokenType.EOF, tokens[2].Type);
    }

    [Fact]
    public void Scan_Strings()
    {
        string source = "\"hello\"";
        var tokens = new Scanner(source).ScanTokens().ToList();

        Assert.Single(tokens, t => t.Type == TokenType.String);
        Token str = tokens.First(t => t.Type == TokenType.String);
        Assert.Equal("\"hello\"", str.Text);
        Assert.Equal(TokenType.EOF, tokens.Last().Type);
    }

    [Fact]
    public void Scan_IdentifiersAndKeywords()
    {
        string source = "var foo print";
        var tokens = new Scanner(source).ScanTokens().ToList();

        Assert.Equal(TokenType.Var, tokens[0].Type);
        Assert.Equal("var", tokens[0].Text);

        Assert.Equal(TokenType.Identifier, tokens[1].Type);
        Assert.Equal("foo", tokens[1].Text);

        Assert.Equal(TokenType.Print, tokens[2].Type);
        Assert.Equal("print", tokens[2].Text);

        Assert.Equal(TokenType.EOF, tokens[3].Type);
    }

    [Fact]
    public void Scan_OperatorsAndComparisons()
    {
        string source = "! != = == < <= > >=";
        var tokens = new Scanner(source).ScanTokens().ToList();

        TokenType[] expected = new[] {
            TokenType.Bang,
            TokenType.BangEqual,
            TokenType.Equal,
            TokenType.EqualEqual,
            TokenType.Less,
            TokenType.LessEqual,
            TokenType.Greater,
            TokenType.GreaterEqual,
            TokenType.EOF,
        };

        Assert.Equal(expected, tokens.Select(t => t.Type));
    }

    [Fact]
    public void Scan_CommentsAreIgnored()
    {
        string source = "var a = 1; // this is a comment\nprint a;";
        var tokens = new Scanner(source).ScanTokens().ToList();

        // Expect: var, identifier, =, number, ;, print, identifier, ;, EOF
        TokenType[] expected = new[] {
            TokenType.Var,
            TokenType.Identifier,
            TokenType.Equal,
            TokenType.Number,
            TokenType.Semicolon,
            TokenType.Print,
            TokenType.Identifier,
            TokenType.Semicolon,
            TokenType.EOF,
        };

        Assert.Equal(expected, tokens.Select(t => t.Type));
    }
}
