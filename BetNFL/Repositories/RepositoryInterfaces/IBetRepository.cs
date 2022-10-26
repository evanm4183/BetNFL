using BetNFL.Models;

namespace BetNFL.Repositories
{
    public interface IBetRepository
    {
        Bet GetLiveBetForGame(int userProfileId, int gameId);
        void PostBet(Bet bet);
    }
}
