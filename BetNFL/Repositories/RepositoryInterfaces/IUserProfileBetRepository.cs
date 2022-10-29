using System.Collections.Generic;
using BetNFL.Models;


namespace BetNFL.Repositories
{
    public interface IUserProfileBetRepository
    {
        List<UserProfileBet> GetMyOpenBets(int userId);
        void PostUserProfileBet(UserProfileBet upBet);
    }
}
