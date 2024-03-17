using Microsoft.EntityFrameworkCore;
using RockPaperScissorsApi.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RockPaperScissorsApi.Services;
using Microsoft.AspNetCore.Identity;

namespace RockPaperScissorsApi.DAL
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {

        //public override DbSet<User> Users { get; set; }

        public DbSet<MatchHistory> MatchHistories { get; set; }

        public DbSet<GameTransaction> GameTransactions { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Matches)
                .WithOne(m => m.Player1)
                .HasForeignKey(m => m.Player1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.SentTransactions)
                .WithOne(t => t.Sender)
                .HasForeignKey(t => t.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.ReceivedTransactions)
                .WithOne(t => t.Receiver)
                .HasForeignKey(t => t.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MatchHistory>()
                .HasMany(m => m.Transactions)
                .WithOne(t => t.Match)
                .HasForeignKey(t => t.MatchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserRole<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserToken<string>>().HasNoKey();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Config.ConnectionString);
        }


    }
}
