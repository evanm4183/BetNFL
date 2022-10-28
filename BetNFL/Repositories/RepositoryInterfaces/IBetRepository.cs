using BetNFL.Models;

namespace BetNFL.Repositories
{
    public interface IBetRepository
    {
        Bet GetLiveBetForGame(int userProfileId, int gameId);
        Bet GetBetById(int id);
        void PostBet(Bet bet);
        void CloseBet(Bet bet);
    }
}
