using Newtonsoft.Json;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackLegionBot.CommandStorage
{
    public class BlbApiHandler
    {
        private readonly IBlbApi _blbApi;
        private readonly BLBAPIConfig _config;
        private string _jwt;

        public BlbApiHandler(IBlbApi blbApi, BLBAPIConfig config)
        {
            this._blbApi = blbApi;
            this._config = config;

            config.AuthenticationChanged += OnAuthenticationTokenChanged;
        }

        public void OnAuthenticationTokenChanged(string jwt)
        {
            this._jwt = jwt;
        }

        // Endpoints
        public async Task<AuthResult> Authenticate()
        {
            var authResult = await _blbApi.Authenticate(this._config);
            this._config.SetJwt(authResult.Token);
            return authResult;
        }


        //        public Task<AuthResult> Authenticate(BLBAPIConfig config) =>
        //            _blbApi.Authenticate(config);

        public Task<IEnumerable<CommandDetails>> GetAllCommands() =>
            _blbApi.GetCommands(this._jwt);

        public Task<CommandDetails> CreateCommand(CommandDetails commandDetails) =>
            _blbApi.CreateCommand(this._jwt, commandDetails);

        public Task<CommandDetails> EditCommand(CommandDetails commandDetails) =>
            _blbApi.EditCommand(this._jwt, commandDetails);

        public Task DeleteCommand(string commandName) =>
            _blbApi.DeleteCommand(this._jwt, commandName);

        public Task AddAlias(string commandName, string alias) =>
            _blbApi.AddAlias(this._jwt, commandName, new AliasInput() { Alias = alias });

        public Task DeleteAlias(string alias) =>
            _blbApi.DeleteAlias(this._jwt, alias);

        public Task<TimedMessage> CreateTimedMessage(string command) =>
            _blbApi.CreateTimedMessage(this._jwt, new TimedMessage()
            {
                Command = command
            });

        public Task<IEnumerable<TimedMessage>> GetTimedMessages() =>
            _blbApi.GetTimedMessages(this._jwt);

        public Task DeleteTimedMessage(string command) =>
            _blbApi.DeleteTimedMessage(this._jwt, command);

        public Task SubscribeToWebhook() =>
            _blbApi.SubscribeToWebhook(this._jwt);

        public Task SubscribeToWebhookIdempotent() =>
            _blbApi.SubscribeToWebhookIdempotent(this._jwt);

        public Task<BLBCounter> IncrementDeathCount(string gameId) =>
            _blbApi.IncrementDeathCount(this._jwt, gameId);

        public Task<BLBCounter> DecrementDeathCount(string gameId) =>
            _blbApi.DecrementDeathCount(this._jwt, gameId);

        public Task<BLBCounter> UpdateDeathCount(BLBCounter blbCounter, string gameId) =>
            _blbApi.UpdateDeathCount(this._jwt, gameId, blbCounter);

        public Task<BLBCounter> GetDeathCount(string gameId) =>
            _blbApi.GetDeathCount(this._jwt, gameId);

        public Task<IEnumerable<BLBCounter>> GetAllDeathCounts() =>
            _blbApi.GetAllDeathCounts(this._jwt);

        public Task<IEnumerable<BLBCounter>> GetAllCounters() =>
            _blbApi.GetCounters(this._jwt);

        public Task<Existence> CounterExists(string counterName) =>
            _blbApi.CounterExists(this._jwt, counterName);
    }

    public static class BlbApiExtensions
    {
        public static ApiError ToBlbApiError(this ApiException apiException) =>
            JsonConvert.DeserializeObject<ApiError>(apiException.Content);
    }
}
