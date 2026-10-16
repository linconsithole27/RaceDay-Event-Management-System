
using Microsoft.EntityFrameworkCore;
using RaceDay.Models;

namespace RaceDay.Data;

public class RaceDayDbContext : DbContext
{
    public RaceDayDbContext(DbContextOptions<RaceDayDbContext> options)
        : base(options)
    {
    }

    public DbSet<Organiser> Organisers => Set<Organiser>();
    public DbSet<Participant> Participants => Set<Participant>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<EventCategory> EventCategories => Set<EventCategory>();
    public DbSet<Enrolment> Enrolments => Set<Enrolment>();
    public DbSet<Result> Results => Set<Result>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Organiser>().ToTable("Organiser");
        modelBuilder.Entity<Participant>().ToTable("Participant");
        modelBuilder.Entity<Event>().ToTable("Event");
        modelBuilder.Entity<EventCategory>().ToTable("Event_Category");
        modelBuilder.Entity<Enrolment>().ToTable("Enrolment");
        modelBuilder.Entity<Result>().ToTable("Result");

        modelBuilder.Entity<Organiser>().HasKey(x => x.Organiser_ID);
        modelBuilder.Entity<Participant>().HasKey(x => x.Participant_ID);
        modelBuilder.Entity<Event>().HasKey(x => x.Event_ID);
        modelBuilder.Entity<EventCategory>().HasKey(x => x.Category_ID);
        modelBuilder.Entity<Enrolment>().HasKey(x => x.Enrolment_ID);
        modelBuilder.Entity<Result>().HasKey(x => x.Result_ID);
    }
}
