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

            foreach (var timeMessageAppender in GetTimeMessageAppenders())
            {
                timeMessageAppender.AppendMessageBasedOnTime(minDiff, maxDiff, dateTimeDiff, ref uptimeMessage);
            }

            return uptimeMessage.Length > 0 ? FixGrammar(uptimeMessage.ToString()) : $"nog geen {TimeSpanConversionLimitToDutchText(minDiff, false)}";
        }

        private static IEnumerable<TimeMessageAppender> GetTimeMessageAppenders() =>
            new List<TimeMessageAppender>()
            {
                new TimeMessageAppender(TimeSpanConversionLimit.YEARS, "jaar", "jaar"),
                new TimeMessageAppender(TimeSpanConversionLimit.MONTHS, "maanden", "maand"),
                new TimeMessageAppender(TimeSpanConversionLimit.WEEKS, "weken", "week"),
                new TimeMessageAppender(TimeSpanConversionLimit.DAYS, "dagen", "dag"),
                new TimeMessageAppender(TimeSpanConversionLimit.HOURS, "uur", "uur"),
                new TimeMessageAppender(TimeSpanConversionLimit.MINUTES, "minuten", "minuut"),
                new TimeMessageAppender(TimeSpanConversionLimit.SECONDS, "seconden", "seconde"),
                new TimeMessageAppender(TimeSpanConversionLimit.MILLISECONDS, "milliseconden", "milliseconde")
            };

        private static string FixGrammar(string message)
        {

            if(message.Contains(',')) message = message.Substring(0, message.LastIndexOf(','));
            return message.Contains(",") ? message.ReplaceLastOccurrence(",", " en") : message;
        }

        public static string ReplaceLastOccurrence(this string source, string find, string replace)
        {
            var place = source.LastIndexOf(find, StringComparison.Ordinal);
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

    internal class TimeMessageAppender
    {
        private readonly TimeSpanConversionLimit _limit;
        private readonly string _pluralTimeSpan;
        private readonly string _singularTimeSpan;
        
        internal TimeMessageAppender(TimeSpanConversionLimit limit, string pluralTimeSpan, string singularTimeSpan)
        {
            this._limit = limit;
            this._pluralTimeSpan = pluralTimeSpan;
            this._singularTimeSpan = singularTimeSpan;
        }

        internal void AppendMessageBasedOnTime(TimeSpanConversionLimit minLimit, TimeSpanConversionLimit maxLimit, DateTimeSpan dateTimeSpan,
            ref StringBuilder uptimeBuilder)
        {
            var timeDiff = GetTimeDiff(this._limit, dateTimeSpan);
            if (timeDiff > 0 && ShouldAddThisLevelOfDetail(this._limit, minLimit, maxLimit))
            {
                uptimeBuilder.Append(timeDiff);
                uptimeBuilder.Append(timeDiff > 1 ? $" {this._pluralTimeSpan}, " : $" {this._singularTimeSpan}, ");
            }
        }

        private static int GetTimeDiff(TimeSpanConversionLimit limit, DateTimeSpan dateTimeDiff) =>
            limit switch
            {
                TimeSpanConversionLimit.MILLISECONDS => dateTimeDiff.Milliseconds,
                TimeSpanConversionLimit.SECONDS => dateTimeDiff.Seconds,
                TimeSpanConversionLimit.MINUTES => dateTimeDiff.Minutes,
                TimeSpanConversionLimit.HOURS => dateTimeDiff.Hours,
                TimeSpanConversionLimit.DAYS => dateTimeDiff.Days % 7,
                TimeSpanConversionLimit.WEEKS => dateTimeDiff.Days / 7,
                TimeSpanConversionLimit.MONTHS => dateTimeDiff.Months,
                TimeSpanConversionLimit.YEARS => dateTimeDiff.Years,
                _ => 0
            };

        private static bool ShouldAddThisLevelOfDetail(TimeSpanConversionLimit selected, 
            TimeSpanConversionLimit minDiff, TimeSpanConversionLimit maxDiff) =>
            minDiff <= selected && selected <= maxDiff;
    }

    public enum TimeSpanConversionLimit
    {
        MILLISECONDS, SECONDS, MINUTES, HOURS, DAYS, WEEKS, MONTHS, YEARS
    }
}
