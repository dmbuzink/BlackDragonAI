using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BlackLegionBot.TwitchApi.Models
{
    public class ChannelInfo
    {
        [JsonProperty("broadcaster_id")]
        public string BroadcasterId { get; set; }
        [JsonProperty("broadcaster_name")]
        public string BroadcasterName { get; set; }
        [JsonProperty("broadcaster_language")]
        public string BroadCasterLanguage { get; set; }
        [JsonProperty("game_id")]
        public string GameId { get; set; }
        [JsonProperty("game_name")]
        public string GameName { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
    }
}
