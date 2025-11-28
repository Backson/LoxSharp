namespace LoxSharp.Ast;

interface IExpressionVisitor<T>
{
    T VisitBinaryExpression(BinaryExpression expr);
    T VisitUnaryExpression(UnaryExpression expr);
    T VisitGroupedExpression(GroupedExpression expr);
    T VisitLiteralExpression(LiteralExpression expr);
}

abstract class Expression
{
    // Visitor pattern
    public abstract T Accept<T>(IExpressionVisitor<T> visitor);
}

class BinaryExpression : Expression
{
    public required Expression Left { get; init; }
    public required Token Operator { get; init; }
    public required Expression Right { get; init; }

    public override T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.VisitBinaryExpression(this);
    }
}

class UnaryExpression : Expression
{
    public required Token Operator { get; init; }
    public required Expression Child { get; init; }

    public override T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.VisitUnaryExpression(this);
    }
}

class GroupedExpression : Expression
{
    public required Expression Child { get; init; }

    public override T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.VisitGroupedExpression(this);
    }
}

class LiteralExpression : Expression
{
    public required Token Value { get; init; }

    public override T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.VisitLiteralExpression(this);
    }
}
