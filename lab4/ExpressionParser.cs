using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace lab4
{
    class ExpressionParser
    {
        Expression Parse(string function)
        {
            function = function.Replace(" ", "");
            function = RemoveRoundedBrackets(function);
            List<Operation> operations = GetOperations(function);
            if (operations.Count == 0)
            {
                //Expression.S
				throw new NotImplementedException();
            }
            else
            {
                var orepation = operations.OrderBy(o => (int)o.Type).First();
                Expression left = Parse(function.Substring(0, orepation.Position));
                Expression right = Parse(function.Substring(orepation.Position));
                return orepation.GetExpression(left, right);
            }
        }

        private string RemoveRoundedBrackets(string function)
        {
            if (function[0] != '(') return function;
            if (function[function.Length-1] != ')') return function;

            int level = 1;
            for (int i = 1; i < function.Length- 1; i++)
            {
                char ch = function[i];
                if (ch == '(')
                {
                    level++;
                    continue;
                }
                if (ch == ')')
                {
                    if (level == 1) return function;
                    level--;
                    continue;
                }
            }
            return RemoveRoundedBrackets(function.Substring(1, function.Length - 2));
        }

        private List<Operation> GetOperations(string function)
        {
            List<Operation> operations = new List<Operation>();
            int level = 0;
            for (int i = 0; i < function.Length; i++)
            {
                char ch = function[i];
                if (ch == '(')
                    level++;
                
                if (ch == ')')
                {
                    if (level == 0) throw new Exception("Неверная скобочная структура");
                    level--;
                }

                if (level > 0) continue;

                if (ch == '+')
                    operations.Add(new Operation(OperationType.Plus, i));
                
                if (ch == '-')
                    operations.Add(new Operation(OperationType.Minus, i));
                
                if (ch == '*')
                    operations.Add(new Operation(OperationType.Mul, i));
                
                if (ch == '/')
                    operations.Add(new Operation(OperationType.Div, i));
                
                if (ch == '^')
                    operations.Add(new Operation(OperationType.Pow, i));
            }
            if (level != 0) throw new Exception("Неверная скобочная структура");
            return operations;
        }
    }
}
