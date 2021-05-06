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

        public static readonly IEnumerable<string> NamesOfAdmins = new string[]
        {
            "blackdragon", "gamtheus"
        };

        private readonly TwitchApiManager _twitchApi;
        private TwitchClient Client { get; }
        private CommandSelector CommandSelector { get; }
        private readonly BlbApiHandler _blbApi;
//        private readonly List<TimedMessage> _timedMessages = new List<TimedMessage>();
        private TimedMessageManager _timedMessageManager;
        private readonly WebhookHandler _webhookHandler;
        private readonly CommercialManager _commercialManager;
        private readonly LiveStatusManager _liveStatusManager;

        public Bot(BlbApiHandler blbApi, ICommandRetriever commandRetriever, TwitchApiManager twitchApi, UserInfo userInfo, 
            IRCCredentials ircCredentials, CooldownManager cooldownManager)
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

            this._liveStatusManager = new LiveStatusManager(_twitchApi);
            this._commercialManager = new CommercialManager(twitchApi, _liveStatusManager);
            CommandSelector = new CommandSelector(this, _twitchApi, commandRetriever, blbApi, cooldownManager, _commercialManager);

            // EventHandlers
            Client.OnMessageReceived += CommandSelector.HandleCommand;
            Client.OnJoinedChannel += (sender, args) => SendMessageToChannel("Joined channel");

            Client.Initialize(creds, this._userInfo.ChannelName);
            this._twitchApi.AuthManager.WhisperNeedsToBeSend += SendWhisperToChannel;
            _timedMessageManager = new TimedMessageManager(commandRetriever, blbApi, SendMessageToChannel);

            var viewerEventsHandlers = new ViewerEventsHandlers(SendMessageToChannel);
            var pubSubClient = new TwitchPubSub();
            pubSubClient.Connect();
            pubSubClient.OnFollow += viewerEventsHandlers.HandleFollowEvent;
            pubSubClient.OnChannelSubscription += viewerEventsHandlers.HandleSubEvent;

            _webhookHandler = new WebhookHandler(blbApi);
            _webhookHandler.CommandsChanged += () =>
            {
                Console.WriteLine("Retrieving commands because webhook");
                commandRetriever.RetrieveCommands();
            };
            _webhookHandler.TimedMessagesChanged += async () =>
            {
                Console.WriteLine("Retrieving timed messages because webhook");
                await _timedMessageManager.Start(this._liveStatusManager);
            };

            Client.OnDisconnected += (sender, args) =>
            {
                Console.WriteLine($"Twitch client has disconnected at {DateTime.Now}");
                Client.Reconnect();
            };
            Client.OnConnectionError += (sender, args) => { Console.WriteLine($"Connection error at {DateTime.Now}"); };
            Client.OnLeftChannel += (sender, args) =>
            {
                Console.WriteLine($"Client left channel at {DateTime.UtcNow}");
            };
        }

        public async Task Connect()
        {
            // blb api
            await _blbApi.Authenticate();

            // twitch irc
            Client.Connect();
//            await this._webhookHandler.Setup();
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

            this._webhookHandler.ListenForWebhooks();
            await _timedMessageManager.Start(this._liveStatusManager);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}