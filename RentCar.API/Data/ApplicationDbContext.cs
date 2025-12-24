using Microsoft.EntityFrameworkCore;
using RentCar.API.Models;

namespace RentCar.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<MsCustomer> MsCustomers { get; set; }
        public DbSet<MsCar> MsCars { get; set; }
        public DbSet<MsCarImages> MsCarImages { get; set; }
        public DbSet<TrRental> TrRentals { get; set; }
        public DbSet<LtPayment> LtPayments { get; set; }
        public DbSet<TrMaintenance> TrMaintenances { get; set; }
        public DbSet<MsEmployee> MsEmployees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==================== TABLE NAMES ====================
            modelBuilder.Entity<MsCustomer>().ToTable("MsCustomer");
            modelBuilder.Entity<MsCar>().ToTable("MsCar");
            modelBuilder.Entity<MsCarImages>().ToTable("MsCarImages");
            modelBuilder.Entity<TrRental>().ToTable("TrRental");
            modelBuilder.Entity<LtPayment>().ToTable("LtPayment");
            modelBuilder.Entity<TrMaintenance>().ToTable("TrMaintenance");
            modelBuilder.Entity<MsEmployee>().ToTable("MsEmployee");

            // ==================== RELATIONSHIPS ====================

            // MsCustomer -> TrRental (One-to-Many)
            modelBuilder.Entity<TrRental>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Rentals)
                .HasForeignKey(r => r.Customer_id)
                .OnDelete(DeleteBehavior.Restrict);

            // MsCar -> TrRental (One-to-Many)
            modelBuilder.Entity<TrRental>()
                .HasOne(r => r.Car)
                .WithMany(c => c.Rentals)
                .HasForeignKey(r => r.Car_id)
                .OnDelete(DeleteBehavior.Restrict);

            // MsCar -> MsCarImages (One-to-Many)
            modelBuilder.Entity<MsCarImages>()
                .HasOne(i => i.Car)
                .WithMany(c => c.CarImages)
                .HasForeignKey(i => i.Car_id)
                .OnDelete(DeleteBehavior.Cascade);

            // TrRental -> LtPayment (One-to-Many)
            modelBuilder.Entity<LtPayment>()
                .HasOne(p => p.Rental)
                .WithMany(r => r.Payments)
                .HasForeignKey(p => p.Rental_id)
                .OnDelete(DeleteBehavior.Cascade);

            // MsCar -> TrMaintenance (One-to-Many)
            modelBuilder.Entity<TrMaintenance>()
                .HasOne(m => m.Car)
                .WithMany(c => c.Maintenances)
                .HasForeignKey(m => m.Car_id)
                .OnDelete(DeleteBehavior.Restrict);

            // MsEmployee -> TrMaintenance (One-to-Many)
            modelBuilder.Entity<TrMaintenance>()
                .HasOne(m => m.Employee)
                .WithMany(e => e.Maintenances)
                .HasForeignKey(m => m.Employee_id)
                .OnDelete(DeleteBehavior.Restrict);

            // ==================== DECIMAL PRECISION ====================
            modelBuilder.Entity<MsCar>()
                .Property(c => c.Price_per_day)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TrRental>()
                .Property(r => r.Total_price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LtPayment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TrMaintenance>()
                .Property(m => m.Cost)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MsEmployee>()
                .Property(e => e.Email)
                .HasPrecision(18, 2);

            // ==================== INDEXES ====================
            // Index on foreign keys for faster queries
            modelBuilder.Entity<TrRental>()
                .HasIndex(r => r.Customer_id);

            modelBuilder.Entity<TrRental>()
                .HasIndex(r => r.Car_id);

            modelBuilder.Entity<MsCarImages>()
                .HasIndex(i => i.Car_id);

            modelBuilder.Entity<LtPayment>()
                .HasIndex(p => p.Rental_id);

            modelBuilder.Entity<TrMaintenance>()
                .HasIndex(m => m.Car_id);

            modelBuilder.Entity<TrMaintenance>()
                .HasIndex(m => m.Employee_id);

            // Index on frequently queried fields
            modelBuilder.Entity<MsCar>()
                .HasIndex(c => c.Status);

            modelBuilder.Entity<MsCustomer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<TrRental>()
                .HasIndex(r => r.Rental_date);

            modelBuilder.Entity<TrRental>()
                .HasIndex(r => r.Payment_status);

            // Uncomment to add initial data
            /*
            modelBuilder.Entity<MsEmployee>().HasData(
                new MsEmployee
                {
                    Employee_id = Guid.NewGuid().ToString(),
                    Name = DateTime.Now,
                    Position = "Manager",
                    Email = 0,
                    Phone_number = "123-456-7890"
                }
            );
            */
        }
    }
}
