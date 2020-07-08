using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackLegionBot.TwitchApi;
using TwitchLib.Api.Interfaces;

namespace BlackLegionBot.CommandHandling.SpecialsOperatorsHandling
{
    public class UptimeOperator : ISpecialOperator
    {
        private const string Operator = "$uptime";
        private TwitchApiManager TwitchApi { get; }

        public UptimeOperator(TwitchApiManager twitchApi)
        {
            this.TwitchApi = twitchApi;
        }

        public string GetOperatorName() => Operator;

        public async Task<string> InjectOperatorAsync(string message, string username, string originalCommand)
        {
            if (message.Contains(Operator))
            {
                var streamData = await this.TwitchApi.GetStreamData();
                if (streamData == null)
                {
                    return "De stream is helaas momenteel niet live. Probeer het opnieuw wanneer de stream live is.";
                }

                message = message.Replace(Operator, streamData.StartedAt.ConvertToDifferenceFromNowInDutch(TimeSpanConversionLimit.Seconds, TimeSpanConversionLimit.Hours));
            }

            return message;
        }
    }

//    public static class DateTimeExtensions
//    {
//        public static string GetTimeDifferenceInDutch(this DateTime dateTime)
//        {
//            var diff = dateTime - DateTime.Now;
//            
//            var uptimeMessage = new StringBuilder();
//            if (diff.Days > 0)
//            {
//                uptimeMessage.Append(diff.Days);
//                uptimeMessage.Append(diff.Days > 1 ? " dagen, " : " dag, ");
//            }
//            if (diff.Hours > 0)
//            {
//                uptimeMessage.Append($"{diff.Hours} uur, ");
//            }
//            if (diff.Minutes > 0)
//            {
//                uptimeMessage.Append(diff.Minutes);
//                uptimeMessage.Append(diff.Minutes > 1 ? " minuten, " : " minuut, ");
//            }
//            if (diff.Seconds > 0)
//            {
//                uptimeMessage.Append(diff.Seconds);
//                uptimeMessage.Append(diff.Seconds > 1 ? " seconden, " : " seconde, ");
//            }
//
//            return FixGrammar(uptimeMessage.ToString());
//        }
//
//        private static string FixGrammar(string message)
//        {
//            message = message.Substring(0, message.LastIndexOf(','));
//            return message.Replace(",", " en");
//        }
//    }
}