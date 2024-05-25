namespace RPNCalc
{
    public class Number : Tokens
    {
        public float number = 0;
        public Number(string a)
        {
            number = float.Parse(a);
        }
        public Number(float num)
        {
            number = num;
        }
        public override string ToString()
        {
            return Convert.ToString(number);
        }
    }
}