using TwitchLib.Client.Events;

namespace BlackLegionBot.CommandHandling
{
    public interface ICommandHandler
    {
        void Handle(OnMessageReceivedArgs messageReceivedArgs);
    }
}