using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lime.Messaging.Resources;
using Lime.Protocol;
using NLog;
using OWLeagueBot.Extensions;
using OWLeagueBot.Models;
using OWLeagueBot.Services;
using Take.Blip.Client;
using Take.Blip.Client.Extensions.Broadcast;
using Take.Blip.Client.Extensions.Bucket;
using Take.Blip.Client.Extensions.Contacts;
using Take.Blip.Client.Extensions.Scheduler;
using static OWLeagueBot.Models.Enumerations;

namespace OWLeagueBot.Receivers
{
    public class AlertReceiver : BaseMessageReceiver
    {
        private readonly IContextManager _contextManager;
        private readonly IContactExtension _contactService;
        private readonly ISender _sender;
        private readonly ILogger _logger;
        private readonly IQuickReplyBuilder _quickReplyBuilder;
        private readonly IOWLFilter _owlFilter;
        private readonly ICarouselBuilder _carouselBuilder;
        private readonly IBroadcastExtension _broadcastExtension;

        public AlertReceiver(IContextManager contextManager,
                             IContactExtension contactService,
                             ISender sender,
                             ILogger logger,
                             IDevActionHandler devActionHandler,
                             IQuickReplyBuilder quickReplyBuilder,
                             IOWLFilter owlFilter,
                             ICarouselBuilder carouselBuilder,
                             IBroadcastExtension broadcastExtension) : base(contextManager, contactService, sender, logger, devActionHandler)
        {
            _contextManager = contextManager;
            _contactService = contactService;
            _sender = sender;
            _logger = logger;
            _quickReplyBuilder = quickReplyBuilder;
            _owlFilter = owlFilter;
            _carouselBuilder = carouselBuilder;
            _broadcastExtension = broadcastExtension;
        }

        protected override async Task ReceiveMessageAsync(Message message, Contact contact, UserContext userContext, CancellationToken cancellationToken)
        {
            var text = message.Content.ToString();

            if (text.Contains("Main"))
            {
                var menu = _quickReplyBuilder.GetDivisionQuickReply(Flow.Alerts, cancellationToken);
                menu.To = message.From;
                await _sender.SendMessageAsync(menu, cancellationToken);
            }
            else if (text.Contains("79") || text.Contains("80"))
            {
                var divisionText = text.Split('_')[1];
                var divisionId = Enum.Parse<DivisionIds>(divisionText);
                var carousel = await _carouselBuilder.GetAlertTeamCarouselAsync(divisionId, cancellationToken);
                carousel.To = message.From;
                await _sender.SendMessageAsync("Which team’s alerts would you like to change?", message.From, cancellationToken);
                await _sender.SendMessageAsync(carousel, cancellationToken);
            }
            else if (text.Contains("Add")) //schedule
            {
                var teamTag = text.Split('_')[1];
                var teamName = GetTeamNameFromTeamTag(teamTag);
                await _broadcastExtension.AddRecipientAsync($"Alert_{teamTag}", message.From.ToIdentity(), cancellationToken);
                if (!userContext.AlertTeams.Contains(teamTag))
                {
                    userContext.AlertTeams.Add(teamTag);
                    await _contextManager.SetUserContextAsync(message.From, userContext, cancellationToken);
                }

                var mainMenu = _carouselBuilder.GetMainMenuCarousel();
                mainMenu.To = message.From;

                await _sender.SendMessageAsync($"Done! You will be notified 30mins before {teamName}’s games!", message.From, cancellationToken);

                await _sender.SendDelayedComposingAsync(message.From, 2000, cancellationToken);
                await _sender.SendMessageAsync(mainMenu, cancellationToken);
            }
            else if (text.Contains("Remove")) //remove schedule
            {
                var teamTag = text.Split('_')[1];
                var teamName = GetTeamNameFromTeamTag(teamTag);
                try
                {
                    await _broadcastExtension.DeleteRecipientAsync($"Alert_{teamTag}", message.From.ToIdentity(), cancellationToken);
                    userContext.AlertTeams.Remove(teamTag);
                    await _contextManager.SetUserContextAsync(message.From, userContext, cancellationToken);
                    await _sender.SendMessageAsync($"Done! You won't be notified before {teamName}’s games anymore!", message.From, cancellationToken);
                }
                catch (Exception ex)
                {
                    await _sender.SendMessageAsync("You are not on that list...", message.From, cancellationToken);
                }
                finally
                {
                    var mainMenu = _carouselBuilder.GetMainMenuCarousel();
                    mainMenu.To = message.From;

                    await _sender.SendDelayedComposingAsync(message.From, 2000, cancellationToken);
                    await _sender.SendMessageAsync(mainMenu, cancellationToken);
                }

            }
        }

        private string GetTeamNameFromTeamTag(string teamTag)
        {
            switch (teamTag)
            {
                case "GLA":
                    return "LA Gladiators";
                case "BOS":
                    return "Boston Uprising";
                case "DAL":
                    return "Dallas Fuel";
                case "FLA":
                    return "Florida Mayhem";
                case "HOU":
                    return "Houston Outlaws";
                case "LON":
                    return "London Spitfire";
                case "VAL":
                    return "LA Valiant";
                case "NYE":
                    return "NY Excelsior";
                case "PHI":
                    return "Philadelphia Fusion";
                case "SFS":
                    return "SF Shock";
                case "SEO":
                    return "Seoul Dynasty";
                case "SHD":
                default:
                    return "Shanghai Dragons";
            }
        }
    }
}
