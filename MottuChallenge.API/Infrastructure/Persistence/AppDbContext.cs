using Microsoft.EntityFrameworkCore;
using MottuChallenge.API.Entities;

namespace MottuChallenge.API.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<MotorcycleEntity> MotorcycleEntities { get; set; }
        public DbSet<DeliveryPersonEntity> DeliveryPeopleEntities { get; set; }
        
        public DbSet<RentalEntity> RentalEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração da MotorcycleEntity...
            modelBuilder.Entity<MotorcycleEntity>(entity =>
            {
                entity.HasKey(e => e.Id); // Chave primária atualizada
                entity.HasIndex(e => e.LicensePlate).IsUnique();
                entity.Property(e => e.Id).HasMaxLength(50);
                entity.Property(e => e.Model).HasMaxLength(100);
                entity.Property(e => e.LicensePlate).HasMaxLength(8);
            });

            // --- CONFIGURAÇÃO ATUALIZADA PARA DELIVERYPERSONENTITY ---
            modelBuilder.Entity<DeliveryPersonEntity>(entity =>
            {
                entity.HasKey(e => e.Id); // Chave primária atualizada
                entity.HasIndex(e => e.Cnpj).IsUnique();
                entity.HasIndex(e => e.CnhNumber).IsUnique();
                
                entity.Property(e => e.Id).HasMaxLength(50);
                entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Cnpj).HasMaxLength(14).IsRequired();
                entity.Property(e => e.CnhNumber).HasMaxLength(11).IsRequired();
                
                entity.Property(e => e.CnhType).HasConversion<string>().HasMaxLength(2);
               
            });
            
            modelBuilder.Entity<RentalEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TotalCost).HasColumnType("decimal(18,2)");

                // Define as relações de chave estrangeira
                entity.HasOne(r => r.DeliveryPerson)
                    .WithMany() // Um entregador pode ter muitas locações
                    .HasForeignKey(r => r.DeliveryPersonId);

                entity.HasOne(r => r.Motorcycle)
                    .WithMany() // Uma moto pode ter muitas locações
                    .HasForeignKey(r => r.MotorcycleId);
            });
        }
    }
}