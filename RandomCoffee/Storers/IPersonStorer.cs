using System.Collections.Generic;
using System.Threading.Tasks;
using RandomCoffee.Database.Entities;

namespace RandomCoffee.Storers
{
	public interface IPersonStorer
	{
		Task<IDictionary<int, Person>> GetAllPersonsByIdsAsync();
	}
}
