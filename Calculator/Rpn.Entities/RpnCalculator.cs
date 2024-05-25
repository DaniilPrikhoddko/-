using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Design;
using System.Globalization;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RPNCalc
{
    public class RpnCalculator
    {
        private string userInput;
        public static void Tokenize(string userStr, ref List<Tokens> token)
        {
            userStr = userStr.Replace(" ", "").ToLower();
            string operators = "+-*/^";
            string numbersStr = null;
            string lettersStr = string.Empty;


            foreach (char i in userStr)
            {
                if (Char.IsDigit(i))
                {
                    if (lettersStr.Length == 1)
                    {
                        token.Add(new Variable(Convert.ToChar(lettersStr)));
                        lettersStr = string.Empty;
                        token.Add(new Operation('*'));
                    } 
                    numbersStr += i;

                }
                else if (Char.IsLetter(i))
                {
                    lettersStr += i;
                }

                else if (operators.Contains(i))
                {
                    AddNumberAndLetterTokens(token, ref numbersStr, ref lettersStr);
                    token.Add(new Operation(i));

                }
                else if (i == '(' || i == ')')
                {

                    AddNumberAndLetterTokens(token, ref numbersStr, ref lettersStr);
                    token.Add(new Parenthesis(i));

                }
                else if (i == ',' || i == '.')
                {
                    numbersStr += ",";
                }

                else if (char.IsPunctuation(i))
                {
                    AddNumberAndLetterTokens(token, ref numbersStr, ref lettersStr);
                    token.Add(new Delimiter(i));
                }

                else
                {
                    Console.WriteLine("Ошибка: Недопустимое выражение");
                }

            }

            AddNumberAndLetterTokens(token, ref numbersStr, ref lettersStr);

            static void AddNumberAndLetterTokens(List<Tokens> token, ref string numbersStr, ref string lettersStr)
            {
                if (numbersStr != null)
                {
                    token.Add(new Number(numbersStr));
                    numbersStr = null;
                }
                if (lettersStr != string.Empty)
                {
                    if (lettersStr.Length == 1)
                    {
                        token.Add(new Variable(Convert.ToChar(lettersStr)));
                    }
                    else
                    {
                        token.Add(new Operation(lettersStr));
                    }
                    lettersStr = string.Empty;

                }
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

                else if (tok is Delimiter)
                {
                    while (stackForOp.Count > 0 && stackForOp.Peek() is Operation)
                    {
                        RPN.Add(stackForOp.Pop());
                        if (stackForOp.Count != 0 && stackForOp.Peek() is Operation)
                        {
                            priorityOfStackOperation = ((Operation)stackForOp.Peek()).priorityOfOperation;
                        }

                    }
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

        public float CalculateExpression(List<Tokens> RPN, float valueOfVariable)
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
                    string[] cotangentOperationNames = new string[] { "ctg", "cotg", "cotan", "ctan" };
                    string[] tangentOperationNames = new string[] { "tg", "tan" };
                    string[] rootOperationNames = new string[] { "rt", "root" };

                    if (rootOperationNames.Contains(operation.nameOfMathOperation))
                    {
                        float power = ((Number)stackForResult.Pop()).number;
                        intermediateValue = (float) Math.Pow(((Number)stackForResult.Pop()).number, 1 / power);
                    }

                    else if (operation.nameOfMathOperation == "cbrt")
                    {
                        intermediateValue = (float)Math.Cbrt(((Number)stackForResult.Pop()).number);
                    }

                    else if (operation.nameOfMathOperation == "sqrt")
                    {
                        intermediateValue = (float) Math.Sqrt(((Number)stackForResult.Pop()).number);
                    }

                    else if (cotangentOperationNames.Contains(operation.nameOfMathOperation))
                    {
                        intermediateValue = 1/(float)Math.Tan(((Number)stackForResult.Pop()).number);
                    }

                    else if (tangentOperationNames.Contains(operation.nameOfMathOperation))
                    {
                        intermediateValue = (float)Math.Tan(((Number)stackForResult.Pop()).number);
                    }

                    else if (operation.nameOfMathOperation == "cos")
                    {
                        intermediateValue = (float)Math.Cos(((Number)stackForResult.Pop()).number);
                    }

                    else if (operation.nameOfMathOperation == "sin")
                    {
                        intermediateValue = (float)Math.Sin(((Number)stackForResult.Pop()).number);
                    }

                    else if (operation.nameOfMathOperation == "log")
                    {
                        intermediateValue = (float)(1/Math.Log(((Number)stackForResult.Pop()).number) * Math.Log(((Number)stackForResult.Pop()).number));
                    }

                    else if (operation.nameOfMathOperation == "ln")
                    {
                        intermediateValue = (float)Math.Log(((Number)stackForResult.Pop()).number);
                    }

                    else if (operation.nameOfMathOperation == "lg")
                    {
                        intermediateValue = (float)Math.Log10(((Number)stackForResult.Pop()).number);
                    }

                    else if (operation.operation == '+')
                    {
                        intermediateValue = ((Number)stackForResult.Pop()).number + ((Number)stackForResult.Pop()).number;
                    }

                    else if (operation.operation == '-')
                    {
                        intermediateValue = -((Number)stackForResult.Pop()).number + ((Number)stackForResult.Pop()).number;
                    }

                    else if (operation.operation == '*')
                    {
                        intermediateValue = ((Number)stackForResult.Pop()).number * ((Number)stackForResult.Pop()).number;
                    }

                    else if (operation.operation == '/')
                    {
                        intermediateValue = 1/((Number)stackForResult.Pop()).number * ((Number)stackForResult.Pop()).number;
                    }

                    else if (operation.operation == '^')
                    {
                        float power = ((Number)stackForResult.Pop()).number;
                        intermediateValue = (float)Math.Pow(((Number)stackForResult.Pop()).number, power);
                    }
                    stackForResult.Push((new Number(intermediateValue)));

                }

            } 
            result = (float)((Number)stackForResult.Pop()).number;

            if (float.IsPositiveInfinity(result))
            {
                result = 16777215;
            }
            else if (float.IsNegativeInfinity(result))
            {
                result = -16777215;
            }
            return result;
        }

        public List<Tokens> GetRPN()
        {
            List<Tokens> token = new List<Tokens>();
            List<Tokens> RPN = new List<Tokens>();

            Tokenize(userInput, ref token);
            RPNImport(token, ref RPN);           
            return RPN;
        }
        public RpnCalculator(string userStr)
        {
            userInput = userStr;
        }
    }
}