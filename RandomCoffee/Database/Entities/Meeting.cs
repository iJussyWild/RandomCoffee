using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RandomCoffee.Database.Entities
{
	[Table("meeting")]
	public class Meeting
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public List<Person> Persons { get; set; } = new();
		public List<PersonMeeting> PersonMeetings { get; set; } = new();
	}
}
