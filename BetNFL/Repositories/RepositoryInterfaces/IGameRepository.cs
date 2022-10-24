using BetNFL.Models;
using System.Collections.Generic;

namespace BetNFL.Repositories
{
    public interface IGameRepository
    {
        List<Game> GetAllGamesInWeek(int week);
        void PostGame(Game game);
    }
}
