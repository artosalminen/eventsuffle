﻿// <auto-generated />
using System;
using Eventsuffle.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Eventsuffle.Infrastructure.Migrations.MsSqlMigrations
{
    [DbContext(typeof(EventSuffleMicrosoftSqlDbContext))]
    [Migration("20200222182103_CreateTables")]
    partial class CreateTables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Eventsuffle.Core.Entities.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("EVENT");
                });

            modelBuilder.Entity("Eventsuffle.Core.Entities.SuggestedDate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("EventId", "Date")
                        .IsUnique();

                    b.ToTable("SUGGESTED_DATE");
                });

            modelBuilder.Entity("Eventsuffle.Core.Entities.Vote", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PersonName")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("VOTE");
                });

            modelBuilder.Entity("Eventsuffle.Core.Entities.VoteSuggestedDate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SuggestedDateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("VoteId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("SuggestedDateId");

                    b.HasIndex("VoteId", "SuggestedDateId")
                        .IsUnique();

                    b.ToTable("VOTE_SUGGESTED_DATE");
                });

            modelBuilder.Entity("Eventsuffle.Core.Entities.SuggestedDate", b =>
                {
                    b.HasOne("Eventsuffle.Core.Entities.Event", "Event")
                        .WithMany("SuggestedDates")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Eventsuffle.Core.Entities.Vote", b =>
                {
                    b.HasOne("Eventsuffle.Core.Entities.Event", "Event")
                        .WithMany("Votes")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Eventsuffle.Core.Entities.VoteSuggestedDate", b =>
                {
                    b.HasOne("Eventsuffle.Core.Entities.SuggestedDate", "SuggestedDate")
                        .WithMany("VoteSuggestedDates")
                        .HasForeignKey("SuggestedDateId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Eventsuffle.Core.Entities.Vote", "Vote")
                        .WithMany("VoteSuggestedDates")
                        .HasForeignKey("VoteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
