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
using OWLeagueBot.Extensions;
using static OWLeagueBot.Models.Enumerations;
using System.Linq;

namespace OWLeagueBot.Services
{
    public class CarouselBuilder : ICarouselBuilder
    {
        private readonly IOWLFilter _owlFilter;

        public CarouselBuilder(IOWLFilter owlFilter)
        {
            _owlFilter = owlFilter;
        }

        public async Task<Message> GetStandingsCarousel(RankingResponse standings)
        {
            var itemList = GetItemListFromRanking(standings);
            AddLastItem(itemList, Flow.Standings);

            var menu = new DocumentCollection()
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

        private List<DocumentSelect> GetItemListFromRanking(RankingResponse standings)
        {
            var itemList = new List<DocumentSelect>();
            var standingList = standings.Content.Take(5).ToList();

            foreach(Content r in standingList)
            {
                var item = new DocumentSelect
                {
                    Header = new DocumentContainer
                    {
                        Value = new MediaLink
                        {
                            AspectRatio = MyConstants.FacebookCarouselAspectRatio,
                            Uri = new Uri(r.Competitor.Logo),
                            Title = $"#{r.Placement}: {r.Competitor.Name}",
                            Text = $"W:{r.Records[0].MatchWin}|L:{r.Records[0].MatchLoss}"
                        }
                    },
                    Options = new DocumentSelectOption[]
                    {
                        new DocumentSelectOption
                        {
                            Label = new DocumentContainer{ Value = "🔔Add Alert" } ,
                            Value = new DocumentContainer{ Value = $"Add#Alert_{r.Competitor.AbbreviatedName}" }
                        }
                    }
                };
                itemList.Add(item);
            }
            return itemList;
        }

        public async Task<Message> GetNewsCarouselAsync(NewsResponse news, CancellationToken cancellationToken)
        {
            var itemList = GetItemListFromNews(news);
            AddLastItem(itemList, Flow.News);

            var menu = new DocumentCollection()
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

        private List<DocumentSelect> AddLastItem(List<DocumentSelect> list, Flow flow)
        {
            var item = new DocumentSelect();
            if(flow == Flow.News)
            {
                item = new DocumentSelect
                {
                    Header = new DocumentContainer
                    {
                        Value = new MediaLink
                        {
                            AspectRatio = MyConstants.FacebookCarouselAspectRatio,
                            Uri = new Uri("https://s3-sa-east-1.amazonaws.com/i.imgtake.takenet.com.br/igm0d/igm0d.jpg"),
                            Title = "News website",
                            Text = "Wanna see everything?"
                        }
                    },
                    Options = new DocumentSelectOption[]
                    {
                        new DocumentSelectOption
                        {
                            Label = new DocumentContainer
                            {
                                Value = new WebLink{ Uri = new Uri("https://overwatchleague.com/news"), Text = "All news" }
                            }
                        }
                    }
                };
            }
            else if(flow == Flow.Standings)
            {
                item = new DocumentSelect
                {
                    Header = new DocumentContainer
                    {
                        Value = new MediaLink
                        {
                            AspectRatio = MyConstants.FacebookCarouselAspectRatio,
                            Uri = new Uri("https://s3-sa-east-1.amazonaws.com/i.imgtake.takenet.com.br/imd25/imd25.jpg"),
                            Title = "Full Standings",
                            Text = "Wanna see everything?"
                        }
                    },
                    Options = new DocumentSelectOption[]
                    {
                        new DocumentSelectOption
                        {
                            Label = new DocumentContainer
                            {
                                Value = new WebLink{ Uri = new Uri("https://overwatchleague.com/standings"), Text = "Full Standings" }
                            }
                        }
                    }
                };
            }
            list.Add(item);
            return list;
        }

        private List<DocumentSelect> GetItemListFromNews(NewsResponse news)
        {
            var itemList = new List<DocumentSelect>();
            foreach(Blog b in news.Blogs)
            {
                var item = new DocumentSelect
                {
                    Header = new DocumentContainer
                    {
                        Value = new MediaLink
                        {
                            AspectRatio = MyConstants.FacebookCarouselAspectRatio,
                            Uri = new Uri("https:"+b.Thumbnail.Url),
                            Title = b.Title,
                            Text = b.Author
                        }
                    },
                    Options = new DocumentSelectOption[]
                    {
                        new DocumentSelectOption()
                        {
                            Label = new DocumentContainer
                            {
                                Value = new WebLink { Uri = new Uri(b.defaultUrl), Text = "Read" }
                            }
                        }
                    }
                };
                itemList.Add(item);
            }
            return itemList;
        }

        public async Task<Message> GetAlertTeamCarouselAsync(DivisionIds division, CancellationToken cancellationToken)
        {
            var divisionTeams = await _owlFilter.GetTeamsByDivisionAsync(division);
            var itemList = GetItemListFromTeams(divisionTeams, Flow.Alerts);
            var menu = new DocumentCollection()
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

        public async Task<Message> GetAgendaCarouselAsync(List<string> teams, CancellationToken cancellationToken)
        {
            var carousel = new DocumentCollection();
            var carouselItems = new List<DocumentSelect>();
            foreach(string s in teams)
            {
                var team = GetTeamIdFromTag(s);
                var match = await _owlFilter.GetNextMatchAsync(team);
                var item = new DocumentSelect
                {
                    Header = new DocumentContainer
                    {
                        Value = new MediaLink
                        {
                            Title = $"{match.StartDate.ConvertLongIntoDateTime().Date.DayOfWeek}",
                            Text = $"{match.Competitors[0].Name} vs {match.Competitors[1].Name}",
                            Uri = new Uri("https://s3-sa-east-1.amazonaws.com/i.imgtake.takenet.com.br/ixwke/ixwke.jpg"),
                            AspectRatio = MyConstants.FacebookCarouselAspectRatio
                        }
                    },
                Options = new DocumentSelectOption[]
                {
                    new DocumentSelectOption
                    {
                        Label = new DocumentContainer{ Value = new WebLink{ Uri = new Uri("https://overwatchleague.com/en-us/schedule"), Text = "🗓Full Agenda" }}
                    }
                }

            };
            carouselItems.Add(item);
          }
          
          if(carouselItems.Count >= 7) carouselItems = carouselItems.Take(7).ToList();

          carousel.Items = carouselItems.ToArray();
          carousel.Total = carouselItems.Count;
          carousel.ItemType = DocumentSelect.MediaType;
          return new Message() { Content = carousel };
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
                            Value = new DocumentContainer{ Value = "#Alert_Main" }
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
                            Value = new DocumentContainer{ Value = "#Agenda_" }
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
                            Label = new DocumentContainer{ Value = new WebLink{ Uri = new Uri("https://overwatchleague.com/en-us/schedule"), Text = "🗓Full Agenda" }}
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
                            Text = "Keep up today to everything thats happening on the league!",
                            Uri = new Uri("https://s3-sa-east-1.amazonaws.com/i.imgtake.takenet.com.br/iz14z/iz14z.jpg"),
                            AspectRatio = MyConstants.FacebookCarouselAspectRatio
                        }
                    },
                    Options = new DocumentSelectOption[]
                    {
                        new DocumentSelectOption
                        {
                            Label = new DocumentContainer{ Value = "📰News" } ,
                            Value = new DocumentContainer{ Value = "#News" }
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
                            Value = new DocumentContainer{ Value = "#Standings" }
                        }
                    }
                }
            };
        }

