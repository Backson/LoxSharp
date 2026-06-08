using System.Data;
using System.Runtime.Intrinsics.X86;

namespace LoxSharp.Core;

public class Interpreter : IExpressionVisitor<object>
{
    public object Visit(BinaryExpression expr)
    {
        object lhs = Evaluate(expr.Left);
        object rhs = Evaluate(expr.Right);

        return expr.Operator.Type switch
        {
            TokenType.Plus => BinaryPlus(lhs, rhs),
            TokenType.Minus => (double)lhs - (double)rhs,
            TokenType.Star => (double)lhs * (double)rhs,
            TokenType.Slash => (double)lhs / (double)rhs,
            TokenType.Greater => (double)lhs > (double)rhs,
            TokenType.GreaterEqual => (double)lhs >= (double)rhs,
            TokenType.Less => (double)lhs < (double)rhs,
            TokenType.LessEqual => (double)lhs <= (double)rhs,
            TokenType.BangEqual => !IsEqual(lhs, rhs),
            TokenType.EqualEqual => IsEqual(lhs, rhs),
            _ => throw new ArgumentException($"Binary expression has invalid operator type {expr.Operator.Type}")
        };
    }

    public object Visit(UnaryExpression expr)
    {
        object rhs = Evaluate(expr.Child);

        return expr.Operator.Type switch
        {
            TokenType.Minus => -(double)rhs,
            TokenType.Bang => !IsTruthy(rhs),
            _ => throw new ArgumentException($"Unary expression has invalid operator type {expr.Operator.Type}")
        };

    }

    public object Visit(GroupingExpression expr)
    {
        return Evaluate(expr.Child);
    }

    public object Visit(LiteralExpression expr)
    {
        return expr.Value.Type switch
        {
            TokenType.Number => double.Parse(expr.Value.Text),
            TokenType.True => true,
            TokenType.False => false,
            TokenType.String => expr.Value.Text,
            _ => throw new ArgumentException($"Literal expression has invalid value type {expr.Value.Type}")
        };
    }

    private object Evaluate(Expression expr)
    {
        return expr.Accept(this);
    }

    private static bool IsTruthy(object o)
    {
        return o switch
        {
            null => false,
            bool b => b,
            byte i => i != 0,
            sbyte i => i != 0,
            short i => i != 0,
            ushort i => i != 0,
            int i => i != 0,
            uint i => i != 0,
            long i => i != 0,
            ulong i => i != 0,
            float f => f != 0.0f,
            double d => d != 0.0,
            _ => throw new NotImplementedException($"IsTruthy is not implemented for argument type {o.GetType()}"),
        };
    }

    private static object BinaryPlus(object lhs, object rhs)
    {
        return (lhs, rhs) switch
        {
            (double a, double b) => a + b,
            (string a, string b) => a + b,
            _ => throw new ArgumentException($"Binary plus is not implemented for types {lhs.GetType()} and {rhs.GetType()}")
        };
    }

    private static bool IsEqual(object lhs, object rhs)
    {
        if (lhs == null && rhs == null)
            return true;
        if (lhs == null)
            return false;
        return lhs.Equals(rhs);
    }
}
