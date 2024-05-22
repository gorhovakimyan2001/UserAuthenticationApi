using Microsoft.EntityFrameworkCore;
using TestProjectDbPart.Models;

namespace TestProjectDbPart.Data
{
    public class UserContext: DbContext
    {

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<AuthenticationModel> Authentications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().ToTable("Users");
            modelBuilder.Entity<UserModel>().HasKey(u => u.Id);

            modelBuilder.Entity<AuthenticationModel>().ToTable("Authentications");
            modelBuilder.Entity<AuthenticationModel>().HasKey(a => a.Email);
        }
    }
}
