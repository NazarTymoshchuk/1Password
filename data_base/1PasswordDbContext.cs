using data_base.Configurations;
using data_base.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace data_base
{
    public class OnePasswordDbContext : DbContext
    {
        public OnePasswordDbContext()
        {
            //this.Database.EnsureDeleted();
            //this.Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            string connect = ConfigurationManager.ConnectionStrings["1PasswordDb"].ConnectionString;
            optionsBuilder.UseSqlServer(connect);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfig).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryConfig).Assembly);
            modelBuilder.SeedClients();
            modelBuilder.SeedAccounts();
            modelBuilder.SeedCategories();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
