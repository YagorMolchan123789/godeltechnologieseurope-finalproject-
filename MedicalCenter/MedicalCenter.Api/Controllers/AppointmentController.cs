using MedicalCenter.Business;
using MedicalCenter.Business.Services.Interfaces;
using MedicalCenter.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MedicalCenter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController(
        UserManager<AppUser> userManager,
        IAppointmentService appointmentService
        ) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetByUserIdAsync()
        {
            var user = await userManager.GetUserAsync(User);

            if (user is null)
            {
                return Unauthorized();
            }

            var appointments = appointmentService.GetByUserIdAsync(user.Id);

            return Ok(appointments);
        }

        [HttpDelete]
        [Route("{appointmentId}")]
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateAppointmentModel model)
        {
            var user = await userManager.GetUserAsync(User);

            if (user is null)
            {
                return Unauthorized();
            }

            var doctor = await userManager.FindByIdAsync(model.DoctorId);

            if (doctor is null || !await userManager.IsInRoleAsync(doctor, "doctor"))
            {
                return BadRequest("Doctor not found");
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
