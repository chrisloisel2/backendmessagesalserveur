using Microsoft.EntityFrameworkCore;

public class MyDbContext : DbContext
{

	public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
	{
		// Create the database if it doesn't exist
		Database.EnsureCreated();
		// Create the tables if they don't exist
		Database.Migrate();
		// Seed the database if empty
		if (!Messages.Any())
		{
			Messages.Add(new Message { Text = "Hello" });
			Messages.Add(new Message { Text = "World" });
			SaveChanges();
		}
	}
	public DbSet<Message> Messages { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer("Server=tcp:dbserveurtrello.database.windows.net,1433;Initial Catalog=trello;Persist Security Info=False;User ID=christopher;Password=Rose230323;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
	}
}
