using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Сrowdfunding.Data;
using Сrowdfunding.Models;

namespace Сrowdfunding
{
    public class CategoryInitial
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category
                    {
                        Name = "Electronics"
                    },
                    new Category
                    {
                        Name = "Education"
                    },
                    new Category
                    {
                        Name = "Sport and healthy lifestyle"
                    },
                    new Category
                    {
                        Name = "Nature"
                    },
                    new Category
                    {
                        Name = "Other"
                    }
                    );
                context.SaveChanges();

            }
        }
    }
}
