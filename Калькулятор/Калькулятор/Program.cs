using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Design;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace калькулятор_лаба_5
{
    public class Tokens
    {

    }
    public class Parenthesis : Tokens
    {
        char parenthesis = ' ';
        public bool itIsCloseParenthesis = false;
        public bool itIsOpenParenthesis = false;
        public Parenthesis(char i)
        {
            parenthesis = i;
            if (i == ')')
            {
                itIsCloseParenthesis = true;
            }
            else
            {
                itIsOpenParenthesis = true;
            }
        }
        public override string ToString()
        {
            return Convert.ToString(parenthesis);
        }
    }
    public class Number : Tokens
    {
        public float number = 0;
        public Number(float a)
        {
            number = a;
        }
        public override string ToString()
        {
            return Convert.ToString(number);
        }
    }
    public class Operation : Tokens
    {
        public char operation = ' ';
        public int priorityOfOperation = 0;
        public Operation(char i)
        {
            operation = i;
            priorityOfOperation = ConvertOperarionToPriority(i);

        }
        public override string ToString()
        {
            return Convert.ToString(operation);
        }
        public int ConvertOperarionToPriority(char i)
        {
            Dictionary<char, int> OperationPriority = new Dictionary<char, int>()
            {
                {'+', 2 },
                {'-', 2 },
                {'*', 3 },
                {'/', 3 },
                {'^', 4 },
            };
            return OperationPriority[i];
        }
    }
    internal class Program
    {
        public static List<Tokens> token = new List<Tokens>();
        public static List<Tokens> RPN = new List<Tokens>();
        public static float result = 0;
        static void Main(string[] args)
        {
            Console.WriteLine("Введите выражение:");
            string userStr = Console.ReadLine();
            userStr = userStr.Replace(" ", "");
            Tokenize(userStr);
            RPNImport(token);
            CalculateExpression(RPN);
            Console.WriteLine("Ответ:");
            Console.WriteLine(result);
        }

        public static void Tokenize(string userStr)
        {

            string operators = "+-*/^";
            string a = null;
            foreach (char i in userStr)
            {
                if (Char.IsDigit(i))
                {
                    a += i;
                }

                else if (operators.Contains(i))
                {
                    if (a != null)
                    {
                        token.Add(new Number(float.Parse(a)));
                        token.Add(new Operation(i));
                        a = null;
                    }
                    else
                    {
                        token.Add(new Operation(i));
                    }

                }
                else if (i == '(' || i == ')')
                {
                    if (i == ')')
                    {
                        token.Add(new Number(float.Parse(a)));
                        a = null;
                    }

                    token.Add(new Parenthesis(i));
                }

                else if (i == ',')
                {
                    a += i;
                }

                else
                {
                    Console.WriteLine("Ошибка: Недопустимое выражение");
                }

            }

            if (a != null)
            {
                token.Add(new Number(float.Parse(a)));
            }

        }

        static void RPNImport(List<Tokens> token)
        {
            Stack<Tokens> stackForOp = new Stack<Tokens>();
            foreach (var tok in token)
            {
                int priorityOfStackOperation = 0;
                int currentPriorityOfOperation = 0;


                if (stackForOp.Count > 0 && stackForOp.Peek() is Operation)
                {
                    priorityOfStackOperation = ((Operation)stackForOp.Peek()).priorityOfOperation;
                }

                if (tok is Number number)
                {
                    RPN.Add(number);
                }

                else if (tok is Parenthesis parenthesis)
                {
                    if (parenthesis.itIsOpenParenthesis == true)
                    {
                        stackForOp.Push(parenthesis);
                    }

                    else if (parenthesis.itIsCloseParenthesis == true)
                    {
                        while (!(stackForOp.Peek() is Parenthesis))
                        {
                            RPN.Add(stackForOp.Pop());
                        }

                        stackForOp.Pop();
                    }

                }

                else if (tok is Operation operation)
                {
                    currentPriorityOfOperation = operation.priorityOfOperation;
                    // Случаи когда мы загружаем операции в стек
                    if (stackForOp.Count == 0)
                    {
                        stackForOp.Push(operation);
                    }

                    else if (currentPriorityOfOperation > priorityOfStackOperation)
                    {
                        stackForOp.Push(operation);
                    }

                    //Ситуации когда мы выгружаем стек
                    else if (currentPriorityOfOperation <= priorityOfStackOperation)
                    {
                        while (stackForOp.Count > 0 && currentPriorityOfOperation <= priorityOfStackOperation && stackForOp.Peek() is Operation)
                        {
                            RPN.Add(stackForOp.Pop());
                            if (stackForOp.Count != 0 && stackForOp.Peek() is Operation)
                            {
                                priorityOfStackOperation = ((Operation)stackForOp.Peek()).priorityOfOperation;
                            }

                        }

                        stackForOp.Push(operation);
                    }


                }

            }

            if (stackForOp.Count != 0)
            {
                while (stackForOp.Count != 0)
                {
                    RPN.Add(stackForOp.Pop());
                }

            }

        }

        public static void CalculateExpression(List<Tokens> RPN)
        {
            Stack<Tokens> stackForResult = new Stack<Tokens>();
            foreach (var elem in RPN)
            {
                if (elem is Number)
                    stackForResult.Push(elem);
                else if (elem is Operation operation)
                {
                    if (operation.operation == '+')
                    {
                        AddNum(stackForResult);
                    }

                    else if (operation.operation == '-')
                    {
                        SubtractNum(stackForResult);
                    }

                    else if (operation.operation == '*')
                    {
                        MultiplyNum(stackForResult);
                    }

                    else if (operation.operation == '/')
                    {
                        DivideNum(stackForResult);
                    }

                    else if (operation.operation == '^')
                    {
                        RaiseNumToPower(stackForResult);
                    }

                }

            }

            result = (float)((Number)stackForResult.Pop()).number;
        }

        private static void RaiseNumToPower(Stack<Tokens> stackForResult)
        {
            float degree = ((Number)stackForResult.Pop()).number;
            float exponentiation = (float)Math.Pow(((Number)stackForResult.Pop()).number, degree);
            stackForResult.Push(new Number(exponentiation));
        }

        private static void DivideNum(Stack<Tokens> stackForResult)
        {
            float division = 1f / (((Number)stackForResult.Pop()).number / ((Number)stackForResult.Pop()).number);
            stackForResult.Push(new Number(division));
        }

        private static void MultiplyNum(Stack<Tokens> stackForResult)
        {
            float multiplication = ((Number)stackForResult.Pop()).number * ((Number)stackForResult.Pop()).number;
            stackForResult.Push(new Number(multiplication));
        }

        private static void SubtractNum(Stack<Tokens> stackForResult)
        {
            float difference = -((Number)stackForResult.Pop()).number + ((Number)stackForResult.Pop()).number;
            stackForResult.Push(new Number(difference));
        }

        private static void AddNum(Stack<Tokens> stackForResult)
        {
            float sum = ((Number)stackForResult.Pop()).number + ((Number)stackForResult.Pop()).number;
            stackForResult.Push(new Number(sum));
        }

    }

}