using System.Threading.Tasks;

namespace BlackLegionBot.CommandHandling.SpecialsOperatorsHandling
{
    public class UserOperator : ISpecialOperator
    {
        private const string Operator = "$user";
        
        public string GetOperatorName() => Operator;
        
        public async Task<string> InjectOperatorAsync(string message, string username, string originalCommand) =>
            message.Replace(Operator, username);
    }
}