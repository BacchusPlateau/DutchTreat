using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IDutchRepository dutchRepository;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(IDutchRepository dutchRepository, ILogger<ProductsController> logger)
        {
            this.dutchRepository = dutchRepository;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(dutchRepository.GetAllProducts());
            } catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return BadRequest("Failed to get products");
            }
        }
    }
}
