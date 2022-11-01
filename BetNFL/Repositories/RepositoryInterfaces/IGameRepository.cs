using BetNFL.Models;
using System.Collections.Generic;

namespace BetNFL.Repositories
{
    public interface IGameRepository
    {
        List<Game> GetAllGamesInWeek(int week);
        List<Game> GetGamesWithOpenBets();
        Game GetGameById(int id);
        Game GetGameWithLiveBets(int id);
        void PostGame(Game game);
        void SetScore(Game game);
        void DeleteGame(int id);
    }
}
