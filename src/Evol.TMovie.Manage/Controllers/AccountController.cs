using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Evol.TMovie.Manage.Models.AccountViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Evol.TMovie.Manage.Models.Identity;
using Evol.TMovie.Domain.QueryEntries;
using Evol.Util.Hash;
using Evol.TMovie.Domain.Services;

namespace Evol.TMovie.Manage.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger _logger;

        private readonly SignInManager<AppUser> _signInManager;

        private readonly IEmployeeQueryEntry _employeeQueryEntry;

        private readonly IEmployeePermissionService _employeePermissionService;

        public AccountController(IEmployeeQueryEntry employeeQueryEntry, IEmployeePermissionService employeePermissionService, SignInManager<AppUser> signInManager, ILoggerFactory loggerFactory)
        {
            _employeeQueryEntry = employeeQueryEntry;
            _employeePermissionService = employeePermissionService;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [HttpGet]
        [AllowAnonymous]
        // GET: Account
        public IActionResult Login(string returnUrl = null)
        {

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // GET: Account/Details/5
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true

                var employee = await _employeeQueryEntry.FindByUsernameAsync(model.Username);
                var inputPasswordHash = HashUtil.Md5PasswordWithSalt(model.Password, employee.Salt);
                var ePermissionDto = await _employeePermissionService.GetAsync(employee.Id);
                var roleCodes = ePermissionDto.Roles.Select(e => e.Value).ToArray();
                var appUser = new AppUser { Id = employee.Id, Username = model.Username, RealName = employee.RealName, Roles = roleCodes };

                var result = await _signInManager.PasswordSignInAsync(appUser, inputPasswordHash, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}