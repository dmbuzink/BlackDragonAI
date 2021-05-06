using System;
using System.Collections.Generic;
using System.Text;

namespace BlackLegionBot.CommandHandling.SpecialsOperatorsHandling
{
    public static class DateTimeExtensions
    {
        public static string ConvertToDifferenceFromNowInDutch(this DateTime dateTime, TimeSpanConversionLimit minDiff, TimeSpanConversionLimit maxDiff)
        {
            var dateTimeDiff = DateTimeSpan.CompareDates(dateTime.ToUniversalTime(), DateTime.UtcNow);
            var uptimeMessage = new StringBuilder();

            // YEARS
            if (dateTimeDiff.Years > 0 && ShouldAddThisLevelOfDetail(TimeSpanConversionLimit.YEARS, minDiff, maxDiff))
            {
                uptimeMessage.Append(dateTimeDiff.Years);
                uptimeMessage.Append(" jaar, ");
            }
            // MONTHS
            if (dateTimeDiff.Months > 0 && ShouldAddThisLevelOfDetail(TimeSpanConversionLimit.MONTHS, minDiff, maxDiff))
            {
                uptimeMessage.Append(dateTimeDiff.Months);
                uptimeMessage.Append(dateTimeDiff.Months > 1 ? " maanden, " : " maand, ");
            }
            // WEEKS
            var weeks = dateTimeDiff.Days / 7;
            if (weeks > 0 && ShouldAddThisLevelOfDetail(TimeSpanConversionLimit.WEEKS, minDiff, maxDiff))
            {
                uptimeMessage.Append(weeks);
                uptimeMessage.Append(weeks > 1 ? " weken, " : " week, ");
            }
            var days = dateTimeDiff.Days % 7;
            // DAYS
            if (days > 0 && ShouldAddThisLevelOfDetail(TimeSpanConversionLimit.DAYS, minDiff, maxDiff))
            {
                uptimeMessage.Append(days);
                uptimeMessage.Append(days > 1 ? " dagen, " : " dag, ");
            }
            // HOURS
            if (dateTimeDiff.Hours > 0 && ShouldAddThisLevelOfDetail(TimeSpanConversionLimit.HOURS, minDiff, maxDiff))
            {
                uptimeMessage.Append($"{dateTimeDiff.Hours} uur, ");
            }
            // MINUTES
            if (dateTimeDiff.Minutes > 0 && ShouldAddThisLevelOfDetail(TimeSpanConversionLimit.MINUTES, minDiff, maxDiff))
            {
                uptimeMessage.Append(dateTimeDiff.Minutes);
                uptimeMessage.Append(dateTimeDiff.Minutes > 1 ? " minuten, " : " minuut, ");
            }
            // SECONDS
            if (dateTimeDiff.Seconds > 0 && ShouldAddThisLevelOfDetail(TimeSpanConversionLimit.SECONDS, minDiff, maxDiff))
            {
                uptimeMessage.Append(dateTimeDiff.Seconds);
                uptimeMessage.Append(dateTimeDiff.Seconds > 1 ? " seconden, " : " seconde, ");
            }
            // MILLISECONDS
            if (dateTimeDiff.Milliseconds > 0 && ShouldAddThisLevelOfDetail(TimeSpanConversionLimit.MILLISECONDS, minDiff, maxDiff))
            {
                uptimeMessage.Append(dateTimeDiff.Milliseconds);
                uptimeMessage.Append(dateTimeDiff.Milliseconds > 1 ? " milliseconden, " : " milliseconde, ");
            }
            return uptimeMessage.Length > 0 ? FixGrammar(uptimeMessage.ToString()) : $"nog geen {TimeSpanConversionLimitToDutchText(minDiff, false)}";
        }

        private static string FixGrammar(string message)
        {

            if(message.Contains(',')) message = message.Substring(0, message.LastIndexOf(','));
            return message.Contains(",") ? message.ReplaceLastOccurrence(",", " en") : message;
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

        public static string TimeSpanConversionLimitToDutchText(TimeSpanConversionLimit tscl, bool inPlural) =>
            tscl switch
            {
                TimeSpanConversionLimit.MILLISECONDS => inPlural ? "milliseconden" : "milliseconde", 
                TimeSpanConversionLimit.SECONDS => inPlural ? "seconden" : "seconde", 
                TimeSpanConversionLimit.MINUTES => inPlural ? "minuten" : "minuut", 
                TimeSpanConversionLimit.HOURS => inPlural ? "uren" : "uur", 
                TimeSpanConversionLimit.DAYS => inPlural ? "dagen" : "dag", 
                TimeSpanConversionLimit.WEEKS => inPlural ? "weken" : "week", 
                TimeSpanConversionLimit.MONTHS => inPlural ? "maanden" : "maand", 
                TimeSpanConversionLimit.YEARS => inPlural ? "jaren" : "jaar",
                _ => string.Empty
            };
    }

    public enum TimeSpanConversionLimit
    {
        MILLISECONDS, SECONDS, MINUTES, HOURS, DAYS, WEEKS, MONTHS, YEARS
    }
}
