using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace калькулятор
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Введите выражение:");
            string userStr = Console.ReadLine();

            List<object> tokens = Tokenization(userStr);
            PrintOutput(tokens);
            Console.WriteLine();
        }

        private static void PrintOutput(List<object> tokens)
        {
            Console.WriteLine("");
            Console.WriteLine("Ответ:");
            RPNCalculator(RPNImport(tokens));
        }

        private static List<object> Tokenization(string userStr)
        {
            userStr = userStr.Replace(" ", "");
            string operators = "+-*/^()";
            List<object> tokens = new List<object>();
            string a = null;

            for (int i = 0; i < userStr.Length; i++)
            {
                if (Char.IsDigit(userStr[i]))
                {
                    a += userStr[i];
                }

                else if (operators.Contains(userStr[i]))
                {
                    if (a != null)
                    {
                        tokens.Add(a);
                    }

                    tokens.Add(userStr[i]);
                    a = null;
                }

                else
                {
                    a += userStr[i];
                }

            }

            if (a != null)
            {
                tokens.Add(a);
            }

            return tokens;
        }

        static List<object> RPNImport(List<object> tokens)
        {
            Dictionary<string, int> OperationPriority = new Dictionary<string, int>()
            {
                {"+", 2 },
                {"-", 2 },
                {"*", 3 },
                {"/", 3 },
                {"^", 4 },
                {"(", 0 },
                {")", 1 }
            };

            List<object> RPN = new List<object>();
            Stack<object> stackForOp = new Stack<object>();
            for (int i = 0; i < tokens.Count; i++)
            {
                if (float.TryParse(tokens[i].ToString(), out float n) == true)
                {
                    RPN.Add(tokens[i]);
                }

                else if (float.TryParse(tokens[i].ToString(),out float n1) == false)
                {
                    if (stackForOp.Count == 0
                        || (OperationPriority[tokens[i].ToString()] > OperationPriority[stackForOp.Peek().ToString()])
                        || (OperationPriority[tokens[i].ToString()] == 0))
                    {
                        stackForOp.Push(tokens[i]);
                    }

                    else if (OperationPriority[tokens[i].ToString()] == 1)
                    {
                        while (OperationPriority[stackForOp.Peek().ToString()] > 0)
                        {
                            RPN.Add(stackForOp.Pop());
                        }

                        stackForOp.Pop();
                    }

                    else if (OperationPriority[tokens[i].ToString()] <= OperationPriority[stackForOp.Peek().ToString()] && OperationPriority[tokens[i].ToString()] != 0)
                    {
                        while (stackForOp.Count > 0)
                        {
                            if (OperationPriority[tokens[i].ToString()] == OperationPriority[stackForOp.Peek().ToString()])
                            {
                                RPN.Add(stackForOp.Pop());
                                break;
                            }

                            else if (OperationPriority[stackForOp.Peek().ToString()] == 0)
                            {
                                break;
                            }

                            else
                            {
                                RPN.Add(stackForOp.Pop());
                            } 
                            
                        }

                        stackForOp.Push(tokens[i]);
                    }
                
                }

            }

            if (stackForOp.Count > 0)
            {
                while (stackForOp.Count != 0)
                {
                    RPN.Add(stackForOp.Pop());
                }

            }

            return RPN;
            
        }

        public static double RPNCalculator(List<object> RPN)
        {
            Stack<object> stackForResult = new Stack<object>();

            foreach (var item in RPN)
            {
                string elem = Convert.ToString(item);
                if (double.TryParse(elem, out var value))
                {
                    stackForResult.Push(value);
                }

                else
                {
                    if (elem == "^")
                    {
                        Exponentiation(stackForResult);
                    }

                    else if (elem == "*")
                    {
                        Multiplication(stackForResult);
                    }

                    else if (elem == "/")
                    {
                        Division(stackForResult);
                    }

                    else if (elem == "+")
                    {
                        Addition(stackForResult);
                    }

                    else if (elem == "-")
                    {
                        Subtraction(stackForResult);
                    }

                }

            } 

            double result = Convert.ToDouble(stackForResult.Pop());
            Console.WriteLine(result);
            return result;
        }

        private static void Subtraction(Stack<object> stackForResult)
        {
            double intermediateResult = -(Convert.ToDouble(stackForResult.Pop()) - Convert.ToDouble(stackForResult.Pop()));
            stackForResult.Push(intermediateResult);
        }

        private static void Addition(Stack<object> stackForResult)
        {
            double intermediateResult = Convert.ToDouble(stackForResult.Pop()) + Convert.ToDouble(stackForResult.Pop());
            stackForResult.Push(intermediateResult);
        }

        private static void Division(Stack<object> stackForResult)
        {
            double intermediateResult = 1f / (Convert.ToDouble(stackForResult.Pop()) / Convert.ToDouble(stackForResult.Pop()));
            stackForResult.Push(intermediateResult);
        }

        private static void Multiplication(Stack<object> stackForResult)
        {
            double intermediateResult = Convert.ToDouble(stackForResult.Pop()) * Convert.ToDouble(stackForResult.Pop());
            stackForResult.Push(intermediateResult);
        }

        private static void Exponentiation(Stack<object> stackForResult)
        {
            double a = Convert.ToDouble(stackForResult.Pop());
            double intermediateResult = Math.Pow(Convert.ToDouble(stackForResult.Pop()), a);
            stackForResult.Push(intermediateResult);
        }

    }

}