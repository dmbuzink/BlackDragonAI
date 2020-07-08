using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace BlackLegionBot.CommandHandling
{
    public class CooldownManager
    {
        private readonly Dictionary<string, Timer> _cooldownTimers;
        private const int CooldownTime = 60;

        public CooldownManager()
        {
            _cooldownTimers = new Dictionary<string, Timer>();
        }

        public void StartCooldown(string commandName)
        {
            var cooldownTimer = new Timer(CooldownTime * 1000)
            {
                AutoReset = false
            };
            cooldownTimer.Elapsed += (sender, args) => RemoveCooldownTimer(commandName);
            cooldownTimer.Start();
            RemoveCooldownTimer(commandName);
            _cooldownTimers.Add(commandName, cooldownTimer);
        }

        private void RemoveCooldownTimer(string commandName) =>
            _cooldownTimers.Remove(commandName);

        public bool IsInCooldown(string commandName) => 
            _cooldownTimers.ContainsKey(commandName);
    }
}
