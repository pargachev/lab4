using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lab4
{
    class Program
    {
        static void Main(string[] args)
        {

			Evaluator evaluator = new Evaluator("-(*(-(x+2)*(x-1)))");
        	var result = evaluator.Eval(3);
        }
    }
}
