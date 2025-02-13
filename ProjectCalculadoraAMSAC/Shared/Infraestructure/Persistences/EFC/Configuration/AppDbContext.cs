using System.Collections.Immutable;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;
using ProjectCalculadoraAMSAC.User.Domain.Model.Aggregates;

namespace ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Configuration
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

     
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
            builder.EnableSensitiveDataLogging();
            builder.AddCreatedUpdatedInterceptor();

            base.OnConfiguring(builder);
        }
        
        public DbSet<AuthUser> AuthUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AuthUser>().HasKey(u => u.Id);
            builder.Entity<AuthUser>().Property(u => u.Id).IsRequired();
            builder.Entity<AuthUser>()
                .HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_AuthUser_Email");
            builder.Entity<AuthUser>().Property(u => u.PasswordHash).IsRequired();        }
     
}
}
