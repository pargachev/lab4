using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Введите имя входного файла:");
                string inputFileName = Console.ReadLine();

                Evaluator function;
                double a;
                double b;
                double h;

                using (StreamReader inputFile = new StreamReader(inputFileName))
                {
                    function = new Evaluator(inputFile.ReadLine());
                    var parameters = inputFile.ReadLine().ToDoubleArray();
                    a = parameters[0];
                    b = parameters[1];
                    h = parameters[2];
                }

                LeftRectangleIntegrator integrator1 = new LeftRectangleIntegrator(function);
                TrapeziumIntegrator integrator2 = new TrapeziumIntegrator(function);

                double result1 = integrator1.Integrate(a, b, h);
                double result2 = integrator2.Integrate(a, b, h);

                Console.WriteLine("Введите имя выходного файла:");
                string outputFileName = Console.ReadLine();

                using (StreamWriter outputFile = new StreamWriter(outputFileName))
                {
                    outputFile.WriteLine(result1.ToString("F4", CultureInfo.InvariantCulture));
                    outputFile.WriteLine(result2.ToString("F4", CultureInfo.InvariantCulture));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }

        }
    }
}
