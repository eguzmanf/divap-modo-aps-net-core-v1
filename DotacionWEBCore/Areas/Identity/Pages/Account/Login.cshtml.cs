using DotacionWEBCore.Areas.Identity.Data;
using DotacionWEBCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Vereyon.Web;

namespace DotacionWEBCore.Areas.Identity.Pages.Account
{

    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<DotacionWEBCoreUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly DatabaseContext _context;
        private readonly IFlashMessage _flashMessage;

        public LoginModel(SignInManager<DotacionWEBCoreUser> signInManager, ILogger<LoginModel> logger, DatabaseContext context, IFlashMessage flashMessage)
        {
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _flashMessage = flashMessage;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {

            [Required(ErrorMessage ="El Run es requerido.")]
            [Display(Name = "Run")]
            public string UserName { get; set; }


            [Required(ErrorMessage = "La Contraseña es requerida.")]
            [Display(Name = "Contraseña")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Recordar credenciales")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == Input.UserName).Select(b => b.ID_Perfil).First();
                    HttpContext.Session.SetString("Role1", Perfil_U.ToString());
                    _flashMessage.Confirmation("Has iniciado sesión correctamente!");

                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {   //
                    _logger.LogWarning("Cuenta de Usuario bloqueada.");
                    return RedirectToPage("./Lockout");

                    // _flashMessage.Danger("Esta cuenta ha sido bloqueada, por favor comuníquese con el Administrador del Sistema!");
                    // return Page();

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Intento fallido.");
                    _flashMessage.Danger("Rut o Contraseña incorrectos!");
                    return Page();
                }
            }


            // If we got this far, something failed, redisplay form
            return Page();
        }

    }
}
