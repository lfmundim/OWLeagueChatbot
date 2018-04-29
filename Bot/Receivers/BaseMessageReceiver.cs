using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lime.Messaging.Contents;
using Lime.Messaging.Resources;
using Lime.Protocol;
using NLog;
using OWLeagueBot.Extensions;
using OWLeagueBot.Models;
using OWLeagueBot.Models.Responses;
using OWLeagueBot.Services;
using Take.Blip.Client;
using Take.Blip.Client.Extensions.Broadcast;
using Take.Blip.Client.Extensions.Bucket;
using Take.Blip.Client.Extensions.Contacts;
using Take.Blip.Client.Extensions.Scheduler;
using static OWLeagueBot.Models.Enumerations;

namespace OWLeagueBot.Receivers
{
    public abstract class BaseMessageReceiver : IMessageReceiver
    {
        private readonly IContactExtension _contactService;
        private readonly ISender _sender;
        private readonly ILogger _logger;
        private readonly IContextManager _contextManager;
        private readonly IBucketExtension _bucket;
        private readonly ISchedulerExtension _scheduler;
        private readonly IBroadcastExtension _broadcast;
        private readonly IOWLFilter _owlFilter;
        private readonly Settings _settings;

        public BaseMessageReceiver(IContextManager contextManager,
                                   IContactExtension contactService,
                                   ISender sender,
                                   ILogger logger,
                                   IBucketExtension bucket,
                                   ISchedulerExtension scheduler,
                                   IBroadcastExtension broadcast,
                                   IOWLFilter owlFilter,
                                   Settings settings)
        {
            _contactService = contactService;
            _sender = sender;
            _logger = logger;
            _contextManager = contextManager;
            _bucket = bucket;
            _scheduler = scheduler;
            _broadcast = broadcast;
            _owlFilter = owlFilter;
            _settings = settings;
        }
        public async Task ReceiveAsync(Message message, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var userContext = await _contextManager.GetUserContextAsync(message.From, cancellationToken);
                var contact = await _contactService.GetAsync(message.From, cancellationToken);

                if (message.Content.ToString().Trim().Equals("#DEVDELETEUSER"))
                {
                    var bucketKey = _contextManager.GetBucketKey(message.From);
                    await _bucket.DeleteAsync(bucketKey);
                }
                else if (message.Content.ToString().Trim().Equals("#DEVREFRESHMESSAGES"))
                {
                    await _scheduler.UpdateBroadcastMessagesAsync(_owlFilter, _settings);
                }
                else
                {
                    await SendDelayedComposing(message.From, 2000, cancellationToken);
                    await ReceiveMessageAsync(message, contact, userContext, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private bool IsTextCommand(string v)
        {
            return v.Contains("#");
        }

        protected abstract Task ReceiveMessageAsync(Message message, Contact contact, UserContext userContext, CancellationToken cancellationToken);
        protected async Task SendDelayedComposing(Node destination, int millisecondsDelay, CancellationToken cancellationToken)
        {
            await _sender.SendMessageAsync(
                new Message
                {
                    Id = null,
                    To = destination,
                    Content = new ChatState { State = ChatStateEvent.Composing }
                },
            cancellationToken);
            await Task.Delay(millisecondsDelay);
        }
    }
}
