using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client.Models;

namespace BlackLegionBot.NonCommandBased
{
    public interface IMessageValidator
    {
        bool Validate(ChatMessage chatMessage);

        void HandleValidationError(ChatMessage chatMessage);
    }
}
