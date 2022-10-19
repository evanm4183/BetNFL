using BetNFL.Models;

namespace BetNFL.Repositories
{
    public interface IUserRepository
    {
        User GetByFirebaseUserId(string firebaseUserId);
    }
}
