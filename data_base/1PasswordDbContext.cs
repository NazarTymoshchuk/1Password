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
            optionsBuilder.UseSqlServer(@"workstation id=1PasswordDb.mssql.somee.com;packet size=4096;user id=Nazar_SQLLogin_1;pwd=s1m4ra7oyo;data source=1PasswordDb.mssql.somee.com;persist security info=False;initial catalog=1PasswordDb");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfig).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryConfig).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountsConfig).Assembly);
            modelBuilder.SeedClients();
            modelBuilder.SeedAccounts();
            modelBuilder.SeedCategories();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
