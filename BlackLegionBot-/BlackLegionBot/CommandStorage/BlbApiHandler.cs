using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Refit;

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
            _blbApi.AddAlias(this._jwt, commandName, new AliasInput() {Alias = alias});

        public Task DeleteAlias(string alias) =>
            _blbApi.DeleteAlias(this._jwt, alias);
    }

    public static class BlbApiExtensions
    {
        public static ApiError ToBlbApiError(this ApiException apiException) =>
            JsonConvert.DeserializeObject<ApiError>(apiException.Content);
    }
}
