using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEngine;

namespace Utils.Extensions
{
    [Serializable]
    public class StringReplacer
    {
        public string From;
        public string To;

        public static StringReplacer Create(string from, string to)
        {
            return new StringReplacer {From = from, To = to};
        }
    }

    public static class StringExtensions
    {
        public static void InsertStringAtStart(this char[] targetArray, string str)
        {
            if (str == null || targetArray == null)
            {
                throw new ArgumentNullException("Arguments cannot be null.");
            }

            int strLength = str.Length;

            if (strLength + 1 > targetArray.Length)
            {
                throw new ArgumentException("Target array is not large enough to hold the inserted string.");
            }

            for (int i = targetArray.Length - 1; i >= strLength; i--)
            {
                targetArray[i] = targetArray[i - strLength];
            }

            for (int i = 0; i < strLength; i++)
            {
                targetArray[i] = str[i];
            }
        }

        public static void FloatToStringNonAlloc(float number, char[] buffer, int maxFractionDigits = 3)
        {
            int index = 0;
            bool isNegative = number < 0;
            if (isNegative)
            {
                number = -number;
                buffer[index++] = '-';
            }

            int intPart = (int) number;
            float fractionalPart = number - intPart;

            index += IntToStringNonAlloc(intPart, buffer, index);

            if (index < buffer.Length)
            {
                buffer[index++] = '.';
            }
            else
            {
                throw new InvalidOperationException("Buffer overflow while adding decimal point.");
            }

            for (int i = 0; i < maxFractionDigits; i++)
            {
                fractionalPart *= 10;
                int fractionalDigit = (int) fractionalPart;

                if (index < buffer.Length)
                {
                    buffer[index++] = (char) ('0' + fractionalDigit);

                    fractionalPart -= fractionalDigit;
                }
                else
                {
                    throw new InvalidOperationException("Buffer overflow while adding fractional digits.");
                }
            }

            if (index < buffer.Length)
            {
                buffer[index] = default;
            }
        }

        public static int IntToStringNonAlloc(int number, char[] buffer, int index = 0)
        {
            int startIndex = index;
            bool isNegative = number < 0;

            if (isNegative)
            {
                number = -number;
            }

            do
            {
                buffer[index++] = (char) ('0' + (number % 10));
                number /= 10;
            } while (number > 0);

            if (isNegative)
            {
                buffer[index++] = '-';
            }

            Array.Reverse(buffer, startIndex, index - startIndex);

            return index - startIndex;
        }

        private static int GetStringLength(int number)
        {
            if (number == 0) return 1;

            int length = 0;
            if (number < 0)
            {
                length++;
                number = -number;
            }

            while (number > 0)
            {
                number /= 10;
                length++;
            }

            return length;
        }

        public static string Replace(this string stringToModificate, IEnumerable<StringReplacer> replacers)
        {
            if (stringToModificate.IsNullOrWhitespace() || !replacers.Any() || replacers.Any(x => x == null)) return stringToModificate;

            var str = new StringBuilder(stringToModificate);
            replacers.ForEach(x => str.Replace(x.From, x.To));
            return str.ToString();
        }

        public static string JoinStrings<T>(this IEnumerable<T> source, Func<T, string> projection, string separator)
        {
            if (source == null) return null;
            var builder = new StringBuilder();
            var first = true;
            foreach (var element in source)
            {
                if (first)
                    first = false;
                else
                    builder.Append(separator);

                builder.Append(projection(element));
            }

            return builder.ToString();
        }

        public static string JoinStrings<T>(this IEnumerable<T> source, string separator)
        {
            return JoinStrings(source, t => t.ToString(), separator);
        }

        /// <summary>
        ///     Returns true if this string is null, empty, or contains only whitespace.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns><c>true</c> if this string is null, empty, or contains only whitespace; otherwise, <c>false</c>.</returns>
        public static bool IsNullOrWhitespace(this string str)
        {
            if (!string.IsNullOrEmpty(str))
                for (var index = 0; index < str.Length; ++index)
                    if (!char.IsWhiteSpace(str[index]))
                        return false;
            return true;
        }
    }
}