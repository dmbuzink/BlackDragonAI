using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackLegionBot.CommandStorage
{
    public interface ICommandRetriever
    {
        Task<Dictionary<string, CommandDetails>> GetCommands();
        
        Task<(bool isFound, CommandDetails command)> TryGetCommand(string name);

        void RetrieveCommands();
    }
}