using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Сrowdfunding.CloudStorage;
using Сrowdfunding.Data;
using Сrowdfunding.Models;
using Сrowdfunding.Models.ViewModels;

namespace Сrowdfunding.Controllers
{
    public class CampaignController : Controller
    {
        private readonly ILogger<CampaignController> _logger;
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private ICloudStorage _cloudStorage;

        public CampaignController(ApplicationDbContext context, UserManager<IdentityUser> userManager, ICloudStorage cloudStorage, ILogger<CampaignController> logger)
        {
            _context = context;
            _userManager = userManager;
            _cloudStorage = cloudStorage;
            _logger = logger;
        }

        
        public async Task<string> GetFilePathAsync(IFormFile file)
        {
            var filePath = Path.GetTempFileName();
            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }
            return filePath;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            var createVm = new CreateCampaignViewModel
            {
                Campaign = new Campaign(),
                Categories = _context.Categories.ToList()
            };
            return View(createVm);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Campaign campaign)
        {
            if (ModelState.IsValid)
            {
                if (campaign.ImageFile != null)
                {                    
                    var file = campaign.ImageFile;   
                    string filePath = await GetFilePathAsync(file);          
                    var uri = _cloudStorage.UploadImage(filePath);
                    campaign.ImageUrl = uri.ToString();
                }
                else
                {
                    campaign.ImageUrl = "https://res.cloudinary.com/dwivxsl5s/image/upload/v1621205266/no-img_if8frz.jpg";
                }
                
                campaign.Author = _userManager.GetUserName(this.User);
                campaign.BeginTime = DateTime.Now;
                campaign.RemainSum = campaign.TotalSum;
                _context.Campaigns.Add(campaign);
                _context.SaveChanges();
            }
            return RedirectPermanent("~/Home/Index");
        }
    }
}
