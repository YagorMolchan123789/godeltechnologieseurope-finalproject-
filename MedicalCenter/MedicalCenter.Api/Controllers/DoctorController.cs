using MedicalCenter.Api.Helpers.Mappers;
using MedicalCenter.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalCenter.Api.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly DoctorInfoDtoMapper _doctorInfoDtoMapper;

        public DoctorController(IDoctorService doctorService, DoctorInfoDtoMapper doctorInfoDtoMapper)
        {
            _doctorService = doctorService;
            _doctorInfoDtoMapper = doctorInfoDtoMapper;
        }

        [HttpGet]
        [Route("doctors")]
        public async Task<IActionResult> GetAllDoctorsAsync()
        {
            var doctors = (await _doctorService.GetAllAsync());
            var doctorDtos = doctors.Select(x => _doctorInfoDtoMapper.MapFromDomainAsync(x).Result).ToList();
            return Ok(doctorDtos);
        }
    }
}
