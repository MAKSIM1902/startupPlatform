using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFundingProject.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public bool IsBlocked { get; set; }
        public int CompanyId { get; set; }
    }
}