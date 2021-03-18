using BlackLegionBot.Credentials;
using Microsoft.Extensions.Configuration;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackLegionBot.CommandStorage
{
    public interface IBlbApi
    {
        [Post("/users/login")]
        Task<AuthResult> Authenticate([Body] BLBAPIConfig config);

        [Get("/commands")]
        Task<IEnumerable<CommandDetails>> GetCommands([Header("X-Access-Token")] string authToken);

        [Post("/commands")]
        Task<CommandDetails> CreateCommand([Header("X-Access-Token")] string authToken, [Body] CommandDetails commandDetails);

        [Put("/commands")]
        Task<CommandDetails> EditCommand([Header("X-Access-Token")] string authToken, [Body] CommandDetails commandDetails);

        [Delete("/commands/{commandName}")]
        Task DeleteCommand([Header("X-Access-Token")] string authToken, string commandName);

        [Post("/commands/alias/{commandName}")]
        Task AddAlias([Header("X-Access-Token")] string authToken, string commandName, [Body] AliasInput aliasInput);

        [Delete("/commands/alias/{alias}")]
        Task DeleteAlias([Header("X-Access-Token")] string authToken, string alias);

        [Post("/timedmessages")]
        Task<TimedMessage> CreateTimedMessage([Header("X-Access-Token")] string authToken, [Body] TimedMessage timedMessage);

        [Get("/timedmessages")]
        Task<IEnumerable<TimedMessage>> GetTimedMessages([Header("X-Access-Token")] string authToken);

        [Delete("/timedmessages/{commandName}")]
        Task DeleteTimedMessage([Header("X-Access-Token")] string authToken, string commandName);

        [Post("/webhook")]
        Task<WebhookSubscriber> SubscribeToWebhook([Header("X-Access-Token")] string authToken);

        [Post("/webhook/idempotent")]
        Task<WebhookSubscriber> SubscribeToWebhookIdempotent([Header("X-Access-Token")] string authToken);

        [Post("/deaths/{gameId}")]
        Task<BLBCounter> IncrementDeathCount([Header("X-Access-Token")] string authToken, string gameId);

        [Put("/deaths/{gameId}")]
        Task<BLBCounter> UpdateDeathCount([Header("X-Access-Token")] string authToken, string gameId, [Body] BLBCounter blbCounter);

        [Get("/deaths/counters")]
        Task<IEnumerable<BLBCounter>> GetCounters([Header("X-Access-Token")] string authToken);

        [Get("/deaths/counters/{counterName}")]
        Task<BLBCounter> GetCounter([Header("X-Access-Token")] string authToken, string counterName);

        [Delete("/deaths/{gameId}")]
        Task<BLBCounter> DecrementDeathCount([Header("X-Access-Token")] string authToken, string gameId);

        [Get("/deaths/{gameId}")]
        Task<BLBCounter?> GetDeathCount([Header("X-Access-Token")] string authToken, string gameId);

        [Get("/deaths")]
        Task<IEnumerable<BLBCounter>> GetAllDeathCounts([Header("X-Access-Token")] string authToken);

        [Get("/deaths/exists/{counterName}")]
        Task<Existence> CounterExists([Header("X-Access-Token")] string authToken, string counterName);
    }

    public class AliasInput
    {
        public string Alias { get; set; }
    }

    public class BLBAPIConfig
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
        public string JWT { get; private set; }

        public event Action<string> AuthenticationChanged;

        public BLBAPIConfig() { }

        public BLBAPIConfig(IConfigurationSection config)
        {
            this.Username = config.GetValue<string>("username");
            this.Password = config.GetValue<string>("password");
            this.Url = config.GetValue<string>("url");
        }

        public void SetJwt(string jwt)
        {
            this.JWT = jwt;
            AuthenticationChanged?.Invoke(this.JWT);
            //            await UpdateAppsettings();
        }

        public async Task UpdateAppsettings()
        {
            var appsettings = await Appsettings.GetAppsettings();
            appsettings.Blbapi = this;
            await appsettings.WriteAppsettings();
        }
    }

    public class AuthResult
    {
        public string Token { get; set; }
        public int AuthorizationLevel { get; set; }
    }

    public class ApiError
    {
        public string Message { get; set; }
        public int Code { get; set; }
        public string DateTime { get; set; }
    }
}