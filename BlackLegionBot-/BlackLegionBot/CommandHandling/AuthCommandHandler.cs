using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlackLegionBot.TwitchApi;
using TwitchLib.Client.Events;

namespace BlackLegionBot.CommandHandling
{
    class AuthCommandHandler : ICommandHandler
    {
        private TwitchApiManager _apiClient;
        private Task ReauthTask;

        public AuthCommandHandler(TwitchApiManager apiClient)
        {
            this._apiClient = apiClient;
        }

        public void Handle(OnMessageReceivedArgs messageReceivedArgs)
        {
            if (ReauthTask == null)
            {
                ReauthTask = this._apiClient.AuthManager.Reauthorize();
            }
        }
    }
}
