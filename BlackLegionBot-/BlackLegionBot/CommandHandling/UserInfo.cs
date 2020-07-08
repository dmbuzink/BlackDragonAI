using Microsoft.Extensions.Configuration;

namespace BlackLegionBot.CommandHandling
{
    public class UserInfo
    {
        public UserInfo()
        {

        }

        public UserInfo(IConfiguration config)
        {
            ClientId = config.GetValue<string>("ClientId");
            Secret = config.GetValue<string>("Secret");
            ChannelName = config.GetValue<string>("ChannelName");
            UserId = config.GetValue<string>("UserId");
        }

        public string ClientId { get; set; }
        public string Secret { get; set; }
        public string ChannelName { get; set; }
        public string UserId { get; set; }
    }
}
