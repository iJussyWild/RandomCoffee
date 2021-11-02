using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RandomCoffee.Database.Entities
{
	[Table("person")]
	public class Person
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[StringLength(50)]
		public string Name { get; set; }

		[StringLength(50)]
		public string LastName { get; set; }

		[StringLength(50)]
		public string Department { get; set; }

		public List<Topic> Topics { get; set; } = new();
		public List<PersonTopic> PersonTopics { get; set; } = new();

		public List<Meeting> Meetings { get; set; } = new();
		public List<PersonMeeting> PersonMeetings { get; set; } = new();
	}
}
