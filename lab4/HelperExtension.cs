using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace lab4
{
    public static class HelperExtension
    {
        public static double[] ToDoubleArray(this string stringDoubleArray)
        {
            string[] array = stringDoubleArray.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return array.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToArray();
        }
    }
}
