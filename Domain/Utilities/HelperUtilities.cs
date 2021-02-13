using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Utilities
{
    public static class HelperUtilities
    {
        static public string RandomString(int size, bool lowerCase = false)
        {
            Random _random = new Random();
            var builder = new StringBuilder(size);
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26;
            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
    }
}
