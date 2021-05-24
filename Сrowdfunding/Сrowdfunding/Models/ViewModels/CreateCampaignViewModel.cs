using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Сrowdfunding.Models.ViewModels
{
    public class CreateCampaignViewModel
    {
        public Campaign Campaign { get; set; }
        public List<Category> Categories { get; set; }
    }
}
