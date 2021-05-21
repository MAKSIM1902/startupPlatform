using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFundingProject.Data.Models
{
    public class Bonus
    {
        public int BonusId { get; set; }
        public string Name { get; set; }
        public double MoneySum { get; set;}
        public string Description { get; set; }
        public int CompanyId { get; set; }
    }
}
