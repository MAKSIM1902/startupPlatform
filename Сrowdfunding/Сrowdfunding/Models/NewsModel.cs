using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Сrowdfunding.Models
{
    public class News
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string NewsContent { get; set; }
        public string Author { get; set; }
        public DateTime PostDate { get; set; }
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }
    }
}
