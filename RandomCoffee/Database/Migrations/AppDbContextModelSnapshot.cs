﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RandomCoffee.Database;

namespace RandomCoffee.Database.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("RandomCoffee.Database.Entities.Meeting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.HasKey("Id");

                    b.ToTable("meeting");
                });

            modelBuilder.Entity("RandomCoffee.Database.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Department")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("person");
                });

            modelBuilder.Entity("RandomCoffee.Database.Entities.PersonMeeting", b =>
                {
                    b.Property<int>("PersonId")
                        .HasColumnType("integer");

                    b.Property<int>("MeetingId")
                        .HasColumnType("integer");

                    b.HasKey("PersonId", "MeetingId");

                    b.HasIndex("MeetingId");

                    b.ToTable("personmeeting");
                });

            modelBuilder.Entity("RandomCoffee.Database.Entities.PersonTopic", b =>
                {
                    b.Property<int>("PersonId")
                        .HasColumnType("integer");

                    b.Property<int>("TopicId")
                        .HasColumnType("integer");

                    b.HasKey("PersonId", "TopicId");

                    b.HasIndex("TopicId");

                    b.ToTable("persontopic");
                });

            modelBuilder.Entity("RandomCoffee.Database.Entities.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Value")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("topic");
                });

            modelBuilder.Entity("RandomCoffee.Database.Entities.PersonMeeting", b =>
                {
                    b.HasOne("RandomCoffee.Database.Entities.Meeting", "Meeting")
                        .WithMany("PersonMeetings")
                        .HasForeignKey("MeetingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RandomCoffee.Database.Entities.Person", "Person")
                        .WithMany("PersonMeetings")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Meeting");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("RandomCoffee.Database.Entities.PersonTopic", b =>
                {
                    b.HasOne("RandomCoffee.Database.Entities.Person", "Person")
                        .WithMany("PersonTopics")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RandomCoffee.Database.Entities.Topic", "Topic")
                        .WithMany("PersonTopics")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("RandomCoffee.Database.Entities.Meeting", b =>
                {
                    b.Navigation("PersonMeetings");
                });

            modelBuilder.Entity("RandomCoffee.Database.Entities.Person", b =>
                {
                    b.Navigation("PersonMeetings");

                    b.Navigation("PersonTopics");
                });

            modelBuilder.Entity("RandomCoffee.Database.Entities.Topic", b =>
                {
                    b.Navigation("PersonTopics");
                });
#pragma warning restore 612, 618
        }
    }
}