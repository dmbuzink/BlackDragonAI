using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using BlackLegionBot.CommandHandling;
using BlackLegionBot.CommandStorage;
using TwitchLib.Client.Models;

namespace BlackLegionBot.NonCommandBased
{
    public class UrlChecker : IMessageValidator
    {
        private readonly Bot _bot;
        private readonly HashSet<string> _peopleWithPermissionToSendMessageWithUrl = new HashSet<string>();

        public UrlChecker(Bot bot)
        {
            this._bot = bot;
        }

        public bool Validate(ChatMessage chatMessage) =>
            !ContainsUrl(chatMessage.Message) || 
            EPermission.SUBS.HasEqualOrHigherPermission(chatMessage.GetPermissionOfSender()) || 
            _peopleWithPermissionToSendMessageWithUrl.Remove(chatMessage.Username.ToLower());

        public void HandleValidationError(ChatMessage chatMessage) =>
            this._bot.TimeoutUser(chatMessage.Username, 1, "Gebruik van URI's in het bericht is niet toegestaan van de specifieke gebruiker");

        private static bool ContainsUrl(string message) =>
            new Regex(".*(http(s)?:\\/\\/.)?(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{2,256}\\.[a-z]{2,6}\\b([-a-zA-Z0-9@:%_\\+.~#?&//=]*).*")
                .Match(message).Success;

        public bool AddPermission(string username) =>
            _peopleWithPermissionToSendMessageWithUrl.Add(username.ToLower());
    }
}
