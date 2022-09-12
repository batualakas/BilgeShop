using BilgeShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Data.Contexts
{
    public class BilgeShopContext : DbContext
    {
        public BilgeShopContext(DbContextOptions<BilgeShopContext> options) : base(options)
        {

        }
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<ProductEntity> Products => Set<ProductEntity>();
        public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Fluent Validation
            modelBuilder.ApplyConfiguration(new ProductEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());

            //  modelBuilder.ApplyConfigurationsFromAssembly(typeof(BilgeShopContext).Assembly);
            // Projedeki bütün konfigürasyonları OnModelCreating'e ekler.

            modelBuilder.Entity<UserEntity>().HasData(new List<UserEntity>()
                {
                new UserEntity()
                {
                    Id = 1,
                    FirstName = "Bilge",
                    LastName = "Admin",
                    Email = "admin@bilgeadam.com",
                    Password = "123456",
                    UserType = Enums.UserTypeEnum.admin,
                },
                new UserEntity()
                {
                    Id=2,
                    FirstName = "Kullanici",
                    LastName = "KullaniciSoyad",
                    Email = "kullanici@bilgeadam.com",
                    Password = "654321",
                    UserType = Enums.UserTypeEnum.user
                }

            });


            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //}

    }
}
