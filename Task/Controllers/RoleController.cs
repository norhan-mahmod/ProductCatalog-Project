using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductTask.Models;

namespace ProductTask.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public RoleController(RoleManager<ApplicationRole> roleManager , UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await roleManager.Roles.ToListAsync();
            return View(roles);
        }
        public IActionResult Create()
        {
            return View(new ApplicationRole());
        }
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationRole role)
        {
            if (ModelState.IsValid)
            {
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("Index");
            }
            return View(role);
        }
        public async Task<IActionResult> Details(string roleId , string viewName = "Details")
        {
            if (roleId == null)
                return NotFound();
            var role = await roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound();
            return View(viewName, role);
        }
        public async Task<IActionResult> Update(string roleId)
        {
            return await Details(roleId, "Update");
        }
        [HttpPost]
        public async Task<IActionResult> Update(ApplicationRole appRole)
        {
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(appRole.Id);
                if (role is null)
                    return NotFound();
                role.Name = appRole.Name;
                role.NormalizedName = appRole.Name.ToUpper();
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("Index");
            }
            return View(appRole);
        }
        public async Task<IActionResult> Delete(string roleId)
        {
            if (roleId == null)
                return NotFound();
            var role = await roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound();
            await roleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            if (roleId == null)
                return NotFound();
            var role = await roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound();
            List<UserInRoleViewModel> usersInRole = new List<UserInRoleViewModel>();
            foreach(var user in await userManager.Users.ToListAsync())
            {
                var userInRoleViewModel = new UserInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                    userInRoleViewModel.IsSelected = true;
                else
                    userInRoleViewModel.IsSelected = false;
                usersInRole.Add(userInRoleViewModel);
            }
            ViewBag.roleId = roleId;
            return View(usersInRole);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(List<UserInRoleViewModel> users , string roleId)
        {
            if (roleId == null)
                return NotFound();
            var role = await roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound();
            foreach(var user in users)
            {
                var appUser = await userManager.FindByIdAsync(user.UserId);
                if (user.IsSelected && !await userManager.IsInRoleAsync(appUser, role.Name))
                    await userManager.AddToRoleAsync(appUser, role.Name);
                else if (!user.IsSelected && await userManager.IsInRoleAsync(appUser, role.Name))
                    await userManager.AddToRoleAsync(appUser, role.Name);
            }
            return RedirectToAction("Update" , new { roleId = roleId });    
        }
    }
}
