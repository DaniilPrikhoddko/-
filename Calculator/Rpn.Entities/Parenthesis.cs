namespace RPNCalc
{
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
}