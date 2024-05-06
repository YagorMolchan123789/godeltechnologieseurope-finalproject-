using MedicalCenter.Api.Models;
using MedicalCenter.Business.Services.Interfaces;
using MedicalCenter.Data.Entities;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MedicalCenter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    public class TimeSlotController(
        UserManager<AppUser> userManager,
        ITimeSlotService timeSlotService) : ControllerBase
    {
        /// <summary>
        /// Get available timeslots list
        /// </summary>
        /// <response code="200">Get a list of available timeslots</response>
        /// <response code="404">The doctor does not exist</response>
        /// <response code="500">Error while getting a list</response>
        [HttpGet("available")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAvailable(string doctorId, DateOnly date)
        {
            try
            {
                var doctor = await userManager.FindByIdAsync(doctorId);

                if (doctor is null || !await userManager.IsInRoleAsync(doctor, "doctor"))
                {
                    return NotFound("Doctor not found");
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
