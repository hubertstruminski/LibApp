using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using LibApp.Data;
using LibApp.Dtos;
using LibApp.Models;
using LibApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace LibApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IMembershipTypeRepository _membershipTypeRepository;
        private readonly IAccountService _accountService;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IMembershipTypeRepository membershipTypeRepository,
            IAccountService accountService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _membershipTypeRepository = membershipTypeRepository;
            _accountService = accountService;

            MembershipTypes = _membershipTypeRepository.GetAllMembershipTypes().Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name,
            });
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IEnumerable<SelectListItem> MembershipTypes { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "The name's input is required.")]
            [StringLength(160, ErrorMessage = "The name must have between 2 and 160 characters", MinimumLength = 2)]
            public string Name { get; set; }

            [Required]
            public int MembershipTypeId { get; set; }

            [Min18YearsToSubscribe]
            public DateTime? Birthdate { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    var registerDto = new RegisterUserDto
                    {
                        Email = Input.Email,
                        Name = Input.Name,
                        MembershipTypeId = (byte)Input.MembershipTypeId,
                        Birthdate = Input.Birthdate,
                        Password = Input.Password,
                        ConfirmPassword = Input.ConfirmPassword,
                    };

                    //var user = new IdentityUser
                    //{
                    //    Email = Input.Email,
                    //    UserName = Input.Email,
                    //};

                    //var result = await _userManager.CreateAsync(user, Input.Password);


                    _accountService.RegisterUser(registerDto);
                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                }
                catch
                {
                    return Page();
                }       
            }
            return Page();
        }
    }
}
