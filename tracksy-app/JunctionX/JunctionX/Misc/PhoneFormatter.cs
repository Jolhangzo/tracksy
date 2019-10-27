using System.Linq;
using System.Text;

namespace JunctionX.Misc
{
    public class PhoneFormatter
    {
        public static string FormatUsa(string s)
        {
            if (!IsCorrect(s.ToString()))
                s = ToCorrect(s.Where(char.IsDigit).Take(10).ToArray());
            return s;
        }

        private static string ToCorrect(char[] digits)
        {
            var formatted = new StringBuilder();

            for (int i = 0; i < digits.Length; i++)
            {
                if (i == 0)
                    formatted.Append('(');
                formatted.Append(digits[i]);
                if (i == 2)
                {
                    formatted.Append(')');
                    formatted.Append(' ');
                }
                if (i == 5)
                    formatted.Append('-');
                if (i == 10)
                    formatted.Remove(14, 1);
            }

            return formatted.ToString();
        }

        private static bool IsCorrect(string s)
        {
            if (s.Length > 14)
                return false;
            for (int i = 0; i < s.Length; i++)
            {
                var c = s[i];
                switch (i)
                {
                    case 0:
                        if (c != '(')
                            return false;
                        break;
                    case 4:
                        if (c != ')')
                            return false;
                        break;
                    case 5:
                        if (c != ' ')
                            return false;
                        break;
                    case 9:
                        if (c != '-')
                            return false;
                        break;
                    default:
                        if (!char.IsDigit(c))
                            return false;
                        break;
                }
            }

            return true;
        }
    }
}
