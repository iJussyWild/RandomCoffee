using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RandomCoffee.Database;
using RandomCoffee.Database.Entities;

namespace RandomCoffee.Storers
{
	public class PersonStorer : Storer, IPersonStorer
	{
		public PersonStorer(AppDbContext dbContext) : base(dbContext)
		{ }

		public async Task<IDictionary<int, Person>> GetAllPersonsByIdsAsync()
			=>
				await DbContext.Persons.AsNoTracking()
					.Include(p => p.Meetings)
					.Include(p => p.Topics)
					.ToDictionaryAsync(p => p.Id);
	}
}
