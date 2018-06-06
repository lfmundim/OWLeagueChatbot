using System.Threading;
using System.Threading.Tasks;
using Lime.Messaging.Resources;
using Lime.Protocol;
using NLog;
using OWLeagueBot.Extensions;
using OWLeagueBot.Models;
using OWLeagueBot.Services;
using Take.Blip.Client;
using Take.Blip.Client.Extensions.Contacts;

namespace OWLeagueBot.Receivers
{
    public class StandingsReceiver : BaseMessageReceiver
    {
        private readonly ISender _sender;
        private readonly IOWLFilter _owlFilter;
        private readonly ICarouselBuilder _carouselBuilder;
        private readonly IQuickReplyBuilder _quickReplyBuilder;
        public StandingsReceiver(IContextManager contextManager,
                                IContactExtension contactService,
                                ISender sender,
                                ILogger logger,
                                IDevActionHandler devActionHandler,
                                IOWLFilter owlFilter,
                                ICarouselBuilder carouselBuilder,
                                IQuickReplyBuilder quickReplyBuilder) : base(contextManager, contactService, sender, logger, devActionHandler)
        {
            _sender = sender;
            _owlFilter = owlFilter;
            _carouselBuilder = carouselBuilder;
            _quickReplyBuilder = quickReplyBuilder;
        }

        protected override async Task ReceiveMessageAsync(Message message, Contact contact, UserContext userContext, CancellationToken cancellationToken)
        {
            await _sender.SendDelayedComposingAsync(message.From, 1000, cancellationToken);
            var standings = await _owlFilter.GetRankingAsync();
            var carousel = await _carouselBuilder.GetStandingsCarousel(standings);
            carousel.To = message.From;
            await _sender.SendMessageAsync("Here’s the current standings for the season:", message.From, cancellationToken);
            await _sender.SendMessageAsync(carousel, cancellationToken);
            await _sender.SendDelayedComposingAsync(message.From, 2000, cancellationToken);
            var back = _quickReplyBuilder.GetBackQuickReply();
            back.To = message.From;
            await _sender.SendMessageAsync(back, cancellationToken);
        }
    }
}