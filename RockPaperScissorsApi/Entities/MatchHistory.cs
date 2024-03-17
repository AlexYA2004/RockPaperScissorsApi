namespace RockPaperScissorsApi.Entities
{
    public class MatchHistory
    {
        public Guid Id { get; set; }

        public Guid Player1Id { get; set; }

        public Guid Player2Id { get; set; }

        public decimal BetAmount { get; set; }

        public GameResult Result { get; set; }

        public User Player1 { get; set; }

        public User Player2 { get; set; }

        public ICollection<GameTransaction> Transactions { get; set; }

        public MatchHistory()
        {
            Id = Guid.NewGuid();
        }
    }
}
