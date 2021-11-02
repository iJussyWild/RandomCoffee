using System.ComponentModel.DataAnnotations.Schema;

namespace RandomCoffee.Database.Entities
{
	[Table("persontopic")]
	public class PersonTopic
	{
		public int PersonId { get; set; }

		public int TopicId { get; set; }

		public Person Person { get; set; }

		public Topic Topic { get; set; }
	}
}
