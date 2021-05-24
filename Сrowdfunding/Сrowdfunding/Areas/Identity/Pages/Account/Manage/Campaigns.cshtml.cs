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
    
    public class CampaignsModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private ApplicationDbContext _context;
        private readonly ILogger<CampaignsModel> _logger;

        [TempData]
        public string StatusMessage { get; set; }

        public InputCampaign Campaign { get; set; }

        public CampaignsModel(ILogger<CampaignsModel> logger, UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public class InputCampaign
        {
            public List<Campaign> Campaigns { get; set; }
            public IdentityUser User { get; set; }
        }

        public IActionResult OnGet()
        {
            string creator = _userManager.GetUserAsync(User).Result.UserName;
            List<Campaign> campaigns;
            if (this.User.IsInRole("Admin") || this.User.IsInRole("Moderator"))
            {
                campaigns = _context.Campaigns.ToList();
            }
            else
            {
                campaigns = _context.Campaigns.Where(p => p.Author == creator).ToList();
            }
            Campaign = new InputCampaign
            {
                Campaigns = campaigns,
                User = _userManager.GetUserAsync(User).Result
            };
            return Page();
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {

            _context.Campaigns.Remove(await _context.Campaigns.FindAsync(id));
            await _context.SaveChangesAsync();
            string author = _userManager.GetUserAsync(User).Result.UserName;
            Campaign = new InputCampaign
            {
                Campaigns = _context.Campaigns.Where(p => p.Author == author).ToList()
            };
            StatusMessage = "Your post has been deleted.";
            return RedirectToPage();

        }
    }
}
