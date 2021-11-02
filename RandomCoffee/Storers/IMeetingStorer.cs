using System.Threading.Tasks;
using RandomCoffee.Database.Entities;

namespace RandomCoffee.Storers
{
	public interface IMeetingStorer
	{
		Task<int> AddMeetingAsync(Meeting meeting);
	}
}
