using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Data.Contexts
{
    public class BilgeShopContextFactory : IDesignTimeDbContextFactory<BilgeShopContext>
    {
        public BilgeShopContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BilgeShopContext>();
            optionsBuilder.UseSqlServer("server =.; database= BilgeShop; uid= sa; pwd=123;");

            return new BilgeShopContext(optionsBuilder.Options);
        }
    }
}
