using BetTime.Data;
using BetTime.Models;

namespace BetTime.Business;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _repository;
    private readonly ILeagueRepository _leagueRepository;

    public TeamService(ITeamRepository repository, ILeagueRepository leagueRepository)
    {
        _repository = repository;
        _leagueRepository = leagueRepository;
    }

public Team CreateTeam(TeamCreateDTO teamCreateDTO)
    {

    if (string.IsNullOrWhiteSpace(teamCreateDTO.Name))
            throw new ArgumentException("Team name is required");

        
        var league = _leagueRepository.GetLeagueById(teamCreateDTO.LeagueId);
        if (league == null)
        throw new KeyNotFoundException($"League with ID {teamCreateDTO.LeagueId} not found");
        var team= new Team(teamCreateDTO.Name, teamCreateDTO.LeagueId);

        _repository.AddTeam(team);
        return team;

    }
public IEnumerable<Team> GetAllTeams()
    {

    return _repository.GetAllTeams();
    }

public IEnumerable<Team> GetTeamsByLeague(int leagueId)
    {
return _repository.GetTeamsByLeague(leagueId);
    }


public Team GetTeamById(int teamId)
    {
 var team= _repository.GetTeamById(teamId);
        if (team == null)
        {
            throw new KeyNotFoundException($"Team with ID {teamId} not found");
        } 
    return team;      
    }
public void DeleteTeam(int teamId)
    {
        var team = _repository.GetTeamById(teamId);
        if (team == null)
            throw new KeyNotFoundException($"Team with ID {teamId} not found");

        _repository.DeleteTeam(team);
    
}

public void UpdateTeam(int id, TeamUpdateDTO teamUpdateDTO)
    {

var team= _repository.GetTeamById(id);
        if (team == null)
        {
          throw new KeyNotFoundException($"Team with ID {id} not found");  
        }
    if (!string.IsNullOrWhiteSpace(teamUpdateDTO.Name))
    team.Name= teamUpdateDTO.Name;
    if (teamUpdateDTO.LeagueId.HasValue)
        {
          
        var league = _leagueRepository.GetLeagueById(teamUpdateDTO.LeagueId.Value);
        if (league == null)
        throw new KeyNotFoundException($"League with ID {teamUpdateDTO.LeagueId.Value} not found");

        team.LeagueId = teamUpdateDTO.LeagueId.Value;
        }

        _repository.UpdateTeam(team);
    }

}