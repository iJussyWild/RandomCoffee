using System.Collections.Generic;
using System.Threading.Tasks;
using RandomCoffee.Database;
using RandomCoffee.Database.Entities;

namespace RandomCoffee.Storers
{
	public class MeetingStorer : Storer, IMeetingStorer
	{
		public MeetingStorer(AppDbContext dbContext) : base(dbContext)
		{ }

		public async Task<Meeting> AddMeetingAsync(IEnumerable<Person> persons)
		{
			var meeting = new Meeting();
			meeting.Persons.AddRange(persons);

			// DbContext.Meetings.Add(meeting);
			// await DbContext.SaveChangesAsync();

			return meeting;
		}
	}
}
