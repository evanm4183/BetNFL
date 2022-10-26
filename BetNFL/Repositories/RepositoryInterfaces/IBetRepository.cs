using BetNFL.Models;

namespace BetNFL.Repositories
{
    public interface IBetRepository
    {
        //Bet GetLiveBetForGame(int userId, int gameId);
        void PostBet(Bet bet);
    }
}
