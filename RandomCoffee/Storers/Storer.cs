using RandomCoffee.Database;

namespace RandomCoffee.Storers
{
	public abstract class Storer
	{
		protected readonly AppDbContext DbContext;

		protected Storer(AppDbContext dbContext)
		{
			DbContext = dbContext;
		}
	}
}
