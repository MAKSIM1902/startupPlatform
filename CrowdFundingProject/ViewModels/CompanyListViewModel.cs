using CrowdFundingProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFundingProject.ViewModels
{
    public class CompanyListViewModel
    {
        public IEnumerable<Company> AllCompanies { get; set; }

        public string CurrentCategory { get; set; }
    }
}