        private List<DocumentSelect> GetItemListFromTeams(CompetitorElement[] divisionTeams, Flow flowIdentity)
        {
            var tag = GetTagFromFlow(flowIdentity);
            var buttonsTexts = GetButtonTextFromFlow(flowIdentity).Split('|');
            var buttonText = buttonsTexts[0];
            var secondButtonText = "";
            if(buttonsTexts.Length == 2)
                secondButtonText = buttonsTexts[1];
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
                    }
                };
                var options = new List<DocumentSelectOption>();
                var firstOption = new DocumentSelectOption
                {
                    Label = new DocumentContainer{ Value = PlainText.Parse(buttonText) } ,
                    Value = new DocumentContainer{ Value = "Add" + tag + c.Competitor.AbbreviatedName }
                };
                options.Add(firstOption);
                if(!secondButtonText.IsNullOrWhiteSpace())
                {
                    var secondOption = new DocumentSelectOption
                    {
                        Label = new DocumentContainer{ Value = PlainText.Parse(secondButtonText) },
                        Value = new DocumentContainer{ Value = "Remove" + tag + c.Competitor.AbbreviatedName }
                    };
                    options.Add(secondOption);
                }
                item.Options = options.ToArray();
                itemList.Add(item);
            }
            return itemList;
        }

        // Colocar tags para remove e schedule
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
                    return "#Alert_";
                case Flow.Standings:
                    return "#Standings_";
                case Flow.MainMenu:
                default:
                    return "#MainMenu_";
            }
        }

        private string GetButtonTextFromFlow(Flow flowIdentity)
        {
            switch (flowIdentity)
            {
                case Flow.Alerts:
                    return "Schedule Alert|Remove Alert";
                case Flow.Onboarding:
                default:
                    return "This one!";

            }
        }
    }
}
