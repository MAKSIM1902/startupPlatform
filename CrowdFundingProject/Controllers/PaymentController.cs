using CrowdFundingProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFundingProject.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PaymentController (ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult> MoneyTransfer(int? companyId)
        {
            if (companyId != null)
            {
                Company company = await _context.Companies.FirstOrDefaultAsync(p => p.CompanyId == companyId);
                if (company != null)
                    return View(company);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> MoneyTransfer(int? companyId, double moneyTransfered)
        {
            if (companyId != null)
            {
                Company company = await _context.Companies.FirstOrDefaultAsync(p => p.CompanyId == companyId);
                company.MoneyNow += moneyTransfered;
                await _context.SaveChangesAsync();
                return RedirectToAction("PaymentSuccess");
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        public IActionResult PaymentSuccess()
        {
            return View();
        }
    }
}
