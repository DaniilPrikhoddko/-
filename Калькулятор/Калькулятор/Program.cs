using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace калькулятор
{
    public class Parenthesis
    {
        public List<string> parnthesis;
    }
    public class Nubmer
    {

    }
    public class Operation
    {

    }
    public class Tokens
    {
        public static List<object> tokens;
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Parenthesis> parentheses = new List<Parenthesis>();
            

            Console.WriteLine("Введите выражение:");
            string userStr = Console.ReadLine();
            List<Tokens> tokens = new List<Tokens>();  
            Tokenize(userStr);
            PrintOutput(Tokens.tokens);
            Console.WriteLine();
            
        }

        private static void PrintOutput(List<Tokens> tokens)
        {
            Console.WriteLine("");
            Console.WriteLine("Ответ:");
            RPNCalculator(RPNImport(Tokens.tokens));
        }

        private static void Tokenize(string userStr)
        {
            userStr = userStr.Replace(" ", "");
            string operators = "+-*/^()";
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
                        GetTokens().Add(a);
                    }

                    GetTokens().Add(userStr[i]);
                    a = null;
                }

                else
                {
                    a += userStr[i];
                }

            }

            if (a != null)
            {
                GetTokens().Add(a);
            }

            static List<object> GetTokens()
            {
                return Tokens.tokens;
            }
        }

        static List<object> RPNImport(List<Tokens> tokens)
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
            for (int i = 0; i < Tokens.tokens.Count; i++)
            {
                if (float.TryParse(Tokens.tokens[i].ToString(), out float n) == true)
                {
                    RPN.Add(Tokens.tokens[i]);
                }

                else if (float.TryParse(Tokens.tokens[i].ToString(),out float n1) == false)
                {
                    if (stackForOp.Count == 0
                        || (OperationPriority[Tokens.tokens[i].ToString()] > OperationPriority[stackForOp.Peek().ToString()])
                        || (OperationPriority[Tokens.tokens[i].ToString()] == 0))
                    {
                        stackForOp.Push(Tokens.tokens[i]);
                    }

                    else if (OperationPriority[Tokens.tokens[i].ToString()] == 1)
                    {
                        while (OperationPriority[stackForOp.Peek().ToString()] > 0)
                        {
                            RPN.Add(stackForOp.Pop());
                        }

                        stackForOp.Pop();
                    }

                    else if (OperationPriority[Tokens.tokens[i].ToString()] <= OperationPriority[stackForOp.Peek().ToString()] && OperationPriority[Tokens.tokens[i].ToString()] != 0)
                    {
                        while (stackForOp.Count > 0)
                        {
                            if (OperationPriority[Tokens.tokens[i].ToString()] == OperationPriority[stackForOp.Peek().ToString()])
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

                        stackForOp.Push(Tokens.tokens[i]);
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

        public static void RPNCalculator(List<object> RPN)
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