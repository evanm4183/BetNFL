using System.Collections.Generic;
using BetNFL.Models;

namespace BetNFL.Repositories
{
    public interface IUserTypeRepository
    {
        public List<UserType> GetPublicUserTypes();
    }
}
