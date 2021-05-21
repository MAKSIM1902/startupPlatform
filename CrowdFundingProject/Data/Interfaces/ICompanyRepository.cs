using CrowdFundingProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFundingProject.Interfaces
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> Companies{ get; }
        IEnumerable<Company> PrefferedCompanies { get; }
        Company GetCompanyById(int companyId);
    }
}
