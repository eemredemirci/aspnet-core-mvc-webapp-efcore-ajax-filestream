using System;

namespace Odev_2.Extensions
{
    public static class StringExtension
    {
        public static string ToFirstName(this string userName)
        {
            string dot = ".";

            string name = string.Empty;

            foreach (char item in userName)
            {
                if (dot.Contains(item))
                {
                    break;
                }
                else
                {
                    name += item;
                }
            }

            return name;
        }

        public static string ToLastName(this string userName)
        {
            string dot = ".";
            bool isAfterDot = false;

            string surname = string.Empty;
            foreach (char item in userName)
            {
                if (!isAfterDot)
                {
                    if (dot.Contains(item))
                    {
                        isAfterDot = true;
                        continue;
                    }
                }
                else

                {
                    surname += item;
                }
            }
            return surname;
        }

        public static string FirstCharToUpper(this string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
        };

        public static string FirstCharToLower(this string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => string.Concat(input[0].ToString().ToLower(), input.AsSpan(1))
        };
    }
}
