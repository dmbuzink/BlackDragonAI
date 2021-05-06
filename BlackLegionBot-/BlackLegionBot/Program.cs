using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using BlackLegionBot.CommandHandling;
using BlackLegionBot.CommandStorage;
using BlackLegionBot.Credentials;
using BlackLegionBot.NonCommandBased;
using BlackLegionBot.TwitchApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Enums;
using TwitchLib.Communication.Models;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;

namespace BlackLegionBot
{
    public class Program
    {
        static void Main(string[] args) =>

            CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    var config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();

                    // configs
                    var blbApiConfig = new BLBAPIConfig(config.GetSection("Blbapi"));
                    services.AddSingleton<BLBAPIConfig>(blbApiConfig);
                    var ircCredentials = new IRCCredentials(config.GetSection("Irc"));
                    services.AddSingleton(ircCredentials);
                    var userInfo = new UserInfo(config.GetSection("UserConfig"));
                    services.AddSingleton(userInfo);

                    var contentSerializer = new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    };
                    var refitSettings = new RefitSettings()
                    {
                        ContentSerializer = new JsonContentSerializer(contentSerializer)
                    };

                    services.AddRefitClient<ITwitchApiManager>(refitSettings).ConfigureHttpClient(httpClient =>
                        httpClient.BaseAddress = new Uri("https://api.twitch.tv"));

                    services.AddRefitClient<ITwitchAuthApi>(refitSettings)
                        .ConfigureHttpClient(httpClient => 
                            httpClient.BaseAddress = new Uri("https://id.twitch.tv/oauth2"));

                    services.AddRefitClient<IBlbApi>(refitSettings).ConfigureHttpClient(httpClient =>
                    {
                        httpClient.BaseAddress = new Uri(blbApiConfig.Url);
                        httpClient.DefaultRequestHeaders.Add("X-Access-Token", blbApiConfig.JWT);
                    });

                    services.AddSingleton<CommercialManager>();
                    services.AddSingleton<CooldownManager>();
                    services.AddSingleton<TwitchAuthManager>();
                    services.AddSingleton<TwitchApiManager>();
                    services.AddSingleton<BlbApiHandler>();
                    services.AddSingleton<ICommandRetriever, CommandCache>();
                    services.AddHostedService<Bot>();
                });
    }
}
