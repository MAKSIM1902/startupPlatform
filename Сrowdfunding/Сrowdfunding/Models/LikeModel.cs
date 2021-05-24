using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Сrowdfunding.Models
{
    public class Like
    {
        public int LikeId { get; set; }
        public int CommentId { get; set; }
        public string Username { get; set; }
        public Comment Comment { get; set; }
    }
}
