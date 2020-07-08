using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BlackLegionBot.TwitchApi.Models
{
    public class BroadcasterSubscription
    {
        [JsonProperty("broadcaster_id")]
        public string BroadcasterId { get; set; }
        [JsonProperty("broadcaster_name")]
        public string BroadcasterName { get; set; }
        [JsonProperty("is_gift")]
        public bool IsGift { get; set; }
        public string Tier { get; set; }
        [JsonProperty("plan_name")]
        public string PlanName { get; set; }
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        [JsonProperty("user_name")]
        public string Username { get; set; }
    }
}
