using System.ComponentModel.DataAnnotations.Schema;

namespace RandomCoffee.Database.Entities
{
	[Table("personmeeting")]
	public class PersonMeeting
	{
		public int PersonId { get; set; }

		public int MeetingId { get; set; }

		public Person Person { get; set; }

		public Meeting Meeting { get; set; }
	}
}
