using CrowdFundingProject.Data.Models;
using CrowdFundingProject.Interfaces;
using CrowdFundingProject.Models;
using CrowdFundingProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFundingProject.Controllers
{
    public class CompanyController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly ICompanyRepository _allCompanies;
        public CompanyController (ICompanyRepository allCompanies, ApplicationDbContext context)
        {
            _allCompanies = allCompanies;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Company company)
        {
            company.CreationDate = DateTime.Now;
            company.MoneyNow = 0;
            company.UserId = User.Identity.Name;
            _context.Companies.Add(company);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult ThisCompany(int companyId)
        {
            Company companies = _allCompanies.Companies.Where(i => i.CompanyId.Equals(companyId)).FirstOrDefault();
            return View(companies);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            Company company = _allCompanies.Companies.FirstOrDefault(p => p.CompanyId == id);
            if (company != null)
            {
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Account");
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Company company =  await _context.Companies.FirstOrDefaultAsync(p => p.CompanyId == id);
                if (company != null)
                    return View(company);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Company company)
        {
            _context.Update(company).Property(p=>p.CompanyId).IsModified = false;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Account");
        }
        
    }
}
