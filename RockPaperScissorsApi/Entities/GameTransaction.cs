namespace RockPaperScissorsApi.Entities
{
    public class GameTransaction
    {
        public Guid Id { get; set; }

        public Guid SenderId { get; set; }

        public Guid ReceiverId { get; set; }

        public Guid MatchId { get; set; }

        public decimal Amount { get; set; }

        public User Sender { get; set; }

        public User Receiver { get; set; }

        public MatchHistory Match { get; set; }

        public GameTransaction()
        {
            Id = Guid.NewGuid();
        }
    }
}
