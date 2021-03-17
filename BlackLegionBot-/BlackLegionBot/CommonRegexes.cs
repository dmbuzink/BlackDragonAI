using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BlackLegionBot
{
    public static class CommonRegexes
    {
        public static readonly Regex UsernameRegex = new Regex("[a-zA-Z0-9_]+");
        public static readonly Regex UsernameWithAtSymbol = new Regex("@[a-zA-Z0-9_]+");
        public static readonly Regex UsernameWithOptionalAtSymbol = new Regex("@?[a-zA-Z0-9_]+");
        public static readonly Regex EmoteRegex = new Regex("bkdn[a-zA-Z0-9_]+");
        public static readonly Regex Numerics = new Regex("[0-9]+");

        public static bool IsCompleteMatch(this Regex regex, string input)
        {
            var match = regex.Match(input);
            return match.Success && match.Value.Equals(input);
        }
    }
}
