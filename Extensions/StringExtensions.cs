using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Extensions
{
    /// <summary>
    /// Class that provides extension methods for System.String.
    /// </summary>
    public static class StringExtensions
    {
        #region Public API
        /// <summary>
        /// Public method that convert System.String value to System.Int32 value.
        /// </summary>
        /// <param name="number">string value that should be converted to Int.32</param>
        /// <param name="scale">Scale of notation.</param>
        /// <exception cref="ArgumentException">Throws when scale gets incorrect value or scale has value "2" and number isn't binary.</exception>
        /// <exception cref="OverflowException">Throws when the number.Length value is bigger than 32(bits in Int32).</exception>
        /// <returns>Converted value.</returns>
        public static int ConvertToInt(this string number, int scale)
        {
            if(number == null || number.Equals(""))
            {
                throw new ArgumentNullException(nameof(number) + " can't be equal to null or empry.");
            }

            if(scale < 2 || scale > 16)
            {
                throw new ArgumentException(nameof(scale) + " should be in range [2;16].");
            }

            if(number.Length >= sizeof(int) * 8)
            {
                throw new OverflowException("Length of " + nameof(number) + "for scale = " + scale + " should be less than 32!");
            }

            if(!IsBinaryValue(number) && scale == 2)
            {
                throw new ArgumentException(nameof(number) + " isn't represented in binary scale!");
            }

            return number.ToInt(scale);
        }
        #endregion

        #region Private API
        /// <summary>
        /// Private method that convert System.String value to System.Int32 value.
        /// </summary>
        /// <param name="number">string value that should be converted to Int.32</param>
        /// <param name="scale">Scale of notation.</param>
        /// <returns>Converted value.</returns>
        private static int ToInt(this string number, int scale)
        {
            char[] chars = number.ToCharArray();
            Array.Reverse(chars);

            return MakeIntFromChars(chars, scale);
        }

        /// <summary>
        /// Private method that convert char[] to int.
        /// </summary>
        /// <param name="chars">char[] representing the number.</param>
        /// <param name="scale">Scale of notation.</param>
        /// <returns>Converted value.</returns>
        private static int MakeIntFromChars(char[] chars, int scale)
        {
            int result = 0;

            for (int i = 0; i < chars.Length; i++)
            {
                result += (CharToInt(chars[i]) * (int)Math.Pow(scale, i));
            }

            return result;
        }

        /// <summary>
        /// Private method that convert char value to int.
        /// </summary>
        /// <param name="c">char value.</param>
        /// <returns>int value.</returns>
        private static int CharToInt(char c)
        {
            if (c >= '0' && c <= '9')
            {
                return c - '0';
            }
            else if (c >= 'a' && c <= 'f')
            {
                return 10 + c - 'a';
            }
            else if (c >= 'A' && c <= 'F')
            {
                return 10 + c - 'A';
            }

            return -1;
        }

        /// <summary>
        /// Private method that check if string is representing binary number.
        /// </summary>
        /// <param name="number">Needed string.</param>
        /// <returns>bool value.</returns>
        private static bool IsBinaryValue(string number)
        {
            foreach (char c in number)
            {
                if (c != '0' && c != '1')
                {
                    return false;
                }
            }
                
            return true;
        }
        #endregion
    }
}