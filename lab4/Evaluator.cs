using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Globalization;

namespace lab4
{
    class Evaluator
    {
    	private ParameterExpression parameter = Expression.Parameter(typeof (double), "x");
		private readonly Func<double, double> eval;

		public Evaluator(string function)
    	{
			Expression expression = Parse(function.Replace(" ", "").ToLowerInvariant());

			var ex = Expression.Lambda<Func<double, double>>(expression, new[] {parameter});
			eval = ex.Compile();
    	}

		public double Eval(double x)
		{
			return eval(x);
		}

        private Expression Parse(string function)
        {
            function = RemoveRoundedBrackets(function);

            List<Operation> operations = GetOperations(function);
            if (operations.Count > 0)
			{
                var orepation = operations.OrderBy(o => (int)o.Type).First();
                Expression left = Parse(function.Substring(0, orepation.Position));
            	Expression right = Parse(function.Substring(orepation.Position + 1));
                return orepation.GetExpression(left, right);
            }

        	var expression = GetUnaryOperator(function) ??
        	                 GetUnaryFunction(function) ?? 
							 GetParameter(function) ?? 
							 GetConstant(function);

			if (expression != null)
				return expression;

			throw new Exception("Не возможно разобрать выражение: " + function);
        }

		private Expression GetUnaryOperator(string function)
		{
			if (function.StartsWith("-"))
				return Expression.Negate(Parse(function.Substring(1)));

			if (function.StartsWith("+"))
				return Expression.UnaryPlus(Parse(function.Substring(1)));
			
			return null;
		}

		private Expression GetUnaryFunction(string function)
		{
			if (function.StartsWith("sin"))
			{
				Expression param = Parse(function.Substring(3));
				return Expression.Call(typeof (Math), "Sin", null, param);
			}
			
			if (function.StartsWith("cos"))
			{
				Expression param = Parse(function.Substring(3));
				return Expression.Call(typeof (Math), "Cos", null, param);
			}

			return null;
		}

		private Expression GetParameter(string function)
		{
			if (function == "x")
				return parameter;

			return null;
		}

		private Expression GetConstant(string function)
		{
			try
			{
				double constant = double.Parse(function, CultureInfo.InvariantCulture);
				return Expression.Constant(constant, typeof(double));
			}
			catch (Exception)
			{
				return null;
			}
		}

        private string RemoveRoundedBrackets(string function)
        {
            if (!function.StartsWith("(")) return function;
            if (!function.EndsWith(")")) return function;

            int level = 1;
			for (int i = 1; i < function.Length - 1; i++)
			{
				char ch = function[i];
				if (ch == '(')
					level++;
				
				if (ch == ')')
					level--;

				if (level < 1)
					return function;
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
					if (level == 0) 
						throw new Exception("Неверная скобочная структура");
					level--;
				}

				if (level > 0) 
					continue;

				if (i == 0 || operations.Count(o => o.Position == i - 1) > 0) // Исключить унарные + и -
					continue;

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
