using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Сrowdfunding.Data;
using Сrowdfunding.Models.ViewModels;

namespace Сrowdfunding.Controllers
{
    public class RoleManagerController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RoleManagerController> _logger;

        public RoleManagerController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<RoleManagerController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesVm = new List<RoleViewModel>();
            foreach (var user in users)
            {
                var thisVm = new RoleViewModel();
                thisVm.UserId = user.Id;
                thisVm.Email = user.Email;
                thisVm.Roles = await GetUserRoles(user);
                userRolesVm.Add(thisVm);
            }
            return View(userRolesVm);
        }

        private async Task<List<string>> GetUserRoles(IdentityUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        [HttpGet]
        public async Task<IActionResult> Manage(string userId)
        {
            ViewBag.userId = userId;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var model = new List<ManageRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                var manageVm = new ManageRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    User = user
                };
                model.Add(manageVm);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(List<ManageRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles.Where(x => x.ToString() != "User" && x.ToString() != "Admin"));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            return RedirectToAction("Index");
        }
    }
}
