using Microsoft.Extensions.Configuration;

namespace BlackLegionBot.Credentials
{
    public class IRCCredentials
    {
        public IRCCredentials()
        {

        }

        public IRCCredentials(IConfiguration configSection)
        {
            Username = configSection.GetValue<string>("Username");
            AccessToken = configSection.GetValue<string>("AccessToken");
        }

        public string Username { get; set; }
    
        public string AccessToken { get; set; }
    }
}
