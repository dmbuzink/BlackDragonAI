using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BlackLegionBot.TwitchApi.Models
{
    public class FollowerInfo
    {
        [JsonProperty("from_id")]
        public string FromId { get; set; }

        [JsonProperty("from_name")]
        public string FromName { get; set; }

        [JsonProperty("to_id")]
        public string ToId { get; set; }

        [JsonProperty("to_name")]
        public string ToName { get; set; }

        [JsonProperty("followed_at")]
        public DateTime FollowedAt { get; set; }
    }
}
