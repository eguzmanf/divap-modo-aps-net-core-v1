using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using DotacionWEBCore.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Vereyon.Web;
using Microsoft.AspNetCore.Http;

namespace DotacionWEBCore.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<DotacionWEBCoreUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly ISession _session;
        private readonly IFlashMessage _flashMessage;


        public LogoutModel(SignInManager<DotacionWEBCoreUser> signInManager, ILogger<LogoutModel> logger, IHttpContextAccessor httpContextAccessor, IFlashMessage flashMessage)
        {
            _signInManager = signInManager;
            _logger = logger;
            _session = httpContextAccessor.HttpContext.Session;
            _flashMessage = flashMessage;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            _flashMessage.Warning("Has cerrado sesión correctamente!");
            HttpContext.Session.Remove("Role1");

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return Page();
            }
        }
    }
}