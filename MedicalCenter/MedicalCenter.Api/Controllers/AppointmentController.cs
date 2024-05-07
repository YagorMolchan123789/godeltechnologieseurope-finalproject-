using MedicalCenter.Business;
using MedicalCenter.Business.Services.Interfaces;
using MedicalCenter.Data.Entities;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MedicalCenter.Api.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AppointmentController(
        UserManager<AppUser> userManager,
        IAppointmentService appointmentService
        ) : ControllerBase
    {
        /// <summary>
        /// Get users appointments by userId 
        /// </summary>
        /// <response code="200">Appointments was successfully found for current user</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Error while appointment creating</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync()
        {
            var user = await userManager.GetUserAsync(User);

            if (user is null)
            {
                return Unauthorized();
            }

            var appointments = await appointmentService.GetUserAppointmentsAsync(user.Id);

            return Ok(appointments);
        }
        /// <summary>
        /// Remove appointment by Id
        /// </summary>
        /// <param name="appointmentId">appointment Id</param>
        /// <returns>Status code</returns>
        [HttpDelete("{appointmentId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int appointmentId)
        {
            var user = await userManager.GetUserAsync(User);

            if (user is null)
            {
                return Unauthorized();
            }

            if (appointmentId <= 0)
            {
                return BadRequest();
            }

            await appointmentService.DeleteAsync(user.Id, appointmentId);

            return Ok();
        }

        /// <summary>
        /// Create appointment
        /// </summary>
        /// <response code="201">Appointment was successfully created</response>
        /// <response code="404">The user or doctor does not exist</response>
        /// <response code="500">Error while appointment creating</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync([FromBody] CreateAppointmentModel model)
        {
            var user = await userManager.GetUserAsync(User);

            if (user is null)
            {
                return Unauthorized();
            }

            var doctor = await userManager.FindByIdAsync(model.DoctorId);

            if (doctor is null || !await userManager.IsInRoleAsync(doctor, "doctor"))
            {
                return NotFound("Doctor not found");
            }

            try
            {
                await appointmentService.CreateAppointmentAsync(user.Id, model);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            return Created("/user/appointments", model);
        }
    }
}
