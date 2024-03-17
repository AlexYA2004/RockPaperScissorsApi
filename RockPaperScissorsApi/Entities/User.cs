using Microsoft.AspNetCore.Identity;

namespace RockPaperScissorsApi.Entities
{
    public class User : IdentityUser<Guid>
    {
        public decimal Balance { get; set; }

        public ICollection<MatchHistory> Matches { get; set; }

        public ICollection<GameTransaction> SentTransactions { get; set; }

        public ICollection<GameTransaction> ReceivedTransactions { get; set; }

        public User()
        {
            Id = Guid.NewGuid();

            Balance = 1000;
        }
    }
}
