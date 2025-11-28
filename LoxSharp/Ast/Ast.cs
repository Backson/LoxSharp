namespace LoxSharp.Ast;

interface IExpressionVisitor<T>
{
    T Visit(BinaryExpression expr);
    T Visit(UnaryExpression expr);
    T Visit(GroupingExpression expr);
    T Visit(LiteralExpression expr);
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
        return visitor.Visit(this);
    }
}

class UnaryExpression : Expression
{
    public required Token Operator { get; init; }
    public required Expression Child { get; init; }

    public override T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

class GroupingExpression : Expression
{
    public required Expression Child { get; init; }

    public override T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

class LiteralExpression : Expression
{
    public required Token Value { get; init; }

    public override T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
