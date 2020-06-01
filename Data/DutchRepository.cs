using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext ctx;
        private readonly ILogger<DutchRepository> logger;

        public DutchRepository(DutchContext ctx, ILogger<DutchRepository> logger)
        {
            this.ctx = ctx;
            this.logger = logger;
        }

        public void AddEntity(object model)
        {
            ctx.Add(model);
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {

            if (includeItems)
            {
                var orders = ctx.Orders
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .ToList();
                return orders;

            } else
            {
                return ctx.Orders
                    .ToList();
            }
        }

        public IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems)
        {
            
            var orders = GetAllOrders(includeItems).Where(orders => orders.User != null && orders.User.UserName == username);
            return orders;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return ctx.Products
                .OrderBy(p => p.Title)
                .ToList();
        }

        public Order GetOrderById(int id)
        {
            return ctx.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.Id == id)
                .FirstOrDefault();
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            try
            {
                return ctx.Products
                    .Where(p => p.Category == category)
                    .OrderBy(p => p.Title)
                    .ToList();
            } catch (Exception ex)
            {
                logger.LogError($"Something went wrong getting products by category {category} and the exception was {ex}");
                return null;
            }
        }

        public bool SaveAll()
        {
            return ctx.SaveChanges() > 0;
        }
    }
}
