using MedicalCenter.Api.Helpers.Mappers;
using MedicalCenter.Api.Models;
using MedicalCenter.Business.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedicalCenter.Data.Entities;

namespace MedicalCenter.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly DoctorInfoDtoMapper _doctorInfoDtoMapper;
        private readonly UserManager<AppUser> _userManager;

        const int UserIdMaxLength = 450;

        public DoctorController(IDoctorService doctorService,
            DoctorInfoDtoMapper doctorInfoDtoMapper,
            UserManager<AppUser> userManager)
        {
            _doctorService = doctorService;
            _doctorInfoDtoMapper = doctorInfoDtoMapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDoctorsAsync()
        {
            var doctors = (await _doctorService.GetAllAsync());

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

        [HttpDelete]
        [Route("{doctorId}")]
        [Authorize(Roles = "admin")]
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

            return Ok();
        }
    }
}
