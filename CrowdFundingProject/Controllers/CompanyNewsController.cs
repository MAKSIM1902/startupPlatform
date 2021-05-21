using CrowdFundingProject.Data.Models;
using CrowdFundingProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFundingProject.Controllers
{
    public class CompanyNewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CompanyNewsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int companyId)
        {
            IEnumerable<CompanyNews> companyNews = _context.CompanyNews.Where(p => p.CompanyId.Equals(companyId));
            return View(companyNews);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CompanyNews companyNews, int companyId)
        {
            companyNews.CompanyId = companyId;
            companyNews.DatePost = DateTime.Now;
            _context.CompanyNews.Add(companyNews);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
