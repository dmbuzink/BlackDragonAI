﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TwitchLib.Client.Models;

namespace BlackLegionBot.NonCommandBased
{
    public class CapsChecker : IMessageValidator
    {
        private const float CapsPercentageToAllow = 0.5f;
        private const int MinimumAmountOfCharacters = 15;

        private readonly Bot _bot;

        public CapsChecker(Bot bot)
        {
            this._bot = bot;
        }

        private static bool UsesTooManyCaps(string message)
        {
            message = CommonRegexes.EmoteRegex.Replace(message, "");
            message = CommonRegexes.UsernameWithAtSymbol.Replace(message, "");
            var messageWithoutWhiteSpace = message.Replace(" ", "");

            if (messageWithoutWhiteSpace.Length < MinimumAmountOfCharacters)
                return false;

            float capsAmount = new Regex("[A-Z]").Matches(messageWithoutWhiteSpace).Count;
            var capsPercentage = capsAmount / messageWithoutWhiteSpace.Length;
            var usesTooManyCaps = capsPercentage > CapsPercentageToAllow;
            return usesTooManyCaps;
        }

        public bool Validate(ChatMessage chatMessage) => !UsesTooManyCaps(chatMessage.Message);
        public void HandleValidationError(ChatMessage chatMessage)
        {
            this._bot.TimeoutUser(chatMessage.Username, 1, "Onnodig gebruik van caps is niet toegestaan");
        }
    }
}
