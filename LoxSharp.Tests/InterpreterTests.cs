using LoxSharp.Core;

namespace LoxSharp.Tests;

public class InterpreterTests
{
    public object Interpret(string source)
    {
        List<Token> tokens = new Scanner(source).ScanTokens();
        Expression expr = new Parser(tokens).Parse();
        var interpreter = new Interpreter();
        return expr.Accept(interpreter);
    }

    [Fact]
    public void Interpret_NumberExpression()
    {
        object result = Interpret("1 + 2 * 3");
        Assert.IsType<double>(result);
        Assert.Equal(7.0, result);
    }

    [Fact]
    public void Interpret_ComparisonExpression()
    {
        object result = Interpret("1 + 2 < 5");
        Assert.IsType<bool>(result);
        Assert.Equal(true, result);
    }
}
