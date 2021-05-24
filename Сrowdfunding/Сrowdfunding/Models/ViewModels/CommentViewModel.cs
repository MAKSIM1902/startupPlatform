using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Сrowdfunding.Models.ViewModels
{
    public class CommentViewModel
    {
        public Campaign Campaign { get; set; }
        public List<Comment> Comments { get; set; }
        public Rating Rating { get; set; }
    }
}
