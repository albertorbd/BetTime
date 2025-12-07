using BetTime.Models;

namespace BetTime.Business;

public interface ITeamService
{
    Team CreateTeam(TeamCreateDTO teamCreateDTO);
    IEnumerable<Team> GetAllTeams();
    Team GetTeamById(int teamId);
    void UpdateTeam(int teamId, TeamUpdateDTO teamUpdateDTO);
    void DeleteTeam(int teamId);
    IEnumerable<Team> GetTeamsByLeague(int leagueId);
}