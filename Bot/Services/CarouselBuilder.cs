using Lime.Messaging.Contents;
using Lime.Protocol;
using OWLeagueBot.Models;
using OWLeagueBot.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Take.Blip.Client;
using static OWLeagueBot.Models.Enumerations;

namespace OWLeagueBot.Services
{
    public class CarouselBuilder : ICarouselBuilder
    {
        private readonly IOWLFilter _owlFilter;

        public CarouselBuilder(IOWLFilter owlFilter)
        {
            _owlFilter = owlFilter;
        }
        public async Task<Message> GetOnboardingTeamCarouselAsync(DivisionIds division, CancellationToken cancellationToken)
        {
            var divisionTeams = await _owlFilter.GetTeamsByDivisionAsync(division);
            var itemList = GetItemListFromTeams(divisionTeams, Flow.Onboarding);
            var menu = new DocumentCollection
            {
                Items = itemList.ToArray(),
                Total = itemList.Count,
                ItemType = DocumentSelect.MediaType
            };
            var menuMessage = new Message()
            {
                Content = menu
            };
            return menuMessage;
        }


        public Message GetMainMenuCarousel()
        {
            var carousel = new DocumentCollection();
            var items = GetMainMenuItems();
            carousel.Items = items;
            carousel.Total = items.Length;
            carousel.ItemType = DocumentSelect.MediaType;
            return new Message(){ Content = carousel };
        }

        private DocumentSelect[] GetMainMenuItems()
        {
            return new DocumentSelect[]
            {
                new DocumentSelect
                {
                    Header = new DocumentContainer
                    {
                        Value = new MediaLink
                        {
                            Title = "Manage Alerts",
                            Text = "I can remind you when your favourite teams are playing!",
                            Uri = new Uri("https://s3-sa-east-1.amazonaws.com/i.imgtake.takenet.com.br/i7m1n/i7m1n.jpg"),
                            AspectRatio = MyConstants.FacebookCarouselAspectRatio
                        }
                    },
                    Options = new DocumentSelectOption[]
                    {
                        new DocumentSelectOption
                        {
                            Label = new DocumentContainer{ Value = "🔔Alerts" } ,
                            Value = new DocumentContainer{ Value = "" }
                        }
                    }
                },
                new DocumentSelect
                {
                    Header = new DocumentContainer
                    {
                        Value = new MediaLink
                        {
                            Title = "My Agenda",
                            Text = "Let me show you your week!",
                            Uri = new Uri("https://s3-sa-east-1.amazonaws.com/i.imgtake.takenet.com.br/im838/im838.jpg"),
                            AspectRatio = MyConstants.FacebookCarouselAspectRatio
                        }
                    },
                    Options = new DocumentSelectOption[]
                    {
                        new DocumentSelectOption
                        {
                            Label = new DocumentContainer{ Value = "📅Agenda" } ,
                            Value = new DocumentContainer{ Value = "" }
                        }
                    }
                },
                new DocumentSelect
                {
                    Header = new DocumentContainer
                    {
                        Value = new MediaLink
                        {
                            Title = "Full Agenda",
                            Text = "If you want a bigger picture, I can show you everything!",
                            Uri = new Uri("https://s3-sa-east-1.amazonaws.com/i.imgtake.takenet.com.br/ipock/ipock.jpg"),
                            AspectRatio = MyConstants.FacebookCarouselAspectRatio
                        }
                    },
                    Options = new DocumentSelectOption[]
                    {
                        new DocumentSelectOption
                        {
                            Label = new DocumentContainer{ Value = "🗓Full Agenda" } ,
                            Value = new DocumentContainer{ Value = new MediaLink{ Uri = new Uri("https://overwatchleague.com/en-us/schedule")}}
                        }
                    }
                },
                new DocumentSelect
                {
                    Header = new DocumentContainer
                    {
                        Value = new MediaLink
                        {
                            Title = "News",
                            Text = "Kepp up today to everything thats happening on the league!",
                            Uri = new Uri("https://s3-sa-east-1.amazonaws.com/i.imgtake.takenet.com.br/iz14z/iz14z.jpg"),
                            AspectRatio = MyConstants.FacebookCarouselAspectRatio
                        }
                    },
                    Options = new DocumentSelectOption[]
                    {
                        new DocumentSelectOption
                        {
                            Label = new DocumentContainer{ Value = "📰News" } ,
                            Value = new DocumentContainer{ Value = "" }
                        }
                    }
                },
                new DocumentSelect
                {
                    Header = new DocumentContainer
                    {
                        Value = new MediaLink
                        {
                            Title = "Standings",
                            Text = "Want a quick overview of the standings?",
                            Uri = new Uri("https://s3-sa-east-1.amazonaws.com/i.imgtake.takenet.com.br/ivtbr/ivtbr.jpg"),
                            AspectRatio = MyConstants.FacebookCarouselAspectRatio
                        }
                    },
                    Options = new DocumentSelectOption[]
                    {
                        new DocumentSelectOption
                        {
                            Label = new DocumentContainer{ Value = "🏆Standings" } ,
                            Value = new DocumentContainer{ Value = "" }
                        }
                    }
                }
            };
        }

        private List<DocumentSelect> GetItemListFromTeams(CompetitorElement[] divisionTeams, Flow flowIdentity)
        {
            var tag = GetTagFromFlow(flowIdentity);
            var buttonText = GetSingleButtonTextFromFlow(flowIdentity);
            var itemList = new List<DocumentSelect>();

            foreach (CompetitorElement c in divisionTeams)
            {
                var item = new DocumentSelect
                {
                    Header = new DocumentContainer
                    {
                        Value = new MediaLink
                        {
                            Title = c.Competitor.Name,
                            Text = c.Competitor.HomeLocation,
                            Uri = new Uri(c.Competitor.Logo),
                            AspectRatio = MyConstants.FacebookCarouselAspectRatio
                        }
                    },
                    Options = new DocumentSelectOption[]
                    {
                        new DocumentSelectOption
                        {
                            Label = new DocumentContainer{ Value = PlainText.Parse(buttonText) } ,
                            Value = new DocumentContainer{ Value = tag + c.Competitor.AbbreviatedName }
                        }
                    }
                };
                itemList.Add(item);
            }
            return itemList;
        }

        private string GetTagFromFlow(Flow flowIdentity)
        {
            switch (flowIdentity)
            {
                case Flow.Onboarding:
                    return "#SelectMainTeam_";
                case Flow.MySchedule:
                    return "#MySchedule_";
                case Flow.News:
                    return "#News_";
                case Flow.Alerts:
                    return "#Alerts_";
                case Flow.Standings:
                    return "#Standings_";
                case Flow.MainMenu:
                default:
                    return "#MainMenu_";
            }
        }

        private string GetSingleButtonTextFromFlow(Flow flowIdentity)
        {
            switch (flowIdentity)
            {
                case Flow.Onboarding:
                default:
                    return "This one!";
            }
        }
    }
}
