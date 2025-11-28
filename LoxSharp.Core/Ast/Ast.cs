namespace LoxSharp.Core.Ast;

public interface IExpressionVisitor<T>
{
    T Visit(BinaryExpression expr);
    T Visit(UnaryExpression expr);
    T Visit(GroupingExpression expr);
    T Visit(LiteralExpression expr);
}

public abstract class Expression
{
    // Visitor pattern
    public abstract T Accept<T>(IExpressionVisitor<T> visitor);
}

public class BinaryExpression : Expression
{
    public required Expression Left { get; init; }
    public required Token Operator { get; init; }
    public required Expression Right { get; init; }

    public override T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

public class UnaryExpression : Expression
{
    public required Token Operator { get; init; }
    public required Expression Child { get; init; }

    public override T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

public class GroupingExpression : Expression
{
    public required Expression Child { get; init; }

    public override T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

public class LiteralExpression : Expression
{
    public required Token Value { get; init; }

    public override T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
