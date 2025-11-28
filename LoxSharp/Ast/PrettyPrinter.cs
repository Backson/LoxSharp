using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSharp.Ast;

class PrettyPrinter : IExpressionVisitor<string>
{
    public string VisitBinaryExpression(BinaryExpression expr)
    {
        string op = expr.Operator.Text;
        string left = expr.Left.Accept(this);
        string right = expr.Right.Accept(this);
        return $"({op} {left} {right})";
    }

    public string VisitGroupedExpression(GroupedExpression expr)
    {
        string child = expr.Child.Accept(this);
        return $"(group {child})";
    }

    public string VisitLiteralExpression(LiteralExpression expr)
    {
        return expr.Value.Text;
    }

    public string VisitUnaryExpression(UnaryExpression expr)
    {
        string op = expr.Operator.Text;
        string child = expr.Child.Accept(this);
        return $"({op} {child})";
    }
}
