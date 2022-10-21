using data_base.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data_base
{
    public static class Initializer
    {
        public static void SeedClients(this ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<User>().HasData(new User[]
                {
                    new User()
                    {
                        Id = 1,
                        Password = "1234"
                    },
                }
            );*/
        }
        public static void SeedCategories(this ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<Category>().HasData(new Category[]
                {
                    new Category()
                    {
                        Id = 1,
                        Name = "Category1",
                    },
                }
            );*/
        }
        public static void SeedAccounts(this ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<Account>().HasData(new Account[]
                {
                    new Account()
                    {
                        Id = 1,
                        Password = "4321",
                        Name = "Facebook",
                        UserId = 1,
                        CategoryId = 1,
                    },
                    new Account()
                    {
                        Id = 2,
                        Password = "4321",
                        Name = "Telegram",
                        UserId = 1,
                        CategoryId = 1,
                    },
                }
            );*/
        }
    }
}
