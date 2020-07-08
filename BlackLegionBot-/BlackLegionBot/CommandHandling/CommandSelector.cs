using System.Collections.Generic;
using BlackLegionBot.CommandStorage;
using BlackLegionBot.NonCommandBased;
using BlackLegionBot.TwitchApi;
using TwitchLib.Api.Interfaces;
using TwitchLib.Client.Events;

namespace BlackLegionBot.CommandHandling
{
    public class CommandSelector
    {
        private Bot Bot { get; }
        private TwitchApiManager TwitchApi { get; }
        private readonly UrlChecker _urlChecker;
        private readonly CapsChecker _capsChecker;
        private readonly IMessageValidator[] _messageValidators;
        
        private GenericCommandHandler GenericCommandHandler { get; }
        private readonly AuthCommandHandler _authCommandHandler;
        private readonly CommandCreateHandler _commandCreateHandler;
        private readonly CommandEditHandler _commandEditHandler;
        private readonly CommandDeletionHandler _commandDeletionHandler;
        private readonly GameSetterHandler _gameSetterHandler;
        private readonly CommandAliasCreationHandler _commandAliasCreationHandler;
        private readonly CommandAliasDeletionHandler _commandAliasDeletionHandler;
        private readonly CommercialStarterHandler _commercialStarterHandler;
        private readonly PermissionHandler _permissionHandler;

        public CommandSelector(Bot bot, TwitchApiManager twitchApi, ICommandRetriever commandRetriever, 
            BlbApiHandler blbApiClient, CooldownManager cooldownManager, CommercialManager commercialManager)
        {
            this.Bot = bot;
            this.TwitchApi = twitchApi;
            this._urlChecker = new UrlChecker(bot);
            this._capsChecker = new CapsChecker(bot);
            _messageValidators = new IMessageValidator[]{_urlChecker, _capsChecker};

            this.GenericCommandHandler = new GenericCommandHandler(commandRetriever, Bot, TwitchApi, cooldownManager);
            this._authCommandHandler = new AuthCommandHandler(twitchApi);
            var crudManager = new CommandCrudManager(blbApiClient, Bot.SendMessageToChannel);
            crudManager.RefreshOfCommandsRequired += commandRetriever.RetrieveCommands;
            this._commandCreateHandler = new CommandCreateHandler(crudManager);
            this._commandEditHandler = new CommandEditHandler(crudManager);
            this._commandDeletionHandler = new CommandDeletionHandler(crudManager);
            this._commandAliasCreationHandler = new CommandAliasCreationHandler(crudManager);
            this._commandAliasDeletionHandler = new CommandAliasDeletionHandler(crudManager);
            this._gameSetterHandler = new GameSetterHandler(twitchApi, Bot.SendMessageToChannel);
            this._commercialStarterHandler = new CommercialStarterHandler(commercialManager, Bot.SendMessageToChannel);
            this._permissionHandler = new PermissionHandler(this._urlChecker, bot);
        }

        public void HandleCommand(object sender, OnMessageReceivedArgs messageReceivedArgs)
        {
            if (!EPermission.MODS.HasEqualOrHigherPermission(messageReceivedArgs.ChatMessage.GetPermissionOfSender()))
            {
                foreach (var messageValidator in this._messageValidators)
                {
                    if (!messageValidator.Validate(messageReceivedArgs.ChatMessage))
                    {
                        messageValidator.HandleValidationError(messageReceivedArgs.ChatMessage);
                        return;
                    }
                }
            }

            GetCommandHandler(messageReceivedArgs).Handle(messageReceivedArgs);
        }

        private ICommandHandler GetCommandHandler(OnMessageReceivedArgs onMessageReceivedArgs)
        {
            var senderIsAdmin = onMessageReceivedArgs.ChatMessage.IsAdmin();
            var senderIsModOrHigher = EPermission.MODS.HasEqualOrHigherPermission(onMessageReceivedArgs.ChatMessage.GetPermissionOfSender());
            var calledCommand = onMessageReceivedArgs.ChatMessage.GetCalledCommand();
            return calledCommand switch
            {
                "!auth" when senderIsAdmin => this._authCommandHandler,
                "!new" when senderIsModOrHigher => this._commandCreateHandler,
                "!edit" when senderIsModOrHigher => this._commandEditHandler,
                "!setalias" when senderIsModOrHigher => this._commandAliasCreationHandler,
                "!addalias" when senderIsModOrHigher => this._commandAliasCreationHandler,
                "!deletealias" when senderIsModOrHigher => this._commandAliasDeletionHandler,
                "!delete" when senderIsModOrHigher => this._commandDeletionHandler,
                "!setgame" when senderIsModOrHigher => this._gameSetterHandler,
                "!startcommercial" when senderIsModOrHigher => this._commercialStarterHandler,
                "!permit" when senderIsModOrHigher => this._permissionHandler,
                _ => (ICommandHandler) GenericCommandHandler
            };
        }
    }
}