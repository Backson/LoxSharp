namespace LoxSharp.Core;

public class Interpreter : IExpressionVisitor<object>
{
    public object Visit(BinaryExpression expr)
    {
        throw new NotImplementedException();
    }

    public object Visit(UnaryExpression expr)
    {
        throw new NotImplementedException();
    }

    public object Visit(GroupingExpression expr)
    {
        throw new NotImplementedException();
    }

    public object Visit(LiteralExpression expr)
    {
        throw new NotImplementedException();
    }
}
