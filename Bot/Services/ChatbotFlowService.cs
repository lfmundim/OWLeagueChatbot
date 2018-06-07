using System;
using System.Threading;
using System.Threading.Tasks;
using Lime.Messaging.Contents;
using Lime.Protocol;
using OWLeagueBot.Extensions;
using OWLeagueBot.Models;
using Take.Blip.Client;
using Take.Blip.Client.Extensions.Broadcast;
using static OWLeagueBot.Models.Enumerations;

namespace OWLeagueBot.Services
{
    public class ChatbotFlowService : IChatbotFlowService
    {
        private readonly ICarouselBuilder _carouselBuilder;
        private readonly IQuickReplyBuilder _quickReplyBuilder;
        private readonly IBroadcastExtension _broadcastExtension;
        private readonly IContextManager _contextManager;
        private readonly IOWLFilter _owlFilter;
        private readonly ISender _sender;

        public ChatbotFlowService(ICarouselBuilder carouselBuilder,
                                  IQuickReplyBuilder quickReplyBuilder,
                                  IBroadcastExtension broadcastExtension,
                                  IContextManager contextManager,
                                  IOWLFilter owlFilter,
                                  ISender sender)
        {
            _carouselBuilder = carouselBuilder;
            _quickReplyBuilder = quickReplyBuilder;
            _broadcastExtension = broadcastExtension;
            _contextManager = contextManager;
            _owlFilter = owlFilter;
            _sender = sender;
        }
        #region Agenda
        public async Task<bool> SendAgendaFlowAsync(UserContext context, Message message, CancellationToken cancellationToken)
        {
            try
            {
                var carousel = await _carouselBuilder.GetCarousel(context.AlertTeams, cancellationToken);
                carousel.To = message.From;
                var textMessage = new Message()
                {
                    Content = PlainText.Parse("Here's your overview for this week:"),
                    To = message.From
                };
                await _sender.SendMessageAsync(textMessage, cancellationToken);
                await _sender.SendMessageAsync(carousel, cancellationToken);
                await _sender.SendDelayedComposingAsync(message.From, 2000, cancellationToken);
                await _sender.SendBackQuickReplyAsync(message.From, _quickReplyBuilder, cancellationToken);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        #endregion Agenda

        #region Alert
        public async Task<bool> SendAlertFlowAsync(UserContext context, Message message, CancellationToken cancellationToken)
        {
            try
            {
                var text = message.Content.ToString();

                if (text.Contains("Main"))
                {
                    await MainAlertFlow(message, cancellationToken);
                }
                else if (text.Contains("79") || text.Contains("80"))
                {
                    await DivisionSelection(message, text, cancellationToken);
                }
                else if (text.Contains("Add")) //schedule
                {
                    await AddAlertFlow(message, context, text, cancellationToken);
                }
                else if (text.Contains("Remove")) //remove schedule
                {
                    await RemoveAlertFlow(message, context, text, cancellationToken);
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private async Task RemoveAlertFlow(Message message, UserContext userContext, string text, CancellationToken cancellationToken)
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

        private async Task AddAlertFlow(Message message, UserContext userContext, string text, CancellationToken cancellationToken)
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

        private async Task DivisionSelection(Message message, string text, CancellationToken cancellationToken)
        {
            var divisionText = text.Split('_')[1];
            var divisionId = Enum.Parse<DivisionIds>(divisionText);
            var carousel = await _carouselBuilder.GetCarousel(divisionId, cancellationToken, Flow.Alerts);
            carousel.To = message.From;
            await _sender.SendMessageAsync("Which team’s alerts would you like to change?", message.From, cancellationToken);
            await _sender.SendMessageAsync(carousel, cancellationToken);
        }

        private async Task MainAlertFlow(Message message, CancellationToken cancellationToken)
        {
            var menu = _quickReplyBuilder.GetDivisionQuickReply(Flow.Alerts, cancellationToken);
            menu.To = message.From;
            await _sender.SendMessageAsync(menu, cancellationToken);
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
        #endregion Alert
    
        #region News
        public async Task<bool> SendNewsFlowAsync(Message message, CancellationToken cancellationToken)
        {
            try
            {
                var news = await _owlFilter.GetNewsAsync();
                var newsCarousel = await _carouselBuilder.GetCarousel(news, cancellationToken);
                newsCarousel.To = message.From;
                await _sender.SendMessageAsync("Here are the league's latest news:", message.From, cancellationToken);
                await _sender.SendMessageAsync(newsCarousel, cancellationToken);
                await _sender.SendDelayedComposingAsync(message.From, 2000, cancellationToken);
                await _sender.SendBackQuickReplyAsync(message.From, _quickReplyBuilder, cancellationToken);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        #endregion News  

        #region Onboarding
        public async Task<bool> SendOnboardingFlowAsync(UserContext userContext, Message message, CancellationToken cancellationToken)
        {
            try
            {
                var text = message.Content.ToString();
                if (userContext.FirstInteraction)
                {
                    await FirstInteractionFlow(message, userContext, cancellationToken);
                }
                else //if (!text.Equals("#Onboarding_0"))
                {
                    await OnboardingFlow(message, userContext, text, cancellationToken);
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private async Task OnboardingFlow(Message message, UserContext userContext, string text, CancellationToken cancellationToken)
        {
            userContext.TeamDivision = text.Split('_')[1];
            await _contextManager.SetUserContextAsync(message.From, userContext, cancellationToken);
            await _sender.SendMessageAsync("And which team is it?", message.From, cancellationToken);
            var division = GetDivisionFromText(userContext.TeamDivision);
            var carousel = await _carouselBuilder.GetCarousel(division, cancellationToken, Flow.Onboarding);
            carousel.To = message.From;
            await _sender.SendMessageAsync(carousel, cancellationToken);
        }

        private async Task FirstInteractionFlow(Message message, UserContext userContext, CancellationToken cancellationToken)
        {
            await _sender.SendMessageAsync("Hey there! I’m Emily! I’m here to help you keep track of what is going on in the Overwatch League!", message.From, cancellationToken);
            await _sender.SendDelayedComposingAsync(message.From, 2000, cancellationToken);
            var menu = _quickReplyBuilder.GetDivisionQuickReply(Flow.Onboarding, cancellationToken);
            menu.To = message.From;
            await _sender.SendMessageAsync(menu, cancellationToken);
            userContext.FirstInteraction = false;
            await _contextManager.SetUserContextAsync(message.From, userContext, cancellationToken);
        }
        #endregion Onboarding    

        #region SelectMainTeam
        public async Task<bool> SendSelectMainTeamFlowAsync(Message message, UserContext userContext, CancellationToken cancellationToken)
        {
            try
            {
                if(message.Content.ToString().Contains("SelectMainTeam_"))
                {
                    var teamTag = message.Content.ToString().Split('_')[1];
                    var teamListName = "Alert_" + teamTag;
                    await _broadcastExtension.UpdateDistributionListAsync(teamListName, message.From.ToIdentity(), cancellationToken);
                    userContext.MainTeam = teamTag;
                    userContext.AlertTeams.Add(teamTag);
                    await _sender.SendMessageAsync("Awesome! I added you to my list for that team and will notify you of any matches 30mins earlier!", message.From, cancellationToken);
                }
                await _sender.SendDelayedComposingAsync(message.From, 2000, cancellationToken);
                await _sender.SendMessageAsync("What can I help you with?", message.From, cancellationToken);
                var carousel = _carouselBuilder.GetMainMenuCarousel();
                carousel.To = message.From;
                await _sender.SendMessageAsync(carousel, cancellationToken);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        #endregion SelectMainTeam

        #region Standings 
        public async Task<bool> SendStandingsFlowAsync(Message message, CancellationToken cancellationToken)
        {
            try
            {
                await _sender.SendDelayedComposingAsync(message.From, 1000, cancellationToken);
                var standings = await _owlFilter.GetRankingAsync();
                var carousel = await _carouselBuilder.GetCarousel(standings);
                carousel.To = message.From;
                await _sender.SendMessageAsync("Here’s the current standings for the season:", message.From, cancellationToken);
                await _sender.SendMessageAsync(carousel, cancellationToken);
                await _sender.SendDelayedComposingAsync(message.From, 2000, cancellationToken);
                await _sender.SendBackQuickReplyAsync(message.From, _quickReplyBuilder, cancellationToken);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        #endregion Standings 
    }
}