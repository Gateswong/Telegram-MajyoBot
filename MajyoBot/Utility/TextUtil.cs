using System;
using System.Linq;

namespace MajyoBot.Utility
{
    public static class TextUtil
    {
        public static string TrimFirstWord(this string text) 
        {
            return new string(text
                .Skip(text.IndexOf(" ", StringComparison.Ordinal) + 1)
                .ToArray()).Trim();
        }
    }
}
