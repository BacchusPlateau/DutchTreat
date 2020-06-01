using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
        public DutchContext(DbContextOptions<DutchContext> options): base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasData(new Order()
                {
                    Id = 1,
                    OrderDate = DateTime.UtcNow,
                    OrderNumber = "12345"
                });
            
        }

        
    }

    


}
