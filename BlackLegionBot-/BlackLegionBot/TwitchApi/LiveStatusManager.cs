using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlackLegionBot.TwitchApi
{
    public class LiveStatusManager
    {
        private LiveStatusCheck _liveStatusCheck;
        private static readonly int _minutesBetweenChecks = 10;
        private readonly TwitchApiManager _twitchApi;

        public LiveStatusManager(TwitchApiManager twitchApi)
        {
            this._twitchApi = twitchApi;
        }

        public async Task<bool> IsLive()
        {
            if (_liveStatusCheck == null || MinutesBetweenNowAndDate(_liveStatusCheck.AtTime) > _minutesBetweenChecks)
            {
                _liveStatusCheck = new LiveStatusCheck()
                {
                    AtTime = DateTime.UtcNow,
                    IsLiveTask = _twitchApi.IsLive()
                };
            }

            var isLiveVar = await this._liveStatusCheck.IsLiveTask;
            Console.WriteLine($"Stream is live is {isLiveVar}");
            return isLiveVar;
        }

        private class LiveStatusCheck
        {
            public Task<bool> IsLiveTask { get; set; }
            public DateTime AtTime { get; set; }
        }

        private int MinutesBetweenNowAndDate(DateTime dateTime)
        {
            var minutesBetweenDates = (int) Math.Floor(DateTime.UtcNow.Subtract(dateTime.ToUniversalTime()).TotalMinutes);
            Console.WriteLine($"Minutes between dates: {minutesBetweenDates}");
            return minutesBetweenDates;
        }
    }
}
