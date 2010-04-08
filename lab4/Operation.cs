using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace lab4
{
    public enum OperationType
    {
        Plus = 0, 
        Minus, 
        Mul, 
        Div, 
        Pow
    }

    public class Operation
    {
        public OperationType Type { get; private set; }
        public int Position { get; private set; }

        public Operation(OperationType type, int position)
        {
            Type = type;
            Position = position;
        }

        internal Expression GetExpression(Expression left, Expression right)
        {
            switch (Type)
            {
                case OperationType.Plus:
                    return Expression.Add(left, right);
                case OperationType.Minus:
                    return Expression.Subtract(left, right);
                case OperationType.Mul:
                    return Expression.Multiply(left, right);
                case OperationType.Div:
                    return Expression.Divide(left, right);
                case OperationType.Pow:
                    return Expression.Power(left, right);
                default:
                    throw new Exception("Математическая операция " + Type.ToString() + " не поддерживается");
            }
        }
    }
}
