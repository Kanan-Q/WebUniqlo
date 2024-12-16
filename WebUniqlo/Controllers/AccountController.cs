using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Authorization;
using WebUniqlo.Services.Abstrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using WebUniqlo.ViewModel.User;
using WebUniqlo.Models;
using WebUniqlo.Helper;
using WebUniqlo.Enums;
using System.Net.Mail;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using WebUniqlo.Services.Interfaces;

namespace WebUniqlo.Controllers
{
    public class AccountController(UserManager<User> _u, SignInManager<User> _sign, IOptions<smtpOptions> _opt, IEmailService _service) : Controller
    {
        smtpOptions _smtp = _opt.Value;
        bool isAuthenticated => User.Identity?.IsAuthenticated ?? false;
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (isAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserCreateVM um)
        {
            if (isAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
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
            var RoleResult = await _u.AddToRoleAsync(user, nameof(Roles.User));
            if (!RoleResult.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);

                }
                return View();
            }
            string token = await _u.GenerateEmailConfirmationTokenAsync(user);
            _service.SendEmailConfirmation(user.Email, user.UserName, token);
            return Content("Email send!");

        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM lm, string? ReturnUrl = null)
        {
            if (isAuthenticated) return RedirectToAction("Index", "Home");
            User? user = null;
            if (!ModelState.IsValid) return View();
            if (lm.UsernameOrEmail.Contains('@'))
            {
                user = await _u.FindByEmailAsync(lm.UsernameOrEmail);
            }
            else
            {
                user = await _u.FindByNameAsync(lm.UsernameOrEmail);
            }
            if (user is null)
            {
                ModelState.AddModelError("", "Username or Password is wrong");
                return View();
            }

            var result = await _sign.PasswordSignInAsync(user, lm.Password, lm.RememberMe, true);
            if (!result.Succeeded)
            {
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "vcvv");
                }
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Wait until" + user.LockoutEnd!.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }
            if (string.IsNullOrEmpty(ReturnUrl))
            {
                if (await _u.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("Index", new { Controller = "Product", Area = "Admin" });
                }
                return RedirectToAction("Index", "Home");

            }
            return LocalRedirect(ReturnUrl);
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _sign.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //public async Task<IActionResult> Email()
        //{
        //    SmtpClient s = new();
        //    s.Port = 587;
        //    s.Host = "smtp.@gmail.com";
        //    s.EnableSsl = true;
        //    s.Credentials = new NetworkCredential("kananag-bp215@code.edu.az", "ddedwddw");
        //    MailAddress from = new MailAddress("kananag-bp215@code.edu.az", "Admin");
        //    MailAddress to = new MailAddress("kananqurbanov244@gmail.com");
        //    MailMessage msg = new MailMessage(from, to);
        //    msg.Subject = "Security alert!";
        //    msg.Body = "<p>Change your password immediately!</p>";
        //    msg.IsBodyHtml = true;
        //    s.Send(msg);
        //    return Ok();
        //}
        //SmtpClient s = new();
        //s.Port = smtp.Port;
        //s.Host = smtp.Host;
        //s.EnableSsl = true;
        //s.Credentials = new NetworkCredential(smtp.Username, smtp.Password);
        //MailAddress from = new MailAddress(smtp.Username, "Admin");
        //MailAddress to = new MailAddress("turan-bp215@code.edu.az");
        //MailMessage msg = new MailMessage(from, to);
        //msg.Subject = "Security alert!";
        //msg.Body = "<p>Change your password immediately!</p>";
        //msg.IsBodyHtml = true;
        //s.Send(msg);
        //_service.SendAsync().Wait();
        //return Ok();
        //}
        public async Task<IActionResult> VerifyEmail(string token, string user)
        {
            var entity = await _u.FindByNameAsync(user);
            if (entity is null) return BadRequest();
            var result = await _u.ConfirmEmailAsync(entity, token.Replace(' ', '+'));
            if (!result.Succeeded)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var item in result.Errors)
                {
                    sb.AppendLine(item.Description);
                }
                return Content(sb.ToString());
            }
            await _sign.SignInAsync(entity, true);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ForgotPassword()
        {
            if (isAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM vm)
        {
            string email = vm.Email;
            if (email is null)
            {
                ModelState.AddModelError("", "Email is required");
                return View();
            }
            User? user = null;
            if (vm.Email.Contains('@'))
            {
                user = await _u.FindByEmailAsync(vm.Email);
            }
            else
            {
                user = await _u.FindByNameAsync(vm.Email);
            }
            var token = await _u.GeneratePasswordResetTokenAsync(user);
            _service.SendEmailConfirmation(user.Email, user.UserName, token);
            return Content("Link sent your email");
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            if (email is null) return BadRequest();
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Index", "Home");
            }
                                                                            
            var model = new ResetPasswordVM { Token = token, Email = email };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM rm)
        {
            if (!ModelState.IsValid) return View(rm);

            var user = await _u.FindByEmailAsync(rm.Email);
            if (user is null)
            {
                ModelState.AddModelError("", "Invalid email.");
                return View();
            }

            var result = await _u.ResetPasswordAsync(user, rm.Token, rm.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(rm);
            }

            return RedirectToAction(nameof(Login));
        }
    }
}

