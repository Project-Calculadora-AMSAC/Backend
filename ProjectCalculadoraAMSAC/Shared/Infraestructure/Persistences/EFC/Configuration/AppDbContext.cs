﻿using System.Collections.Immutable;
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
        public DbSet<Proyecto> Proyecto { get; set; }
        public DbSet<TipoPam> TipoPam { get; set; }
        public DbSet<Estimacion> Estimaciones { get; set; }
        public DbSet<AtributosPam> AtributoPam { get; set; }
        public DbSet<VariablesPam> VariablePam { get; set; }
        public DbSet<ValorAtributoEstimacion> ValoresAtributosEstimacion { get; set; }
        public DbSet<UnidadDeMedida> UnidadesDeMedida { get; set; }
        public DbSet<CostoEstimado> CostoEstimados { get; set; }
        public DbSet<AuthUserRefreshToken> AuthUsersRefreshTokens { get; set; }

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
            
            builder.Entity<AuthUserRefreshToken>(refreshToken =>
            {
                refreshToken.HasKey(rt => rt.Id);

                refreshToken.HasOne(rt => rt.AuthUser)
                    .WithMany()
                    .HasForeignKey(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            
            // Configuración de la entidad Proyecto
            builder.Entity<Proyecto>(proyecto =>
            {
                proyecto.HasKey(p => p.ProyectoId);
                proyecto.Property(p => p.ProyectoId).ValueGeneratedOnAdd();
                proyecto.Property(p => p.Name).IsRequired().HasMaxLength(255);
                proyecto.HasIndex(p => p.Name)
                    .IsUnique();
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
                tipoPam.Property(tp => tp.Status).IsRequired().HasDefaultValue(true);
                
                tipoPam.HasMany(tp => tp.Estimaciones)
                    .WithOne(e => e.TipoPam)
                    .HasForeignKey(e => e.TipoPamId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                tipoPam.HasMany(tp => tp.Atributos)
                    .WithOne(e => e.TipoPam)
                    .HasForeignKey(e => e.TipoPamId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                tipoPam.HasMany(tp => tp.Variables)
                    .WithOne(c => c.TipoPam)
                    .HasForeignKey(c => c.TipoPamId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Configuracion AtributosPam 
            builder.Entity<AtributosPam>(atributoPam =>
            {
                atributoPam.HasKey(a => a.AtributoPamId);
                atributoPam.Property(a => a.Nombre).IsRequired().HasMaxLength(255);
                atributoPam.Property(a => a.TipoDato).IsRequired();
                
                atributoPam.HasOne(a => a.UnidadDeMedida)
                    .WithMany()
                    .HasForeignKey(a => a.UnidadDeMedidaId)
                    .OnDelete(DeleteBehavior.Restrict);
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

              
                estimacion.HasOne(e => e.Proyecto)
                    .WithMany(p => p.Estimaciones)
                    .HasForeignKey(e => e.ProyectoId)
                    .OnDelete(DeleteBehavior.Cascade);

                estimacion.HasOne(e => e.TipoPam)
                    .WithMany(t => t.Estimaciones)
                    .HasForeignKey(e => e.TipoPamId)
                    .OnDelete(DeleteBehavior.Cascade);

                estimacion.HasMany(e => e.Valores)
                    .WithOne(v => v.Estimacion)
                    .HasForeignKey(v => v.EstimacionId)
                    .OnDelete(DeleteBehavior.Cascade);
                estimacion.HasOne(e => e.CostoEstimado)
                    .WithOne(c => c.Estimacion)
                    .HasForeignKey<CostoEstimado>(c => c.EstimacionId)
                    .OnDelete(DeleteBehavior.Cascade); // Si se borra la estimación, se borra su costo
            });
            
            builder.Entity<CostoEstimado>(costo =>
            {
                costo.HasKey(c => c.Id);
                costo.Property(c => c.CostoDirecto).HasColumnType("decimal(18,2)");
                costo.Property(c => c.GastosGenerales).HasColumnType("decimal(18,2)");
                costo.Property(c => c.Utilidades).HasColumnType("decimal(18,2)");
                costo.Property(c => c.IGV).HasColumnType("decimal(18,2)");
                costo.Property(c => c.ExpedienteTecnico).HasColumnType("decimal(18,2)");
                costo.Property(c => c.Supervision).HasColumnType("decimal(18,2)");
                costo.Property(c => c.GestionProyecto).HasColumnType("decimal(18,2)");
                costo.Property(c => c.Capacitacion).HasColumnType("decimal(18,2)");
                costo.Property(c => c.Contingencias).HasColumnType("decimal(18,2)");
                costo.Property(c => c.SubTotal).HasColumnType("decimal(18,2)");
                costo.Property(c => c.SubTotalObras).HasColumnType("decimal(18,2)");
                costo.Property(c => c.TotalEstimado).HasColumnType("decimal(18,2)");

                // ✅ Relación con Estimación
                costo.HasOne(c => c.Estimacion)
                    .WithOne(e => e.CostoEstimado)
                    .HasForeignKey<CostoEstimado>(c => c.EstimacionId);
            });
            
            // Configuración de la entidad `AtributoEstimacion`
            builder.Entity<ValorAtributoEstimacion>(valor =>
            {
                valor.HasKey(v => v.Id);
                valor.Property(v => v.Valor).IsRequired();

                valor.HasOne(v => v.Estimacion)
                    .WithMany(e => e.Valores)
                    .HasForeignKey(v => v.EstimacionId)
                    .OnDelete(DeleteBehavior.Cascade);

                valor.HasOne(v => v.AtributoPam)
                    .WithMany()
                    .HasForeignKey(v => v.AtributoPamId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Configuración de la entidad `VariablesPAM`
            builder.Entity<VariablesPam>(variable =>
            {
                variable.HasKey(v => v.Id);
                variable.Property(v => v.Nombre).IsRequired().HasMaxLength(255);
                variable.Property(v => v.Valor).IsRequired();
            });
            
            builder.Entity<UnidadDeMedida>(unidad =>
            {
                unidad.HasKey(u => u.Id);
                unidad.Property(u => u.Nombre).IsRequired().HasMaxLength(255);
                
            });
        }
            
            
}
}
