using TwitchLib.Client.Events;

namespace BlackLegionBot.CommandHandling
{
    public interface ICommandHandler
    {
        // TODO: Replace by a method returning a Task instead, so error handling is a thing
        void Handle(OnMessageReceivedArgs messageReceivedArgs);
    }
}