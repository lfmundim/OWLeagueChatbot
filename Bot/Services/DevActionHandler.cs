using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lime.Protocol;
using OWLeagueBot.Extensions;
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

        public DevActionHandler(IContextManager contextManager,
                                IBucketExtension bucket,
                                ISchedulerExtension scheduler,
                                IOWLFilter owlFilter,
                                Settings settings)
        {
            _contextManager = contextManager;
            _bucket = bucket;
            _scheduler = scheduler;
            _owlFilter = owlFilter;
            _settings = settings;
        }
        public async Task<bool> HandleAsync(Message message, CancellationToken cancellationToken)
        {
            try
            {
                var action = message.Content.ToString().Trim();
                action = RemoveDevTag(action);

                if (message.Content.ToString().Trim().Equals("DEVDELETEUSER"))
                {
                    var bucketKey = _contextManager.GetBucketKey(message.From);
                    await _bucket.DeleteAsync(bucketKey);
                }
                else if (message.Content.ToString().Trim().Equals("DEVREFRESHMESSAGES"))
                {
                    if (message.From.Name.Equals("1700444659974923"))
                        await _scheduler.UpdateBroadcastMessagesAsync(_owlFilter, _settings);
                    else
                        throw new Exception();
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
