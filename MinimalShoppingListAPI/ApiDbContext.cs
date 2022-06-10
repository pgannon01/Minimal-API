using Microsoft.EntityFrameworkCore;

namespace MinimalShoppingListAPI
{
    public class ApiDbContext : DbContext
    {
        public DbSet<Grocery> Groceries => Set<Grocery>(); // Creates a DbSet of type T, which is set to grocery

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }
    }
}
