using System.Linq;
using System.Threading.Tasks;
using RandomCoffee.Database;
using RandomCoffee.Database.Entities;

namespace RandomCoffee.Storers
{
	public class MeetingStorer : Storer, IMeetingStorer
	{
		public MeetingStorer(AppDbContext dbContext) : base(dbContext)
		{ }

		public async Task<int> AddMeetingAsync(Meeting meeting)
		{
			var pms = meeting.Persons.Select(p =>
				new PersonMeeting()
				{
					MeetingId = meeting.Id,
					PersonId = p.Id
				});

			var newMeeting = new Meeting();
			newMeeting.PersonMeetings.AddRange(pms);
			DbContext.Meetings.Add(newMeeting);

			await DbContext.SaveChangesAsync();
			meeting.Id = meeting.Id;

			return meeting.Id;
		}
	}
}
