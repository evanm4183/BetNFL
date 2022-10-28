using System;

namespace BetNFL.Models
{
    public class UserProfileBet
    {
        public int Id { get; set; }
        public int UserProfileId { get; set; }
        public int BetId { get; set; }
        public int WinnerId { get; set; }
        public int Side { get; set; }
        public decimal BetAmount { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime ProcessedDateTime { get; set; }
    }
}
