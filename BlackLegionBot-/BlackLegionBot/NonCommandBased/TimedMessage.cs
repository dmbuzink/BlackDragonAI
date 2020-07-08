using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace BlackLegionBot.NonCommandBased
{
    public class TimedMessage
    {
        private readonly Timer _timer;
        private readonly Timer _offsetTimer;

        public TimedMessage(int secondsBetweenEvents, string message, Action<string> sendMessage, int offsetInMinutes = 0) : 
            this(0, secondsBetweenEvents, message, sendMessage, offsetInMinutes)
        {
            
        }

        public TimedMessage(int minutesBetweenEvents, int secondsBetweenEvents, string message, Action<string> sendMessage, int offsetInMinutes = 0)
        {
            // Sets up the timer
            this._timer = new Timer((minutesBetweenEvents * 60 + secondsBetweenEvents) * 1000)
            {
                AutoReset = true
            };
            this._timer.Elapsed += (sender, args) => sendMessage(message);


            if (offsetInMinutes > 0)
            {
                // Sets up a offset timer to start the base timer on a offset
                this._offsetTimer = new Timer(offsetInMinutes * 60 * 1000)
                {
                    AutoReset = false
                };
                this._offsetTimer.Elapsed += (sender, args) => this._timer.Start();
                this._offsetTimer.Start();
            }
            else
            {
                this._timer.Start();
            }
        }
    }
}
