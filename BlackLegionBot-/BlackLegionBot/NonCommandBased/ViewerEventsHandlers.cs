using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client.Events;
using TwitchLib.PubSub.Events;

namespace BlackLegionBot.NonCommandBased
{
    public class ViewerEventsHandlers
    {
        private readonly Action<string> _sendMessage;
        public ViewerEventsHandlers(Action<string> sendMessage)
        {
            this._sendMessage = sendMessage;
        }

        public void HandleFollowEvent(object sender, OnFollowArgs followArgs)
        {
            _sendMessage($"{followArgs.DisplayName} bedankt voor de follow en geniet van de streams!");
        }

        public void HandleSubEvent(object sender, OnChannelSubscriptionArgs subArgs)
        {
            _sendMessage($"{subArgs.Subscription.Username} bedankt voor de sub!");
        }
    }
}
