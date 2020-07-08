using System;
using System.Collections.Generic;
using System.Text;

namespace BlackLegionBot.CommandHandling.SpecialsOperatorsHandling
{
    public static class DateTimeExtensions
    {
        public static string ConvertToDifferenceFromNowInDutch(this DateTime dateTime, TimeSpanConversionLimit minDiff, TimeSpanConversionLimit maxDiff)
        {
            var timeSpan = DateTime.UtcNow - dateTime.ToUniversalTime();
            var uptimeMessage = new StringBuilder();

            var years = Math.Floor(timeSpan.Days / 365M);
            if (years > 0 && ShouldAddThisLevelOfDetail(TimeSpanConversionLimit.Years, minDiff, maxDiff))
            {
                uptimeMessage.Append(years);
                uptimeMessage.Append(" jaar, ");
            }
            var weeks = Math.Floor((timeSpan.Days % 365) / 7M);
            if (weeks > 0 && ShouldAddThisLevelOfDetail(TimeSpanConversionLimit.Weeks, minDiff, maxDiff))
            {
                uptimeMessage.Append(weeks);
                uptimeMessage.Append(weeks > 1 ? " weken, " : " week, ");
            }
            var days = timeSpan.Days % 7;
            if (days > 0 && ShouldAddThisLevelOfDetail(TimeSpanConversionLimit.Days, minDiff, maxDiff))
            {
                uptimeMessage.Append(days);
                uptimeMessage.Append(days > 1 ? " dagen, " : " dag, ");
            }
            if (timeSpan.Hours > 0 && ShouldAddThisLevelOfDetail(TimeSpanConversionLimit.Hours, minDiff, maxDiff))
            {
                uptimeMessage.Append($"{timeSpan.Hours} uur, ");
            }
            if (timeSpan.Minutes > 0 && ShouldAddThisLevelOfDetail(TimeSpanConversionLimit.Minutes, minDiff, maxDiff))
            {
                uptimeMessage.Append(timeSpan.Minutes);
                uptimeMessage.Append(timeSpan.Minutes > 1 ? " minuten, " : " minuut, ");
            }
            if (timeSpan.Seconds > 0 && ShouldAddThisLevelOfDetail(TimeSpanConversionLimit.Seconds, minDiff, maxDiff))
            {
                uptimeMessage.Append(timeSpan.Seconds);
                uptimeMessage.Append(timeSpan.Seconds > 1 ? " seconden, " : " seconde, ");
            }

            return FixGrammar(uptimeMessage.ToString());
        }

        private static string FixGrammar(string message)
        {
            message = message.Substring(0, message.LastIndexOf(','));
            return message.ReplaceLastOccurrence(",", " en");
        }

        private static bool ShouldAddThisLevelOfDetail(TimeSpanConversionLimit selected, 
            TimeSpanConversionLimit minDiff, TimeSpanConversionLimit maxDiff) =>
                minDiff <= selected && selected <= maxDiff;

        public static string ReplaceLastOccurrence(this string source, string find, string replace)
        {
            var place = source.LastIndexOf(find);
            var result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }
    }

    public enum TimeSpanConversionLimit
    {
        Seconds, Minutes, Hours, Days, Weeks, Years
    }
}
