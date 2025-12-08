using Microsoft.EntityFrameworkCore;
using BetTime.Models;

namespace BetTime.Data
{
    public class BetTimeContext : DbContext
    {
        public BetTimeContext(DbContextOptions<BetTimeContext> options)
            : base(options)
        {
        }

        
        public DbSet<User> Users { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Bet> Bets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Sport>().HasKey(s => s.Id);
            modelBuilder.Entity<League>().HasKey(l => l.Id);
            modelBuilder.Entity<Team>().HasKey(t => t.Id);
            modelBuilder.Entity<Match>().HasKey(m => m.Id);
            modelBuilder.Entity<Transaction>().HasKey(t => t.Id);
            modelBuilder.Entity<Bet>().HasKey(b => b.Id);

            
            modelBuilder.Entity<User>()
                .HasMany(u => u.Transactions)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Bets)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Sport>()
                .HasMany(s => s.Leagues)
                .WithOne(l => l.Sport)
                .HasForeignKey(l => l.SportId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<League>()
                .HasMany(l => l.Teams)
                .WithOne(t => t.League)
                .HasForeignKey(t => t.LeagueId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<League>()
                .HasMany(l => l.Matches)
                .WithOne(m => m.League)
                .HasForeignKey(m => m.LeagueId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Team>()
                .HasMany(t => t.HomeMatches)
                .WithOne(m => m.HomeTeam)
                .HasForeignKey(m => m.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasMany(t => t.AwayMatches)
                .WithOne(m => m.AwayTeam)
                .HasForeignKey(m => m.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasMany(m => m.Bets)
                .WithOne(b => b.Match)
                .HasForeignKey(b => b.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder
                .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
                .EnableSensitiveDataLogging();
        }
    }
}