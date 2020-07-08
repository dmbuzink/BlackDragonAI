using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BlackLegionBot.CommandHandling;
using BlackLegionBot.CommandStorage;
using BlackLegionBot.Credentials;
using BlackLegionBot.NonCommandBased;
using BlackLegionBot.TwitchApi;
using Microsoft.Extensions.Hosting;
using TwitchLib.Api;
using TwitchLib.Api.Core;
using TwitchLib.Api.Core.Enums;
using TwitchLib.Api.Interfaces;
using TwitchLib.Client;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Enums;
using TwitchLib.Communication.Models;
using TwitchLib.PubSub;

namespace BlackLegionBot
{
    public class Bot : IHostedService
    {
        private readonly UserInfo _userInfo;

        public static readonly IEnumerable<string> NamesOfAdmins = new List<string>()
        {
            "blackdragon", "dutchgamingmedia", "gamtheus"
        };

        private readonly TwitchApiManager _twitchApi;
        private TwitchClient Client { get; }
        private CommandSelector CommandSelector { get; }
        private readonly BlbApiHandler _blbApi;
        private List<TimedMessage> _timedMessages = new List<TimedMessage>();

        public Bot(BlbApiHandler blbApi, ICommandRetriever commandRetriever, TwitchApiManager twitchApi, UserInfo userInfo, 
            IRCCredentials ircCredentials, CooldownManager cooldownManager, CommercialManager commercialManager)
        {
            this._userInfo = userInfo;
            this._twitchApi = twitchApi;
            _blbApi = blbApi;

            var creds = new ConnectionCredentials(ircCredentials.Username, ircCredentials.AccessToken);
            var webSocketClient = new WebSocketClient(new ClientOptions()
            {
                ClientType = ClientType.Chat
            });
            Client = new TwitchClient(webSocketClient);


            CommandSelector = new CommandSelector(this, _twitchApi, commandRetriever, blbApi, cooldownManager, commercialManager);

            // EventHandlers
            Client.OnMessageReceived += CommandSelector.HandleCommand;
            Client.OnJoinedChannel += (sender, args) => SendMessageToChannel("Joined channel");

            Client.Initialize(creds, this._userInfo.ChannelName);
            this._twitchApi.AuthManager.WhisperNeedsToBeSend += SendWhisperToChannel;

            _timedMessages.Add(new TimedMessage(30, 0, 
                "Als je altijd op de hoogte wilt zijn van wat ik doe en wanneer ik live ga, volg me dan op Twitter @BlackDragonNL of klik hier: https://twitter.com/BlackDragonNL", 
                SendMessageToChannel));

            // special events
//            var pubSubWebSocketClient = new WebSocketClient(new ClientOptions()
//            {
//                ClientType = ClientType.PubSub
//            });
            var viewerEventsHandlers = new ViewerEventsHandlers(SendMessageToChannel);
            var pubSubClient = new TwitchPubSub();
            pubSubClient.Connect();
            pubSubClient.OnFollow += viewerEventsHandlers.HandleFollowEvent;
            pubSubClient.OnChannelSubscription += viewerEventsHandlers.HandleSubEvent;
        }

        public async Task Connect()
        {
            // blb api
            await _blbApi.Authenticate();

            // twitch irc
            Client.Connect();
        }

        public void SendMessageToChannel(string message)
        {
            if (!message.TrimStart()[0].Equals('/'))
                message = $"/me {message}";
            this.Client.SendMessage(this._userInfo.ChannelName, message);
        }

        public void SendWhisperToChannel(string message) =>
            SendWhisperToChannel(message, this._userInfo.ChannelName);
        
        public void SendWhisperToChannel(string message, string whisperTo) =>
            SendMessageToChannel($"/w {whisperTo} {message}");

        public void TimeoutUser(string user, int duration, string message = "")
        {
            Client.TimeoutUser(this._userInfo.ChannelName, user, new TimeSpan(0, duration / 60, duration % 60), message);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Connect();

            await this._twitchApi.Initialize();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}