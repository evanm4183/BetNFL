﻿using BetNFL.Models;

namespace BetNFL.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByFirebaseUserId(string firebaseUserId);
        void AddFunds(UserProfile userProfile);
    }
}
