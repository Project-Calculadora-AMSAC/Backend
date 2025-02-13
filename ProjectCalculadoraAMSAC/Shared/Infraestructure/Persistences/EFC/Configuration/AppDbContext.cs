using System.Collections.Immutable;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
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
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<TipoPam> TiposPam { get; set; }
        public DbSet<Estimacion> Estimaciones { get; set; }
        public DbSet<AtributoEstimacion> AtributosEstimacion { get; set; }
        public DbSet<VariablesPam> VariablesPams { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<AuthUser>(authUser =>
            {
                authUser.HasKey(u => u.Id);
                authUser.Property(u => u.Id).IsRequired();
                authUser.Property(u => u.Email).IsRequired().HasMaxLength(255);
                authUser.HasIndex(u => u.Email)
                    .IsUnique()
                    .HasDatabaseName("IX_AuthUser_Email");
                authUser.Property(u => u.PasswordHash).IsRequired();

                authUser.HasMany(u => u.Estimaciones)
                    .WithOne(e => e.AuthUser)
                    .HasForeignKey(e => e.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Configuración de la entidad Proyecto
            builder.Entity<Proyecto>(proyecto =>
            {
                proyecto.HasKey(p => p.ProyectoId);
                proyecto.Property(p => p.ProyectoId).ValueGeneratedOnAdd();
                proyecto.Property(p => p.Name).IsRequired().HasMaxLength(255);
                proyecto.Property(p => p.Descripcion).HasMaxLength(500);
                proyecto.HasMany(p => p.Estimaciones)
                    .WithOne(e => e.Proyecto)
                    .HasForeignKey(e => e.ProyectoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Configuración de la entidad TipoPam 
            builder.Entity<TipoPam>(tipoPam =>
            {
                tipoPam.HasKey(tp => tp.Id);
                tipoPam.Property(tp => tp.Name).IsRequired().HasMaxLength(255);
                tipoPam.Property(tp => tp.Status).HasDefaultValue(true);
                tipoPam.HasMany(tp => tp.Estimaciones)
                    .WithOne(e => e.TipoPam)
                    .HasForeignKey(e => e.TipoPamId)
                    .OnDelete(DeleteBehavior.Restrict);
                tipoPam.HasMany(tp => tp.Variables)
                    .WithOne(c => c.TipoPam)
                    .HasForeignKey(c => c.TipoPamId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Configuración de la entidad `Estimacion`
            builder.Entity<Estimacion>(estimacion =>
            {
                estimacion.HasKey(e => e.EstimacionId);
                estimacion.Property(e => e.CodPam).IsRequired().HasMaxLength(50);
                estimacion.Property(e => e.FechaEstimacion).IsRequired();

                estimacion.HasIndex(e => e.CodPam)
                    .IsUnique()
                    .HasDatabaseName("IX_Estimacion_CodPam");

                estimacion.HasMany(e => e.Atributos)
                    .WithOne(a => a.Estimacion)
                    .HasForeignKey(a => a.EstimacionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Configuración de la entidad `AtributoEstimacion`
            builder.Entity<AtributoEstimacion>(atributo =>
            {
                atributo.HasKey(a => a.AtributoEstimacionId);
                atributo.Property(a => a.Name).IsRequired().HasMaxLength(100);
                atributo.Property(a => a.Valor).IsRequired();
            });
            
            // Configuración de la entidad `VariablesPAM`
            builder.Entity<VariablesPam>(variables =>
            {
                variables.HasKey(c => c.Id);
                variables.Property(c => c.TipoPamId).IsRequired();
            });
        }
            
            
}
}
