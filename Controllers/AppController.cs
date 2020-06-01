using DutchTreat.Data;
using DutchTreat.Models;
using DutchTreat.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService mailService;
        private readonly IDutchRepository dutchRepository;

        public AppController(IMailService mailService, IDutchRepository dutchRepository)
        {
            this.mailService = mailService;
            this.dutchRepository = dutchRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
    
        [HttpGet("contact")]
        public IActionResult Contact()
        {
            ViewBag.Title = "Contact Us";

            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                mailService.SendMessage(model.Name, model.Subject, model.Message);
                ViewBag.UserMessage = "Mail sent.";
            } else
            {
                //done on client
            }
            return View();
        }

    
        public IActionResult About()
        {
            ViewBag.Title = "About Us";

            return View();
        }

        [Authorize]
        public IActionResult Shop()
        {
            var results = dutchRepository.GetAllProducts();

            return View(results);
        }

    
    }

}
