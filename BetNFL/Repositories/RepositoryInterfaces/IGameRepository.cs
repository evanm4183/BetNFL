using BetNFL.Models;

namespace BetNFL.Repositories
{
    public interface IGameRepository
    {
        void PostGame(Game game);
    }
}
