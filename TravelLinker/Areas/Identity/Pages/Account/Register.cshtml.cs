// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using TravelLinker.Helpers;
using TravelLinkerDataAccess.Services;
using TravelLinkerModels.Models;

namespace TravelLinker.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webHost;
        private readonly IHotelService _hotelService;
        private readonly ICompanyService _companyService; 
    

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender ,
            IWebHostEnvironment webHost, IHotelService hotelService , ICompanyService companyService)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _webHost = webHost;
            _hotelService = hotelService;       
            _companyService = companyService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "Enter Your Enterprise  Email  ")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required (ErrorMessage ="Enter Your Password ")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
         
            public string ConfirmPassword { get; set; }
            /// <summary>
            /// This Filde Created By Me 
            /// </summary>
            [MaxLength(60)]
            [Required(ErrorMessage = "Enter  Enterprise Name  ")]
            public string EnterpriseName { get; set; }
            [Phone]
            [MinLength(11)]
            [MaxLength(11)]
            [Display(Name = "Phone Number")]
            [Required (ErrorMessage = "Enter  Enterprise Phone Number")] 
            public string PhoneNumber { get; set; }
            [Required(ErrorMessage = "Select  Enterprise City")]
            public string City { get; set; }        

            [Required(ErrorMessage = "Select  Enterprise Type")]
            [Display(Name = "Enterprise Type")]
            public string EnterpriseType { get; set; }

            // Fill IT Using Helper Class 
            public IEnumerable<SelectListItem> TypesOfEnterprise { get; set; }

            public IEnumerable<SelectListItem> Citys { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            Input = new InputModel
            {
                TypesOfEnterprise = StaticData.LoadEnterprises() ,
                Citys = StaticData.LoadCitiesInEgypt() 
            }; 
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {

                //Check if The Phone Is existing 
                var CheckPhone = _userManager
                    .Users.FirstOrDefault(x => x.PhoneNumber == Input.PhoneNumber);
                if (CheckPhone != null)
                {
                    ModelState.AddModelError("", $"This {Input.PhoneNumber} Is Token Before");
                    Input.TypesOfEnterprise = StaticData.LoadEnterprises();
                    Input.Citys = StaticData.LoadCitiesInEgypt();  
                    return Page();
                }

                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                
                user.EnterpriseName = Input.EnterpriseName;
                user.PhoneNumber = Input.PhoneNumber;
                user.City = Input.City;
             //    user.ProfileImageUrl = ""; // Because We Can't Insert Null Value 
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    // after user Is Created Succeeded We Will Add Claim and image for Him  
                  
                     
                    // Add Claim (UserType) > Hotel , Transport Company  may Have (Admin)
                    Claim UserType = new Claim(StaticData.UserTypeClaim, Input.EnterpriseType);
                    await _userManager.AddClaimAsync(user, UserType);
                
                    //-- <-01005415303->--

                    // Creatr Row In Hotel Or Comapny Table 
                    if (UserType.Value == StaticData.HotelType)
                    {
                        _hotelService.Create(user.Id); 
                    }
                    else if (UserType.Value == StaticData.CompanyType)
                    {
                        _companyService.Create(user.Id);
                    }

                 

                    //--- >> ---------------------------------------------------
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            Input.TypesOfEnterprise = StaticData.LoadEnterprises();

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
