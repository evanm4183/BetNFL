using System;

namespace BetNFL.Models
{
    public class Bet
    {
        public int Id { get; set; }
        public int UserProfileId { get; set; }
        public int GameId { get; set; }
        public int BetTypeId { get; set; }
        public double? Line { get; set; }
        public int AwayTeamOdds { get; set; }
        public int HomeTeamOdds { get; set; }
        public DateTime CreateDateTime { get; set; }
        public bool isLive { get; set; }
        public BetType BetType { get; set; }
        public UserProfile UserProfile {get; set;}
    }
}
