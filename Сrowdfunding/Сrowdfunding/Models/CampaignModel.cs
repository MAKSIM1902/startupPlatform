using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Сrowdfunding.Models
{
    public class Campaign
    {
        public int Id { get; set; }
        public string Author { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        [Required]
        public int TotalSum { get; set; }
        public int RemainSum { get; set; }
        public DateTime BeginTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        public string Story { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string ImageUrl { get; set; }
        public bool Ended { get; set; }
        [NotMapped]
        public virtual IFormFile ImageFile { get; set; }
        public string ImageStorageName { get; set; }
        public Category Category { get; set; }
        public List<Comment> Comments { get; set; }
        public List<News> News { get; set; }
        public List<Reward> Rewards { get; set; }
        public List<Rating> Ratings { get; set; }

    }
}
