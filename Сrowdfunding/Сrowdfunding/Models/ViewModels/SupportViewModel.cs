using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Сrowdfunding.Models.ViewModels
{
    public class SupportViewModel
    {
        public Campaign Campaign { get; set; }
        public List<Reward> Rewards { get; set; }
    }
}
