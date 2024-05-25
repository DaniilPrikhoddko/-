namespace RPNCalc
{
    public class Operation : Tokens
    {
        public char operation = ' ';
        public int priorityOfOperation = 0;
        public string nameOfMathOperation = null;
        public Operation(char i)
        {
            operation = i;
            priorityOfOperation = ConvertOperarionToPriority(i);

        }
        public Operation(string i)
        {
            nameOfMathOperation = i;
            priorityOfOperation = 5;
        }
        public override string ToString()
        {
            if (nameOfMathOperation == null)
            {
                return Convert.ToString(operation);
            }
            else
            {
                return Convert.ToString(nameOfMathOperation);
            }   
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