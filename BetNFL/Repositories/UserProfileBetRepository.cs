using BetNFL.Models;
using Microsoft.Extensions.Configuration;

namespace BetNFL.Repositories
{
    public class UserProfileBetRepository : BaseRepository, IUserProfileBetRepository
    {
        public UserProfileBetRepository(IConfiguration configuration) : base(configuration) { }

        public void PostUserProfileBet(UserProfileBet upBet)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO UserProfileBet
                        (
                            UserProfileId, BetId, WinnerId, Side,
                            BetAmount, CreateDateTime, ProcessedDateTime
                        )
                        VALUES
                        (
                            @userProfileId, @betId, NULL, @side, 
                            @betAmount, GETDATE(), NULL
                        )
                    ";
                    cmd.Parameters.AddWithValue("@userProfileId", upBet.UserProfileId);
                    cmd.Parameters.AddWithValue("@betId", upBet.BetId);
                    cmd.Parameters.AddWithValue("@side", upBet.Side);
                    cmd.Parameters.AddWithValue("@betAmount", upBet.BetAmount);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
