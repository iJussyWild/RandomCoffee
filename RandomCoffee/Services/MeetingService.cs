using System.Linq;
using System.Threading.Tasks;
using RandomCoffee.Database.Entities;
using RandomCoffee.Storers;

namespace RandomCoffee.Services
{
	public class MeetingService
	{
		private const int PersonsCount = 4;

		private readonly IPersonStorer _personStorer;
		private readonly IMeetingStorer _meetingStorer;
		private readonly PersonPointsResolver _resolver;

		public MeetingService(
			IPersonStorer personStorer,
			IMeetingStorer meetingStorer,
			PersonPointsResolver resolver)
		{
			_personStorer = personStorer;
			_meetingStorer = meetingStorer;
			_resolver = resolver;
		}

		public async Task<Meeting> CreateMeetingAsync(bool isNeedSave)
		{
			var persons = await _personStorer.GetAllPersonsByIdsAsync();
			if (persons == null || persons.Count == 0)
				throw new LogicException("Persons are not exist");

			var meeting = new Meeting();
			var person = persons.Values.OrderBy(p => p.Meetings.Count).First();
			persons.Remove(person.Id);
			meeting.Persons.Add(person);

			var otherPersons = PersonsCount - 1;
			for (var i = 0; i < otherPersons; i++)
			{
				person = _resolver.GetPersonForMeetingByMaxPoints(persons.Values, person);
				persons.Remove(person.Id);
				meeting.Persons.Add(person);
			}

			if (isNeedSave)
				await _meetingStorer.AddMeetingAsync(meeting);

			return meeting;
		}
	}
}
