using MedicalCenter.Business;
using MedicalCenter.Business.Interfaces;
using MedicalCenter.Domain;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateAppointmentModel model)
        {
            var user = await userManager.GetUserAsync(User);

            //if (user is null)
            //{
            //    return Unauthorized();
            //}

            var doctor = await userManager.FindByIdAsync(model.DoctorId);

            if (doctor is null || !await userManager.IsInRoleAsync(doctor, "doctor"))
            {
                return BadRequest("Doctor not found");
            }

            try
            {
                await appointmentService.CreateAppointmentAsync(model.DoctorId, model);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            return Created("/user/appointments", model);

        }
    }
}
