using BetTime.Data;
using BetTime.Models;

namespace BetTime.Business;

public class LeagueService : ILeagueService
{
    private readonly ILeagueRepository _repository;
    private readonly ISportRepository _sportRepository;

    public LeagueService(ILeagueRepository repository, ISportRepository sportRepository)
    {
        _repository = repository;
        _sportRepository = sportRepository;
    }


public League CreateLeague(LeagueCreateDTO leagueCreateDTO)
    {
        if (string.IsNullOrWhiteSpace(leagueCreateDTO.Name))
        throw new ArgumentException("League name is required");
        var sport= _sportRepository.GetSportById(leagueCreateDTO.SportId);
        if(sport == null)
        {
          throw new KeyNotFoundException($"Sport with ID {leagueCreateDTO.SportId} not found");   
        }
        var league= new League(leagueCreateDTO.Name, leagueCreateDTO.SportId);
        _repository.AddLeague(league);
        return league;
    }

public IEnumerable<League> GetAllLeagues()
    {
   return _repository.GetAllLeagues();
    }


public League GetLeagueById(int leagueId)
    {

var league= _repository.GetLeagueById(leagueId);
        if (league == null)
        {
         throw new KeyNotFoundException($"League with ID {leagueId} not found");  
        }
        return league;
    }

public void DeleteLeague(int leagueId)
    {
    
var league= _repository.GetLeagueById(leagueId);
        if (league == null)
        {
      throw new KeyNotFoundException($"League with ID {leagueId} not found");  
        }
_repository.DeleteLeague(league);
    }

 public void UpdateLeague(int leagueId, LeagueUpdateDTO leagueUpdateDTO)
    {
        var league = _repository.GetLeagueById(leagueId);
        if (league == null)
            throw new KeyNotFoundException($"League with ID {leagueId} not found");

        if (!string.IsNullOrEmpty(leagueUpdateDTO.Name))
            league.Name = leagueUpdateDTO.Name;

        if (leagueUpdateDTO.SportId.HasValue)
        {
            var sport = _sportRepository.GetSportById(leagueUpdateDTO.SportId.Value);
            if (sport == null)
                throw new KeyNotFoundException($"Sport with ID {leagueUpdateDTO.SportId.Value} not found");
            league.SportId = leagueUpdateDTO.SportId.Value;
        }

        _repository.UpdateLeague(league);

    }

public IEnumerable<League> GetLeaguesBySport(int sportId)
    {
     return _repository.GetLeaguesBySport(sportId);
      
    }

}