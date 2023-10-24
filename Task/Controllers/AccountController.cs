using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProductTask.Models;

namespace ProductTask.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager ,
                                SignInManager<ApplicationUser> signInManager,
                                RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
			this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public IActionResult SignUp()
        {
            return View(new RegisterViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    Email = registerViewModel.Email,
                    UserName = registerViewModel.Email.Split('@')[0]
                };
                var result = await userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                    return RedirectToAction("Login");
            }
            return View(registerViewModel);
        }
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(loginViewModel.Email);
                if (user is null)
                    return NotFound();
                var isPasswordCorrect = await userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (isPasswordCorrect)
                {
					var result = await signInManager.PasswordSignInAsync(user, loginViewModel.Password,false, false);
                    if (result.Succeeded)
                        return RedirectToAction("Index", "Product");
				}
                else
                {
                    ModelState["Password"].ValidationState = ModelValidationState.Invalid;
                    ModelState.AddModelError("Password", "Invalid Password!");
                }

            }
            return View(loginViewModel);
        }
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> LogInAsAdmin()
        {
            var user = await userManager.FindByEmailAsync("AdminUser@gmail.com");
            string password = "Abc@12";
            if (user is null)
            {
                var AdminUser = new ApplicationUser()
                {
                    Email = "AdminUser@gmail.com",
                    UserName = "AdminUser"
                };
                var AdminRole = new ApplicationRole()
                {
                    Name = "Admin"
                };
                await userManager.CreateAsync(AdminUser, password);
                await roleManager.CreateAsync(AdminRole);
                await userManager.AddToRoleAsync(AdminUser, AdminRole.Name);
                await signInManager.PasswordSignInAsync(AdminUser, password, false, false);
                return RedirectToAction("Index", "Product");
            }
            await signInManager.PasswordSignInAsync(user, password, false, false);
            return RedirectToAction("Index", "Product");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
