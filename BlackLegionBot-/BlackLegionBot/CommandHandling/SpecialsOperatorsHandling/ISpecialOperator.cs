using System.Threading.Tasks;

namespace BlackLegionBot.CommandHandling.SpecialsOperatorsHandling
{
    public interface ISpecialOperator
    {
        string GetOperatorName();
        Task<string> InjectOperatorAsync(string message, string username, string originalMessage);
    }
}