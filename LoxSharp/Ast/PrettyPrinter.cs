namespace LoxSharp.Ast;

class PrettyPrinter : IExpressionVisitor<string>
{
    public string Visit(BinaryExpression expr)
    {
        string op = expr.Operator.Text;
        string left = expr.Left.Accept(this);
        string right = expr.Right.Accept(this);
        return $"({op} {left} {right})";
    }

    public string Visit(GroupedExpression expr)
    {
        string child = expr.Child.Accept(this);
        return $"(group {child})";
    }

    public string Visit(LiteralExpression expr)
    {
        return expr.Value.Text;
    }

    public string Visit(UnaryExpression expr)
    {
        string op = expr.Operator.Text;
        string child = expr.Child.Accept(this);
        return $"({op} {child})";
    }
}
