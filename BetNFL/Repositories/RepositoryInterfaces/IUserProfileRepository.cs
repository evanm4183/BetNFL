using BetNFL.Models;

namespace BetNFL.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByFirebaseUserId(string firebaseUserId);
        void RegisterNewUser(UserProfile userProfile);
        void AddFunds(UserProfile userProfile);
    }
}
