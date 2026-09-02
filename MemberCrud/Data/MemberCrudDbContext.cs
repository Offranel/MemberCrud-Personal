using Microsoft.EntityFrameworkCore;
using MemberCrud.Models;
using Microsoft.Extensions.Configuration;

// Note: This file now reads the database connection string from
// appsettings.json via Microsoft.Extensions.Configuration. The
// DbContextOptions constructor is preserved so unit tests can still
// provide an in-memory provider or other configuration.

namespace MemberCrud.Data
{
    public class MemberCrudDbContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Ministry> Ministries { get; set; }
        public DbSet<MemberMinistry> MemberMinistries { get; set; }
        public DbSet<VolunteerMessage> VolunteerMessages { get; set; }

        // Parameterless constructor used by the application when no external
        // DbContextOptions are provided. The OnConfiguring method will
        // configure SQL Server in that case.
        public MemberCrudDbContext()
        {
        }

        // Constructor that accepts DbContextOptions so unit tests can provide
        // an in-memory provider or other configuration.
        public MemberCrudDbContext(DbContextOptions<MemberCrudDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                    .Build();

                var connectionString = config.GetConnectionString("MemberCrud")
                                       ?? config["ConnectionStrings:MemberCrud"];

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException("Connection string 'MemberCrud' not found in appsettings.json.");
                }

                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}