using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebUniqlo.Enums;
using WebUniqlo.Models;
using WebUniqlo.ViewModel.User;

namespace Uniqlo.Controllers
{
    public class AccountController(UserManager<User> _u, SignInManager<User> _sign) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserCreateVM um)
        {
            if (!ModelState.IsValid) return BadRequest();
            User user = new User
            {
                Email = um.Email,
                FullName = um.FullName,
                UserName = um.UserName,
                ImageURL = "Photo.jpg"
            };
            var result = await _u.CreateAsync(user, um.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);

                }
                return View();
            }
            /*var RoleResult = await _sign.AddToRoleAsync(user, nameof(Roles.User));
            if (!RoleResult.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);

                }
                return View();
            }*/
            return View();

        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM lm, string ReturnUrl)
        {
            User? user = null;
            if (!ModelState.IsValid) return View();
            if (lm.UsernameOrEmail.Contains('@'))
            {
                user = await _u.FindByNameAsync(lm.UsernameOrEmail);
            }
            else
            {
                user = await _u.FindByEmailAsync(lm.UsernameOrEmail);
            }
            if (user is null)
            {
                ModelState.AddModelError("", "Username or Password is wrong");
                return View();
            }

            var result = await _sign.PasswordSignInAsync(user, lm.Password, lm.RememberMe, true);
            if (result.IsNotAllowed)
            {
                ModelState.AddModelError("", "Username or Password is wrong");
            }
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Wait until" + user.LockoutEnd!.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                return View();

            }
            if (string.IsNullOrEmpty(ReturnUrl))
            {
                return RedirectToAction("Index", "Home");

            }
            return LocalRedirect(ReturnUrl);
        }
        public async Task<IActionResult> LogOut()
        {
            await _sign.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
