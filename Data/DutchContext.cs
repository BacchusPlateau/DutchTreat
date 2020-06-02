using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    /*
     
    //install entity framework globally
    C:\dox\code\tutorials\dotnetMvcEntityBootstrapAngular\BigProject\DutchTreat>dotnet tool install dotnet-ef -g

    //examine the configuration in the project
    C:\dox\code\tutorials\dotnetMvcEntityBootstrapAngular\BigProject\DutchTreat>dotnet ef database update

    //create migrations classes
    C:\dox\code\tutorials\dotnetMvcEntityBootstrapAngular\BigProject\DutchTreat>dotnet ef migrations add InitialDb
    
    //create tables based on entities defined
    C:\dox\code\tutorials\dotnetMvcEntityBootstrapAngular\BigProject\DutchTreat>dotnet ef database update
     
    //seed the tables
    C:\dox\code\tutorials\dotnetMvcEntityBootstrapAngular\BigProject\DutchTreat>dotnet ef migrations SeedData  

    //add Identity support
    C:\dox\code\tutorials\dotnetMvcEntityBootstrapAngular\BigProject\DutchTreat>dotnet ef migrations add Identity

     */


    public class DutchContext : IdentityDbContext<StoreUser>
    {
        public static readonly ILoggerFactory MyLoggerFactory
         = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DutchContext(DbContextOptions<DutchContext> options): base(options)
        {
          
        }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options
                .UseLoggerFactory(MyLoggerFactory)
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(p => p.UnitPrice)
                .HasColumnType("decimal(18,2)");

        }

        
    }

    


}
