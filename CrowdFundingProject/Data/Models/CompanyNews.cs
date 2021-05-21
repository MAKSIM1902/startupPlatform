using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFundingProject.Data.Models
{
    public class CompanyNews
    {
        public int Id { get; set; }
        public string NewsName { get; set; }
        public string Description { get; set;}
        public string ImageLink { get; set; }
        public DateTime DatePost { get; set; }
        public int CompanyId { get; set; }
    }
}
