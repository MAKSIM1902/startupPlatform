using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Сrowdfunding.Models.ViewModels
{
    public class NewsViewModel
    {
        public Campaign Campaign { get; set; }
        public List<News> News { get; set; }
    }
}
