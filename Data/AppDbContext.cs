using Microsoft.EntityFrameworkCore;
namespace Test1.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}

	public DbSet<User> User { get; set; }
}