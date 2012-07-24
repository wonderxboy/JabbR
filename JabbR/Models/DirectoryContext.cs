using System.Data.Entity;
using JabbR.Models.Mapping;

namespace JabbR.Models
{
    public class DirectoryContext : DbContext
    {
        static DirectoryContext()
        {
            Database.SetInitializer<DirectoryContext>(null);
        }

        public DirectoryContext()
            : base("Name=Directory")
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new EmployeeMap());
        }
    }
}
