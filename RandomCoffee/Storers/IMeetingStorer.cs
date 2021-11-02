using System.Collections.Generic;
using System.Threading.Tasks;
using RandomCoffee.Database.Entities;

namespace RandomCoffee.Storers
{
	public interface IMeetingStorer
	{
		Task<Meeting> AddMeetingAsync(IEnumerable<Person> persons);
	}
}
