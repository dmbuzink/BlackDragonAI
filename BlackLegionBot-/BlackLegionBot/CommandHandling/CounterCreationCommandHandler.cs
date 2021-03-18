using BlackLegionBot.CommandStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TwitchLib.Client.Events;

namespace BlackLegionBot.CommandHandling
{
    public class CounterCreationCommandHandler : ICommandHandler
    {
        private readonly string _commandCall = "!newcounter ";
        private readonly BlbApiHandler _blbApi;
        private readonly Bot _bot;
        public event Action<string> OnCounterCreated;

        public CounterCreationCommandHandler(BlbApiHandler blbApi, Bot bot)
        {
            this._blbApi = blbApi;
            this._bot = bot;
        }

        public async void Handle(OnMessageReceivedArgs messageReceivedArgs)
        {
            if (this._commandCall.Length >= messageReceivedArgs.ChatMessage.Message.Length) return;
            var commandName = messageReceivedArgs.ChatMessage.Message.
                Substring(this._commandCall.Length)
                .TrimEnd()
                .Replace("!", "");

            // Call blbApi
            var counter = new BLBCounter()
            {
                Deaths = 0,
                GameId = commandName,
                IsDeathCount = false
            };
            await this._blbApi.UpdateDeathCount(counter, commandName);
            this.OnCounterCreated?.Invoke(commandName);
        }
    }

    public class CounterRetrievalCommandHandler : ICommandHandler
    {
        private readonly BlbApiHandler _blbApi;
        private readonly Bot _bot;
        private HashSet<string> counters;

        public CounterRetrievalCommandHandler(BlbApiHandler handler, Bot bot)
        {
            this._blbApi = handler;
            this._bot = bot;
            SetupCounters();
        }

        private async void SetupCounters()
        {
            var counters = await this._blbApi.GetAllCounters();
            this.counters = counters.Select(c => c.GameId).ToHashSet();
        }

        public void AddCounter(string counterName)
        {
            this.counters.Add(counterName);
        }

        public async void Handle(OnMessageReceivedArgs messageReceivedArgs)
        {
            var message = messageReceivedArgs.ChatMessage.Message;
            if (!message.Contains("!"))
                return;

            var counterName = new Regex("![a-zA-Z]+").Matches(message)[0].Value;
            if (!this.counters.Contains(counterName))
                return;

            int count;
            if (message.Contains("+"))
            {
                // Increment
                var counter = await this._blbApi.IncrementDeathCount(counterName);
                count = counter.Deaths;
            }
            else if (message.Contains("-"))
            {
                // Decrement
                var counter = await this._blbApi.DecrementDeathCount(counterName);
                count = counter.Deaths;
            }
            else
            {
                // Retrieve
                var counter = await this._blbApi.GetDeathCount(counterName);
                count = counter.Deaths;
            }

            this._bot.SendMessageToChannel($"De {counterName} staat op {count}!");
        }
    }
}
