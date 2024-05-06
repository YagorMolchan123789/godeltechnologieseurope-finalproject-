using MedicalCenter.Api.Models;
using MedicalCenter.Business.Services.Interfaces;
using MedicalCenter.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MedicalCenter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TimeSlotController(
        UserManager<AppUser> userManager,
        ITimeSlotService timeSlotService) : ControllerBase
    {
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable(string doctorId, DateOnly date)
        {
            try
            {
                var doctor = await userManager.FindByIdAsync(doctorId);

                if (doctor is null || !await userManager.IsInRoleAsync(doctor, "doctor"))
                {
                    return BadRequest("Doctor not found");
                }

                var timeSlots = await timeSlotService.GetAvailableTimeSlotsAsync(doctorId, date);

                var result = new GetAvailableTimeSlotsResponse()
                {
                    timeSlots = timeSlots,
                    CurrentTime = TimeOnly.FromDateTime(DateTime.Now)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
