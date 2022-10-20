using System.Collections.Generic;
using BetNFL.Models;

namespace BetNFL.Repositories
{
    public interface ITeamRepository
    {
        List<Team> GetAll();
    }
}
