using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RandomCoffee.Database.Entities
{
	[Table("topic")]
	public class Topic
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[StringLength(50)]
		public string Value { get; set; }

		public List<Person> Persons { get; set; } = new();
		public List<PersonTopic> PersonTopics { get; set; } = new();
	}
}
