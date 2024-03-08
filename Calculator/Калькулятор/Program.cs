using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Design;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace RPNCalc
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите выражение:");
            string userInput = Console.ReadLine();
            var rpn = new RpnCalculator(userInput);
            float result = rpn.Calculate();
            Console.WriteLine("Ответ:");
            Console.WriteLine(result);
        }


    }

}