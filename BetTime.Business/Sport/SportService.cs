using BetTime.Models;
using BetTime.Data;

namespace BetTime.Business;


public class SportService:ISportService
{

private readonly ISportRepository _repository;

public SportService(ISportRepository repository)
    {
      _repository= repository;  
    }

public Sport CreateSport(SportCreateDTO sportCreateDTO)
    {
   var sport= new Sport(sportCreateDTO.Name);
   _repository.AddSport(sport);
   return sport;     
    }

public IEnumerable<Sport> GetAllSports()
    {
   return _repository.GetAllSports();
   
    }

public Sport GetSportById(int sportId)
    {

var sport= _repository.GetSportById(sportId);
 if (sport == null)
        {
        throw new KeyNotFoundException($"Deporte con ID {sportId} no encontrado");
        }   
        return sport;   
    }

public void DeleteSport(int sportId)
    {
     var sport= _repository.GetSportById(sportId);
     if(sport== null)
        {
     throw new KeyNotFoundException($"Deporte con ID {sportId} no encontrado");
        }
     _repository.DeleteSport(sport);   
    }
public void UpdateSport(int id, SportUpdateDTO sportUpdateDTO)
    {
  var sport= _repository.GetSportById(id);
   if (sport == null)
            throw new KeyNotFoundException($"Sport with ID {id} not found");

        if (!string.IsNullOrEmpty(sportUpdateDTO.Name))
            sport.Name = sportUpdateDTO.Name;

        _repository.UpdateSport(sport);     
    }
}