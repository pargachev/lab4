using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lab4
{
    public class TrapeziumIntegrator
    {
        private Evaluator function;

        public TrapeziumIntegrator(Evaluator function)
        {
            this.function = function;
        }

        internal double Integrate(double a, double b, double h)
        {
            double x = a;
            double intergal = 0;
            while (x < b)
            {
                intergal += (function.Eval(x) + function.Eval(x + h)) / 2;
                x += h;
            }
            intergal *= h;
            return intergal;
        }
    }
}
