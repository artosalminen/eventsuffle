using Eventsuffle.Core.Entities;
using Eventsuffle.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace Eventsuffle.Infrastructure.Data
{
    public class EventSuffleDbContext : DbContext
    {
        protected DatabaseOptions DbOptions { get; set; }

        public EventSuffleDbContext(DbContextOptions<EventSuffleDbContext> options, IOptions<DatabaseOptions> dbOptions) : base(options)
        {
            DbOptions = dbOptions.Value;
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<SuggestedDate> SuggestedDates { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Event>(ConfigureEvent);
            modelBuilder.Entity<Vote>(ConfigureVote);
            modelBuilder.Entity<SuggestedDate>(ConfigureSuggestedDate);
            modelBuilder.Entity<VoteSuggestedDate>(ConfigureVoteSuggestedDate);
        }

        protected void ConfigureEvent(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("EVENT").HasKey(e => e.Id);
            builder.HasIndex(e => new { e.Id }).IsUnique();
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Name).IsRequired().HasMaxLength(Event.NAME_MAX_LENGTH);            
        }

        protected void ConfigureVote(EntityTypeBuilder<Vote> builder)
        {
            builder.ToTable("VOTE").HasKey(e => e.Id);
            builder.HasIndex(e => new { e.Id }).IsUnique();
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.PersonName).IsRequired().HasMaxLength(Vote.PERSON_NAME_MAX_LENGTH);
            builder.HasOne(e => e.Event)
                .WithMany(e => e.Votes)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected void ConfigureSuggestedDate(EntityTypeBuilder<SuggestedDate> builder)
        {
            builder.ToTable("SUGGESTED_DATE").HasKey(e => e.Id);
            builder.HasIndex(e => new { e.Id }).IsUnique();
            builder.HasIndex(e => new { e.EventId, e.Date }).IsUnique();
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Date).IsRequired();
        }

        protected void ConfigureVoteSuggestedDate(EntityTypeBuilder<VoteSuggestedDate> builder)
        {
            builder.ToTable("VOTE_SUGGESTED_DATE").HasKey(e => e.Id);
            builder.HasIndex(e => new { e.Id }).IsUnique();
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.HasIndex(e => new { e.VoteId, e.SuggestedDateId }).IsUnique();
            builder.Property(e => e.VoteId).IsRequired();
            builder.Property(e => e.SuggestedDateId).IsRequired();
            builder.HasOne(e => e.Vote).WithMany(v => v.VoteSuggestedDates).HasForeignKey(e => e.VoteId);
            builder.HasOne(e => e.SuggestedDate)
                .WithMany(v => v.VoteSuggestedDates)
                .HasForeignKey(e => e.SuggestedDateId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
