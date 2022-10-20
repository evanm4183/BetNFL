using System.Security.Claims;
using BetNFL.Models;
using BetNFL.Repositories;

namespace BetNFL.Utils
{
    public static class AuthUtils
    {
        public static UserProfile GetCurrentUserProfile(
            ClaimsPrincipal user, IUserProfileRepository userProfileRepo
            )
        {
            var firebaseUserId = user.FindFirst(ClaimTypes.NameIdentifier).Value;
            return userProfileRepo.GetByFirebaseUserId(firebaseUserId);
        }

        public static bool IsCurrentUserAdmin(
            ClaimsPrincipal user, IUserProfileRepository userProfileRepo
            )
        {
            var currentUser = GetCurrentUserProfile(user, userProfileRepo);
            return currentUser.UserType.Name == "admin";
        }
    }
}
