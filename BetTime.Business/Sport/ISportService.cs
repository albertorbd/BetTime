using BetTime.Models;

namespace BetTime.Business;

public interface ISportService
{
    
Sport CreateSport(SportCreateDTO sportCreateDTO);

IEnumerable<Sport> GetAllSports();
Sport GetSportById(int sportId);
void DeleteSport(int sportId);
void UpdateSport(int id, SportUpdateDTO sportUpdateDTO);
}