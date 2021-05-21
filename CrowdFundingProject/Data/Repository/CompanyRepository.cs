using CrowdFundingProject.Interfaces;
using CrowdFundingProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFundingProject.Data.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CompanyRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IEnumerable<Company> Companies => _applicationDbContext.Companies.Include(c => c.Category);
        public IEnumerable<Company> PrefferedCompanies => _applicationDbContext.Companies.Where(p => p.IsFavorite).Include(c => c.Category);
     
        public Company GetCompanyById(int companyId) => _applicationDbContext.Companies.FirstOrDefault(p => p.CompanyId == companyId);
    }
    
}
