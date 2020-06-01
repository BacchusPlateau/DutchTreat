using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IDutchRepository dutchRepository;
        private readonly ILogger<ProductsController> logger;
        private readonly IMapper mapper;

        public OrdersController(
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
        public IActionResult Get(bool includeItems = true)
        {
            try
            {
                var username = User.Identity.Name;

                var results = dutchRepository.GetAllOrdersByUser(username, includeItems);

                return Ok(
                    mapper.Map<IEnumerable<Order>, 
                        IEnumerable<OrderViewModel>>(results));
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to get orders {ex}");
                return BadRequest("Failed to get orders");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order = dutchRepository.GetOrderById(id);

                if (order != null)
                {
                    return Ok(mapper.Map<Order, OrderViewModel>(order));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to get order {id}: {ex}");
                return BadRequest("Failed to get order");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]OrderViewModel model) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var order = mapper.Map<OrderViewModel, Order>(model);

                    if (order.OrderDate == DateTime.MinValue)
                    {
                        order.OrderDate = DateTime.Now;
                    }

                    dutchRepository.AddEntity(order);
                    if (dutchRepository.SaveAll())
                    {
                        return Created($"/api/orders/{order.Id}", mapper.Map<Order, OrderViewModel>(order));
                    }
                } else
                {
                    return BadRequest(ModelState);
                }
            }catch (Exception ex)
            {
                logger.LogError($"Failed to save order {ex}");
            }
            return BadRequest("Could not save order");
        }

    }
}
