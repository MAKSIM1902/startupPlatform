using CrowdFundingProject.Data.Models;
using CrowdFundingProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFundingProject.Controllers
{
    public class BonusController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BonusController (ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Bonus bonus, int CompanyId)
        {
            bonus.CompanyId = CompanyId;
            _context.Bonuses.Add(bonus);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
