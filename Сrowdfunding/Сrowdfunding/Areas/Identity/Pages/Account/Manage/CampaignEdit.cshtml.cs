using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Сrowdfunding.Data;
using Сrowdfunding.Models;

namespace Сrowdfunding.Areas.Identity.Pages.Account.Manage
{
    public class CampaignEditModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private ApplicationDbContext _context;
        private readonly ILogger<CampaignsModel> _logger;

        [TempData]
        public string StatusMessage { get; set; }

        public CampaignEditView CampaignEdit { get; set; }

        public CampaignEditModel(ILogger<CampaignsModel> logger, UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public class CampaignEditView
        {
            public Campaign Campaign { get; set; }
            public List<Category> Categories { get; set; }
        }

        public IActionResult OnGet(int id)
        {
            var campaign = _context.Campaigns.Find(id);
            var categories = _context.Categories.ToList();
            CampaignEdit = new CampaignEditView
            {
                Campaign = campaign,
                Categories = categories                
            };
            return Page();
        }

        public IActionResult OnPost(Campaign campaign)
        {

            var _campaign = _context.Campaigns.Find(campaign.Id);
            _campaign.Category = campaign.Category;
            _campaign.CategoryId = campaign.CategoryId;
            _campaign.Name = campaign.Name;
            _campaign.ShortDescription = campaign.ShortDescription;
            _campaign.Story = campaign.Story;
            _context.SaveChanges();
            return RedirectToPage("Campaigns");

        }
    }
}
