namespace RPNCalc
{
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
}