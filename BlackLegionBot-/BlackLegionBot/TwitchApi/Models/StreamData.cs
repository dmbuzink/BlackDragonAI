using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BlackLegionBot.TwitchApi.Models
{
    public class StreamData
    {
        public string Id { get; set; }
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        [JsonProperty("user_name")]
        public string Username { get; set; }
        [JsonProperty("game_id")]
        public string GameId { get; set; }

        public string Type { get; set; }
        public string Title { get; set; }
        [JsonProperty("viewer_count")]
        public int ViewerCount { get; set; }
        [JsonProperty("started_at")]
        public DateTime StartedAt { get; set; }
        public string Language { get; set; }
        [JsonProperty("thumbnail_url")]
        public string ThumbnailUrl { get; set; }
    }
}
