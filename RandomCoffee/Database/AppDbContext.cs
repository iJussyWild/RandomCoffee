using Microsoft.EntityFrameworkCore;
using RandomCoffee.Database.Entities;

namespace RandomCoffee.Database
{
	public sealed class AppDbContext : DbContext
	{
		public DbSet<Person> Persons { get; set; }
		public DbSet<Topic> Topics { get; set; }
		public DbSet<Meeting> Meetings { get; set; }

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{ }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Person>().ToTable("person");
			modelBuilder.Entity<Topic>().ToTable("topic");
			modelBuilder.Entity<Meeting>().ToTable("meeting");
			modelBuilder.Entity<Person>()
				.HasMany(p => p.Topics)
				.WithMany(t => t.Persons)
				.UsingEntity<PersonTopic>(
					j => j
						.HasOne(pt => pt.Topic)
						.WithMany(t => t.PersonTopics)
						.HasForeignKey(pt => pt.TopicId),
					j => j
						.HasOne(pt => pt.Person)
						.WithMany(p => p.PersonTopics)
						.HasForeignKey(pt => pt.PersonId),
					j =>
					{
						j.HasKey(pt => new { pt.PersonId, pt.TopicId });
					});
			modelBuilder.Entity<Person>()
				.HasMany(p => p.Meetings)
				.WithMany(m => m.Persons)
				.UsingEntity<PersonMeeting>(
					j => j
						.HasOne(pm => pm.Meeting)
						.WithMany(m => m.PersonMeetings)
						.HasForeignKey(pm => pm.MeetingId),
					j => j
						.HasOne(pm => pm.Person)
						.WithMany(p => p.PersonMeetings)
						.HasForeignKey(pm => pm.PersonId),
					j =>
					{
						j.HasKey(pm => new { pm.PersonId, pm.MeetingId });
					});
		}
	}
}
