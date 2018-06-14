using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lime.Protocol;
using OWLeagueBot.Extensions;
using Take.Blip.Client;
using Take.Blip.Client.Extensions.Bucket;
using Take.Blip.Client.Extensions.Scheduler;

namespace OWLeagueBot.Services
{
    public class DevActionHandler : IDevActionHandler
    {
        private readonly IContextManager _contextManager;
        private readonly IBucketExtension _bucket;
        private readonly ISchedulerExtension _scheduler;
        private readonly IOWLFilter _owlFilter;
        private readonly Settings _settings;
        private readonly ISender _sender;

        public DevActionHandler(IContextManager contextManager,
                                IBucketExtension bucket,
                                ISchedulerExtension scheduler,
                                IOWLFilter owlFilter,
                                Settings settings,
                                ISender sender)
        {
            _contextManager = contextManager;
            _bucket = bucket;
            _scheduler = scheduler;
            _owlFilter = owlFilter;
            _settings = settings;
            _sender = sender;
        }
        public async Task<bool> HandleAsync(Message message, CancellationToken cancellationToken)
        {
            try
            {
                var action = message.Content.ToString().Trim();
                action = RemoveDevTag(action);

<<<<<<< HEAD
                if (action.Equals(""))
=======
                if (action.Equals("")) //DevAction to delete current user from the bot's context
>>>>>>> 46aebc919bf2b9113bed7f14f43cb01e4352d47e
                {
                    var bucketKey = _contextManager.GetBucketKey(message.From);
                    await _bucket.DeleteAsync(bucketKey);
                    await _sender.SendMessageAsync("DELETION DONE", message.From, cancellationToken);
                }
<<<<<<< HEAD
                else if (action.Equals(""))
                {
                    if (message.From.Name.Equals(""))
=======
                else if (action.Equals("")) //DevAction to refresh scheduled messages. TODO: automate
                {
                    if (message.From.Name.Equals("")) //To make sure no one exploits this, set the Main Dev identifier from facebook here
>>>>>>> 46aebc919bf2b9113bed7f14f43cb01e4352d47e
                    {
                        await _scheduler.UpdateBroadcastMessagesAsync(_owlFilter, _settings);
                        await _sender.SendMessageAsync("UPDATE DONE", message.From, cancellationToken);
                    }
                    else
                    {
                        await _sender.SendMessageAsync("NOT MAIN DEV. ABORTING", message.From, cancellationToken);
                        throw new Exception();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string RemoveDevTag(string action)
        {
            return action.Replace("#DEVACTION#", "");
        }
    }
}
