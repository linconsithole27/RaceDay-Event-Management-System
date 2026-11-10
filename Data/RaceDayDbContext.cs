using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RACEDAY.API.Models;

namespace RACEDAY.API.Data;

public partial class RaceDayDbContext : DbContext
{
    public RaceDayDbContext(DbContextOptions<RaceDayDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Enrolment> Enrolments { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventCategory> EventCategories { get; set; }

    public virtual DbSet<Organiser> Organisers { get; set; }

    public virtual DbSet<Participat> Participats { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Enrolment>(entity =>
        {
            entity.HasKey(e => e.EnrolmentId).HasName("PK__Enrolmen__52498F0BA7FE846A");

            entity.ToTable("Enrolment");

            entity.Property(e => e.EnrolmentId)
                .ValueGeneratedNever()
                .HasColumnName("Enrolment_ID");
            entity.Property(e => e.CategoryId).HasColumnName("Category_ID");
            entity.Property(e => e.EnrolmentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("Enrolment_Date");
            entity.Property(e => e.ParticipantId).HasColumnName("Participant_ID");

            entity.HasOne(d => d.Category).WithMany(p => p.Enrolments)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Enrolment__Categ__778AC167");

            entity.HasOne(d => d.Participant).WithMany(p => p.Enrolments)
                .HasForeignKey(d => d.ParticipantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Enrolment__Parti__76969D2E");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Event__FD6BEFE43A363EBD");

            entity.ToTable("Event");

            entity.Property(e => e.EventId)
                .ValueGeneratedNever()
                .HasColumnName("Event_ID");
            entity.Property(e => e.Descriptionn)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Distance).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.EventDate).HasColumnName("Event_Date");
            entity.Property(e => e.EventName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Event_Name");
            entity.Property(e => e.EventType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Event_Type");
            entity.Property(e => e.Locationn)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.OrganiserId).HasColumnName("Organiser_ID");

            entity.HasOne(d => d.Organiser).WithMany(p => p.Events)
                .HasForeignKey(d => d.OrganiserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Event_Organiser");
        });

        modelBuilder.Entity<EventCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Event_Ca__6DB38D4E50BBA34E");

            entity.ToTable("Event_Category");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedNever()
                .HasColumnName("Category_ID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Category_Name");
            entity.Property(e => e.EventId).HasColumnName("Event_ID");

            entity.HasOne(d => d.Event).WithMany(p => p.EventCategories)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Event_Cat__Event__72C60C4A");
        });

        modelBuilder.Entity<Organiser>(entity =>
        {
            entity.HasKey(e => e.OrganiserId).HasName("PK__Organise__43E450773829F587");

            entity.ToTable("Organiser");

            entity.Property(e => e.OrganiserId)
                .ValueGeneratedNever()
                .HasColumnName("Organiser_ID");
            entity.Property(e => e.OrganiserEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Organiser_Email");
            entity.Property(e => e.OrganiserName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Organiser_Name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Participat>(entity =>
        {
            entity.HasKey(e => e.ParticipantId).HasName("PK__Particip__E05F08254FE62E96");

            entity.ToTable("Participat");

            entity.HasIndex(e => e.ParticipantEmail, "UQ__Particip__898083A2ACFF1AFA").IsUnique();

            entity.Property(e => e.ParticipantId)
                .ValueGeneratedNever()
                .HasColumnName("Participant_ID");
            entity.Property(e => e.ParticipantEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Participant_Email");
            entity.Property(e => e.ParticipantName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Participant_Name");
            entity.Property(e => e.Password)
                .HasMaxLength(99)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.ResultId).HasName("PK__Result__5E08F543B2CEE132");

            entity.ToTable("Result");

            entity.Property(e => e.ResultId)
                .ValueGeneratedNever()
                .HasColumnName("Result_ID");
            entity.Property(e => e.EnrolmentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("Enrolment_Date");
            entity.Property(e => e.EnrolmentId).HasColumnName("Enrolment_ID");
            entity.Property(e => e.FinishTime).HasColumnName("Finish_Time");

            entity.HasOne(d => d.Enrolment).WithMany(p => p.Results)
                .HasForeignKey(d => d.EnrolmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Result__Enrolmen__7B5B524B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
