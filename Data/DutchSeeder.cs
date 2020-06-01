using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext dutchContext;
        private readonly IWebHostEnvironment env;
        private readonly UserManager<StoreUser> userManager;

        public DutchSeeder(DutchContext dutchContext, IWebHostEnvironment env, UserManager<StoreUser> userManager)
        {
            this.dutchContext = dutchContext;
            this.env = env;
            this.userManager = userManager;
        }

        public async Task SeedAsync()
        {
            dutchContext.Database.EnsureCreated();

            StoreUser user = await userManager.FindByEmailAsync("tigerguy@gmail.com");
            if (user == null)
            {
                user = new StoreUser
                {
                    FirstName = "Bret",
                    LastName = "Williams",
                    Email = "tigerguy@gmail.com",
                    UserName = "tigerguy@gmail.com"
                };
                var result = await userManager.CreateAsync(user, "K0mbat134!");
                if(result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Couldn't create new user");
                }
            }

            if(!dutchContext.Products.Any())
            {
                var file = Path.Combine(env.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(file);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                dutchContext.Products.AddRange(products);

                var order = dutchContext.Orders.Where(o => o.Id == 1).FirstOrDefault();
                if (order != null)
                {
                    order.User = user;
                    order.Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    }; 
                }

                dutchContext.SaveChanges();
            }

        }

    }
}
