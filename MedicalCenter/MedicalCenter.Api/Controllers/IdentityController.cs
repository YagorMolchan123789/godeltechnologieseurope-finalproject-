using System.Text;
using Asp.Versioning;
using FluentValidation;
using MedicalCenter.Api.Models;
using MedicalCenter.Api.Services;
using MedicalCenter.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace MedicalCenter.Api.Controllers
{
    [ApiController]
    [Route("api")]
    [ApiVersion("1.0")]
    public class IdentityController : Controller
    {
        private readonly IValidator<RegisterPatientRequest> _registerPatientRequestValidator;
        private readonly IValidator<LoginRequest> _loginRequestValidator;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserService _userService;

        public IdentityController(
            IValidator<RegisterPatientRequest> registerPatientRequestValidator,
            SignInManager<AppUser> signInManager,
            IValidator<LoginRequest> loginRequestValidator,
            IUserService userService
            )
        {
            _registerPatientRequestValidator = registerPatientRequestValidator ?? throw new ArgumentNullException(nameof(registerPatientRequestValidator));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _loginRequestValidator = loginRequestValidator ?? throw new ArgumentNullException(nameof(loginRequestValidator));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <response code="201">User register success</response>
        /// <response code="400">Wrong request or user with this email already exists</response>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterPatientRequest registration)
        {
            const string RoleName = "patient";

            var validationResult = await _registerPatientRequestValidator.ValidateAsync(registration);

            if (!validationResult.IsValid)
            {
                var sb = new StringBuilder();

                foreach (var failure in validationResult.Errors)
                {
                    sb.Append($"Property {failure.PropertyName} failed validation. Error was: {failure.ErrorMessage} ");
                }

                return BadRequest(sb.ToString());
            }

            return await _userService.AddUserAsync(registration, RoleName);
        }

        /// <summary>
        /// Login existing user
        /// </summary>
        /// <response code="200">User login success</response>
        /// <response code="400">Wrong login data</response>
        /// <response code="401">User is not registered</response>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest login, [FromQuery] bool? useCookies, [FromQuery] bool? useSessionCookies)
        {
            var validationResult = await _loginRequestValidator.ValidateAsync(login);

            if (!validationResult.IsValid)
            {
                var sb = new StringBuilder();

                foreach (var failure in validationResult.Errors)
                {
                    sb.Append($"Property {failure.PropertyName} failed validation. Error was: {failure.ErrorMessage} ");
                }

                return BadRequest(sb.ToString());
            }

            var useCookieScheme = (useCookies == true) || (useSessionCookies == true);
            var isPersistent = (useCookies == true) && (useSessionCookies != true);
            _signInManager.AuthenticationScheme = useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent, lockoutOnFailure: true);

            if (result.RequiresTwoFactor)
            {
                if (!string.IsNullOrEmpty(login.TwoFactorCode))
                {
                    result = await _signInManager.TwoFactorAuthenticatorSignInAsync(login.TwoFactorCode, isPersistent, rememberClient: isPersistent);
                }
                else if (!string.IsNullOrEmpty(login.TwoFactorRecoveryCode))
                {
                    result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(login.TwoFactorRecoveryCode);
                }
            }

            if (!result.Succeeded)
            {
                return Unauthorized(result.ToString());
            }

            // The signInManager already produced the needed response in the form of a cookie or bearer token.
            return Empty;
        }
    }
}
