using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Сrowdfunding.Models
{
    public class Rating
    {
        [Key]
        public int RateId { get; set; }
        public int Rate { get; set; } = 0;
        public int CampaignId { get; set; }
        public string Username { get; set; }
        public Campaign Campaign { get; set; }
    }
}
