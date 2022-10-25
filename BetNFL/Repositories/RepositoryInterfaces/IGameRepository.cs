using BetNFL.Models;
using System.Collections.Generic;

namespace BetNFL.Repositories
{
    public interface IGameRepository
    {
        List<Game> GetAllGamesInWeek(int week);
        Game GetGameById(int id);
        void PostGame(Game game);
        void SetScore(Game game);
    }
}
