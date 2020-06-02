using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace DutchTreat.Controllers
{
    [Route("api/orders/{orderid}/items")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {

        private readonly IDutchRepository dutchRepository;
        private readonly ILogger<ProductsController> logger;
        private readonly IMapper mapper;

        public OrderItemsController(
            IDutchRepository dutchRepository,
            ILogger<ProductsController> logger,
            IMapper mapper
            )
        {
            this.dutchRepository = dutchRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(int orderId)
        {
            var order = dutchRepository.GetOrderById(User.Identity.Name, orderId);
            if (order != null)
                return Ok(mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(order.Items));
            else
                return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int orderId, int id)
        {
            var order = dutchRepository.GetOrderById(User.Identity.Name, orderId);
            if (order != null)
            {
                var item = order.Items.Where(i => i.Id == id).FirstOrDefault();
                if( item != null)
                    return Ok(mapper.Map<OrderItem,OrderItemViewModel>(item));
            }
           
            return NotFound();
        }

    }
}
