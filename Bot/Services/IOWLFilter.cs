using OWLeagueBot.Models;
using OWLeagueBot.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static OWLeagueBot.Models.Enumerations;

namespace OWLeagueBot.Services
{
    public interface IOWLFilter
    {
        Task<ScheduleResponse> GetLastMatchupAsync(TeamIds mainTeamId, TeamIds secondTeamId);
        Task<ScheduleResponse> GetNextMatchAsync(TeamIds teamId);
        Task<RankingResponse> GetRankingAsync();
        Task<NewsResponse> GetNewsAsync();
        Task<CompetitorElement[]> GetTeamsByDivisionAsync(DivisionIds divisionId);
        Task<ScheduleResponse[]> GetFutureMatchesAsync(TeamIds teamId);
    }
}
