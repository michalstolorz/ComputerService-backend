using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ComputerService.Data.Models;
using ComputerService.Data.EntityConfiguration;

namespace ComputerService.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        //public DbSet<Customer> Customer => Set<Customer>();
        public DbSet<EmployeeRepair> EmployeeRepairs => Set<EmployeeRepair>();
        public DbSet<Invoice> Invoice => Set<Invoice>();
        public DbSet<Part> Part => Set<Part>();
        public DbSet<Repair> Repair => Set<Repair>();
        public DbSet<RepairType> RepairType => Set<RepairType>();
        public DbSet<RequiredRepairType> RequiredRepairType => Set<RequiredRepairType>();
        public DbSet<Role> Role => Set<Role>();
        public DbSet<UsedPart> UsedPart => Set<UsedPart>();
        public DbSet<User> User => Set<User>();
        public DbSet<UserRole> UserRole => Set<UserRole>();


        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepairConfiguration).Assembly);
        }
    }
}
