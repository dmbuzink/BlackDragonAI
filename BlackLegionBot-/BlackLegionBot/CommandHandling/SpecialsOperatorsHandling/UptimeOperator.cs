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

                message = message.Replace(Operator, streamData.StartedAt.ConvertToDifferenceFromNowInDutch(TimeSpanConversionLimit.SECONDS, TimeSpanConversionLimit.HOURS));
            }

            return message;
        }
    }
}