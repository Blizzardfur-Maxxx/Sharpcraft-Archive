using System;

namespace SharpCraft.Core.Util
{
    public class Base36
    {
        private const int BitsInLong = 64;
        private const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string ToString(long decimalNumber, int radix)
        {
            if (radix < 2 || radix > Digits.Length)
                throw new ArgumentException("The radix must be >= 2 and <= " + Digits.Length.ToString());
            if (decimalNumber == 0)
                return "0";
            int index = BitsInLong - 1;
            long currentNumber = Math.Abs(decimalNumber);
            char[] charArray = new char[BitsInLong];
            while (currentNumber != 0)
            {
                int remainder = (int)(currentNumber % radix);
                charArray[index--] = Digits[remainder];
                currentNumber = currentNumber / radix;
            }
            string result = new string(charArray, index + 1, BitsInLong - index - 1);
            if (decimalNumber < 0)
            {
                result = "-" + result;
            }
            return result.ToLowerInvariant(); //for compatibility
        }

        public static long FromString(string number, int radix)
        {
            if (radix < 2 || radix > Digits.Length)
                throw new ArgumentException("The radix must be >= 2 and <= " + Digits.Length.ToString());
            if (string.IsNullOrEmpty(number))
                return 0;
            // Make sure the arbitrary numeral system number is in upper case
            number = number.ToUpperInvariant();
            long result = 0;
            long multiplier = 1;
            for (int i = number.Length - 1; i >= 0; i--)
            {
                char c = number[i];
                if (i == 0 && c == '-')
                {
                    // This is the negative sign symbol
                    result = -result;
                    break;
                }
                int digit = Digits.IndexOf(c);
                if (digit == -1)
                    throw new ArgumentException("Invalid character in the arbitrary numeral system number", "number");
                result += digit * multiplier;
                multiplier *= radix;
            }
            return result;
        }
    }
}
