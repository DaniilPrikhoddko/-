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
            string nameOfMathOp = null;
            int count = 0;
            foreach (char i in userStr)
            {
                if (Char.IsDigit(i))
                {
                    if (token.Count>0 && count == 0 && token.Last() is Variable)
                    {
                        token.Add(new Operation('*'));
                    }
                    a += i;
                    count++;
                }
                else if (Char.IsLetter(i))
                {
                    nameOfMathOp += i;
                }
                else if (Char.IsLetter(i))
                {

                    token.Add(new Number(float.Parse(a)));
                    a = null;
                    count = 0;
                    if (token.Count > 0 && token.Last() is Number)
                    {
                        token.Add(new Operation('*'));
                    }
                    token.Add(new Variable(i));
                }
                else if (operators.Contains(i))
                {
                    if (a != null)
                    {
                        token.Add(new Number(float.Parse(a)));
                        token.Add(new Operation(i));
                        a = null;
                        count = 0;
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
                        count = 0;
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
                else if (tok is Variable variable)
                {
                    RPN.Add(variable);
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

        public static float CalculateExpression(List<Tokens> RPN, float valueOfVariable)
        {
            float result = 0;
            float intermediateValue = 0;
            Stack<Tokens> stackForResult = new Stack<Tokens>();
            foreach (var elem in RPN)
            {
                if (elem is Number)
                {
                    stackForResult.Push(elem);
                }
                else if (elem is Variable variable)
                {
                    stackForResult.Push(new Number(valueOfVariable));
                }

                else if (elem is Operation operation)
                {
                    if (operation.operation == '+')
                    {
                        intermediateValue = AddNum(((Number)stackForResult.Pop()).number, ((Number)stackForResult.Pop()).number);
                    }

                    else if (operation.operation == '-')
                    {
                        intermediateValue = SubtractNum(((Number)stackForResult.Pop()).number, ((Number)stackForResult.Pop()).number);
                    }

                    else if (operation.operation == '*')
                    {
                        intermediateValue = MultiplyNum(((Number)stackForResult.Pop()).number, ((Number)stackForResult.Pop()).number);
                    }

                    else if (operation.operation == '/')
                    {
                        intermediateValue = DivideNum(((Number)stackForResult.Pop()).number, ((Number)stackForResult.Pop()).number);
                    }

                    else if (operation.operation == '^')
                    {
                        intermediateValue = RaiseNumToPower(((Number)stackForResult.Pop()).number, ((Number)stackForResult.Pop()).number);
                    }
                    stackForResult.Push((new Number(intermediateValue)));

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
        public float Calculate(float valueOfVariable)
        {
            List<Tokens> token = new List<Tokens>();
            List<Tokens> RPN = new List<Tokens>();

            Tokenize(userInput, ref token);
            RPNImport(token, ref RPN);
            float result = CalculateExpression(RPN, valueOfVariable);
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
    public class Variable : Tokens
    {
        public char variableName { get; set;}
        public Variable(char name)
        {
            variableName = name;
        }
        public override string ToString()
        {
            return Convert.ToString(variableName);
        }
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