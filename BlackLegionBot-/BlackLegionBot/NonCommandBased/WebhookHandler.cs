using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BlackLegionBot.CommandStorage;

namespace BlackLegionBot.NonCommandBased
{
    public class WebhookHandler
    {
        public event Action CommandsChanged;
        public event Action TimedMessagesChanged;

        private readonly BlbApiHandler _apiClient;
        private readonly List<(string webhookPath, Action eventToRaise)> _webhooks = new List<(string webhookPath, Action eventToRaise)>();

        public WebhookHandler(BlbApiHandler apiClient)
        {
            this._apiClient = apiClient;
            _webhooks.Add(("commands", RaiseCommandChangedEvent));
            _webhooks.Add(("timedmessages", RaiseTimedMessagesChangedEvent));
            ListenForWebhooks();
        }

        public async Task Setup()
        {
            await this._apiClient.SubscribeToWebhookIdempotent();
        }

        public HttpListener SetListener()
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://*:2005/");
            listener.Start();
            Console.WriteLine("Listening");
            return listener;
        }

        public async void ListenForWebhooks()
        {
            var listener = SetListener();

            while (true)
            {
                var request = await listener.GetContextAsync();
                var path = request.Request.RawUrl;
                foreach (var webhook in _webhooks.Where(webhook => path.Contains(webhook.webhookPath)))
                {
                    webhook.eventToRaise();
                    break;
                }
            }
        }

        private void RaiseCommandChangedEvent() => CommandsChanged?.Invoke();
        private void RaiseTimedMessagesChangedEvent() => TimedMessagesChanged?.Invoke();
    }
}
