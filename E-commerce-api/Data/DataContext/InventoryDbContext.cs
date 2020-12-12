using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShopBridge.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Data.DataContext
{
    public class InventoryDbContext:DbContext
    {
        private IConfiguration Configuration { get; set; }

        public InventoryDbContext()
        {

        }
        public InventoryDbContext(DbContextOptions options):base (options)
        {

        }
        DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(FetchDbConnectionString());
            }
        }

        private string FetchDbConnectionString()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            return Configuration.GetValue<string>("DbConnectionString");
        }
    }
}
