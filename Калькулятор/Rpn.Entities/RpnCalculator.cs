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
    public class RpnCalculator
    {
        private string userInput;
        public static void Tokenize(string userStr, ref List<Tokens> token)
        {
            userStr = userStr.Replace(" ", "");
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

        static void RPNImport(List<Tokens> token, ref List<Tokens> RPN)
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

        public static float CalculateExpression(List<Tokens> RPN)
        {
            float result = 0;
            float variable = 0;
            Stack<Tokens> stackForResult = new Stack<Tokens>();
            foreach (var elem in RPN)
            {
                if (elem is Number)
                    stackForResult.Push(elem);
                else if (elem is Operation operation)
                {
                    if (operation.operation == '+')
                    {
                        variable = AddNum(((Number)stackForResult.Pop()).number, ((Number)stackForResult.Pop()).number);
                    }

                    else if (operation.operation == '-')
                    {
                        variable = SubtractNum(((Number)stackForResult.Pop()).number, ((Number)stackForResult.Pop()).number);
                    }

                    else if (operation.operation == '*')
                    {
                        variable = MultiplyNum(((Number)stackForResult.Pop()).number, ((Number)stackForResult.Pop()).number);
                    }

                    else if (operation.operation == '/')
                    {
                        variable = DivideNum(((Number)stackForResult.Pop()).number, ((Number)stackForResult.Pop()).number);
                    }

                    else if (operation.operation == '^')
                    {
                        variable = RaiseNumToPower(((Number)stackForResult.Pop()).number, ((Number)stackForResult.Pop()).number);
                    }
                    stackForResult.Push((new Number(variable)));

                }

            }
            result = (float)((Number)stackForResult.Pop()).number;
            return result;
        }

        private static float RaiseNumToPower(float degree, float num)
        {

            float exponentiation = (float)Math.Pow(num, degree);
            return exponentiation;
        }

        private static float DivideNum(float divisor, float dividend)
        {
            float quotient = dividend / divisor;
            return quotient;
        }

        private static float MultiplyNum(float factor1, float factor2)
        {
            float multiplication = factor1 * factor2;
            return multiplication;
        }

        private static float SubtractNum(float subtrahend, float minuend)
        {
            float difference = minuend - subtrahend;
            return difference;
        }

        private static float AddNum(float term1, float term2)
        {
            float sum = term1 + term2;
            return sum;
        }
        public float Calculate()
        {
            List<Tokens> token = new List<Tokens>();
            List<Tokens> RPN = new List<Tokens>();

            Tokenize(userInput, ref token);
            RPNImport(token, ref RPN);
            float result = CalculateExpression(RPN);
            return result;
        }
        public RpnCalculator(string userStr)
        {
            userInput = userStr;
        }
    }

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
}