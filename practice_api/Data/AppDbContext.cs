using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using practice_api.Data;


namespace practice_api.Data
{
    public class AppDbContext: IdentityDbContext<AppIdentityUser,AppIdentityRole,string>
    {

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Blogs> Blogs { get; set; }

        public DbSet<RefreshTokens> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // 🔑 required!
            modelBuilder.Entity<AppIdentityUser>()
                .HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RefreshTokens>()
                .HasIndex(rt => rt.Token)
                .IsUnique();
        }
    }


}
