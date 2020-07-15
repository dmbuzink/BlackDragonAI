using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackLegionBot.CommandStorage;

namespace BlackLegionBot.NonCommandBased
{
    public class TimedMessageManager
    {
        private readonly ICommandRetriever _commandRetriever;
        private readonly BlbApiHandler _apiClient;
        private IEnumerable<TimedMessageHandler> _timedMessageHandlers;
        private readonly Action<string> _sendMessage;

        public TimedMessageManager(ICommandRetriever commandRetriever, BlbApiHandler apiClient, Action<string> sendMessage)
        {
            this._commandRetriever = commandRetriever;
            this._apiClient = apiClient;
            this._sendMessage = sendMessage;
        }

        public async void Start()
        {
            var commands = await this._commandRetriever.GetCommands();
            var timedMessages = await this._apiClient.GetTimedMessages();
            var timedMessageHandlers = new List<TimedMessageHandler>();

            var timeBetweenMessages = 30;
            var totalTimeBetweenMessages = timeBetweenMessages * timedMessages.Count();
            for (var i = 0; i < timedMessages.Count(); i++)
            {
                var tm = timedMessages.ElementAt(i);

                if(commands.TryGetValue(tm.Command, out var command))
                {
                    timedMessageHandlers.Add(new TimedMessageHandler(totalTimeBetweenMessages, 0, 
                        command.Message, _sendMessage, timeBetweenMessages * i));
                }
            }

            this._timedMessageHandlers = timedMessageHandlers;
        }
    }
}
