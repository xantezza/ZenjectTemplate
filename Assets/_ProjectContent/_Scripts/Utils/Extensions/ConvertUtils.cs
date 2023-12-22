using System;
using System.Globalization;
using System.Text;

namespace Utils.Extensions
{
    public enum ConvertFormat
    {
        Comas = 0,
        Kilos = 1
    }

    public static class ConvertUtils
    {
        private static readonly StringBuilder stringBuilder = new();

        public static string ToFormat(this int num, ConvertFormat format)
        {
            switch (format)
            {
                case ConvertFormat.Comas: return num.ToInvariantComasFormat();
                case ConvertFormat.Kilos: return num.ToKiloFormat();
            }

            return num.ToInvariantComasFormat();
        }

        /// <summary>
        ///     For proper parsing different float values from string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ToInvariantFloat(this string str)
        {
            return float.Parse(str.Replace(',', '.'), CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     For proper parsing different double values from string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ToInvariantDouble(this string str)
        {
            return double.Parse(str.Replace(',', '.'), CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Returns invariant int with comas between thousands
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToInvariantComasFormat(this int value)
        {
            return value.ToString("N0", CultureInfo.InvariantCulture);
        }

        public static string ToKiloFormat(this int num)
        {
            if (num >= 1000000) return (Math.Floor(num * 0.00001) * 0.1).ToString("0.#", CultureInfo.InvariantCulture) + "M";

            if (num >= 1000) return (Math.Floor(num * 0.01) * 0.1).ToString("0.#", CultureInfo.InvariantCulture) + "k";

            return num.ToString("#,0");
        }

        public static string ToKiloFormat2(this int num)
        {
            if (num >= 1000000000) return (num / 1000000000D).ToString("0.##B", CultureInfo.InvariantCulture);
            if (num >= 1000000) return (num / 1000000D).ToString("0.##M", CultureInfo.InvariantCulture);
            if (num >= 1000) return (num / 1000D).ToString("0.##k", CultureInfo.InvariantCulture);

            return num.ToString("#,0");
        }

        public static string ToSnakeCase(this object obj)
        {
            if (obj == null) return "";

            var text = obj.ToString();

            if (text.Length < 2) return text;

            stringBuilder.Clear();
            stringBuilder.Append(char.ToLowerInvariant(text[0]));

            var previousChar = ' ';

            for (var i = 1; i < text.Length; ++i)
            {
                var currentCharacter = text[i];

                if (char.IsUpper(currentCharacter)
                    || char.IsDigit(currentCharacter) && !char.IsDigit(previousChar)
                    || char.IsDigit(previousChar) && !char.IsDigit(currentCharacter))
                {
                    stringBuilder.Append('_');
                    stringBuilder.Append(char.ToLowerInvariant(currentCharacter));
                }
                else
                {
                    stringBuilder.Append(currentCharacter);
                }

                previousChar = currentCharacter;
            }

            return stringBuilder.ToString();
        }
    }
}