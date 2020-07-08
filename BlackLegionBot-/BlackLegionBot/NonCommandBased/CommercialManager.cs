using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using BlackLegionBot.TwitchApi;

namespace BlackLegionBot.NonCommandBased
{
    public class CommercialManager
    {
        private readonly TwitchApiManager _apiClient;
        private DateTime _canStartAgainAt = DateTime.UnixEpoch;
        private Timer _commercialTimer;
        private readonly int _secondsBetweenCommercials = 30 * 60;

        public CommercialManager(TwitchApiManager apiClient)
        {
            this._apiClient = apiClient;
            SetTimer();
        }

        public async Task StartCommercial(ECommercialLength length = ECommercialLength.L30)
        {
            if (_canStartAgainAt.Date.ToUniversalTime() <= DateTime.UtcNow)
            {
                SetTimer();
                var result = await this._apiClient.StartCommercial(length);
                Console.WriteLine($"Started timer with length: {length}");
                _canStartAgainAt = DateTime.UtcNow.AddSeconds(result.RetryAfter);
            }
        }

        private void SetTimer()
        {
            this._commercialTimer = new Timer(_secondsBetweenCommercials * 1000)
            {
                AutoReset = false
            };
            this._commercialTimer.Elapsed += (sender, args) => StartCommercial(ECommercialLength.L30);
            this._commercialTimer.Start();

        }
    }

    public enum ECommercialLength
    {
        L30 = 30,
        L60 = 60,
        L90 = 90,
        L120 = 120,
        L150 = 150,
        L180 = 180
}
}
