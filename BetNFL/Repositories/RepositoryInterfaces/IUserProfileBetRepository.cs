using System.Collections.Generic;
using BetNFL.Models;


namespace BetNFL.Repositories
{
    public interface IUserProfileBetRepository
    {
        List<UserProfileBet> GetBettorOpenBets(int userId);
        List<UserProfileBet> GetSportsbookOpenBets(int userId);
        void PostUserProfileBet(UserProfileBet upBet);
        void SettleOpenBetsByGame(int gameId);
    }
}
