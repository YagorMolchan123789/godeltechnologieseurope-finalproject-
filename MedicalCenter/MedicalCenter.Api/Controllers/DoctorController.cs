using FluentValidation;
using System.Text;
using MedicalCenter.Api.Helpers.Mappers;
using MedicalCenter.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MedicalCenter.Data.Entities;
using MedicalCenter.Api.Models;
using Asp.Versioning;
using MedicalCenter.Api.Services;

namespace MedicalCenter.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly DoctorInfoDtoMapper _doctorInfoDtoMapper;
        private readonly UserManager<AppUser> _userManager;

        private readonly IValidator<RegisterDoctorRequest> _registerDoctorRequestValidator;
        private readonly IUserService _userService;
        private const int UserIdMaxLength = 450;

        public DoctorController(
            IDoctorService doctorService,
            DoctorInfoDtoMapper doctorInfoDtoMapper,
            UserManager<AppUser> userManager,
            IValidator<RegisterDoctorRequest> registerDoctorRequestValidator,
            IUserService userService
            )
        {
            _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
            _doctorInfoDtoMapper = doctorInfoDtoMapper ?? throw new ArgumentNullException(nameof(doctorInfoDtoMapper));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _registerDoctorRequestValidator = registerDoctorRequestValidator ?? throw new ArgumentNullException(nameof(registerDoctorRequestValidator));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        /// <summary>
        /// Get list of avaliable doctors
        /// </summary>
        /// <response code="200">Get a list of available doctors</response>
        /// <response code="401">User is not registered</response>
        [HttpGet]
        [Route("doctors")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllDoctorsAsync()
        {
            var doctors = await _doctorService.GetAllAsync();
            var doctorDtos = doctors.Select(x => _doctorInfoDtoMapper.MapFromDomainAsync(x).Result).ToList();

            var user = await _userManager.GetUserAsync(User);

            if (user is null)
            {
                return Unauthorized();
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "admin");

            var result = new DoctorsDto
            {
                DoctorInfos = doctorDtos,
                IsShowButton = isAdmin
            };

            return Ok(result);
        }

        /// <summary>
        /// Delete doctor by Id (only for admin)
        /// </summary>
        /// <response code="204">Doctor was successfully deleted</response>
        /// <response code="400">Wrong doctor Id</response>
        /// <response code="401">User no login</response>
        /// <response code="403">User is no admin</response>
        [HttpDelete]
        [Route("{doctorId}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteAsync(string doctorId)
        {
            var user = await _userManager.FindByIdAsync(doctorId);

            if (user == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(doctorId) || doctorId.Length > UserIdMaxLength)
            {
                return BadRequest();
            }

            await _userManager.DeleteAsync(user);

            return NoContent();
        }

        /// <summary>
        /// Create doctor (only for admin)
        /// </summary>
        /// <response code="201">Doctor was successfully created</response>
        /// <response code="400">Wrong request or doctor with this email already exists</response>
        /// <response code="401">User no login</response>
        /// <response code="403">User is no admin</response>
        [HttpPost]
        [Route("doctors")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateAsync([FromBody] RegisterDoctorRequest registration)
        {
            const string RoleName = "doctor";

            var validationResult = await _registerDoctorRequestValidator.ValidateAsync(registration);

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
    }
}
