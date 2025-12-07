using BetTime.Models;

namespace BetTime.Business;

public interface ILeagueService
{
    League CreateLeague(LeagueCreateDTO leagueCreateDTO);
    IEnumerable<League> GetAllLeagues();
    League GetLeagueById(int leagueId);
    void UpdateLeague(int leagueId, LeagueUpdateDTO leagueUpdateDTO);
    void DeleteLeague(int leagueId);
    IEnumerable<League> GetLeaguesBySport(int sportId);
}