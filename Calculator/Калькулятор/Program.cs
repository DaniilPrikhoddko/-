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
            Console.WriteLine("Введите значение переменной");
            float valueOfVariable = float.Parse(Console.ReadLine());
            var calculator = new RpnCalculator(userInput);
            List<Tokens> RPN = calculator.GetRPN();
            float result = calculator.CalculateExpression(RPN, valueOfVariable);
            Console.WriteLine("Ответ:");
            Console.WriteLine(result);
        }


    }

}