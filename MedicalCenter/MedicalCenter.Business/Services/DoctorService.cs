using MedicalCenter.Business.Services.Interfaces;
using MedicalCenter.Data.Entities;
using MedicalCenter.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MedicalCenter.Business.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        private readonly IAppointmentRepository _appointmentRepository;

        private readonly UserManager<AppUser> _userManager;

        public DoctorService(
            IDoctorRepository doctorRepository,
            IAppointmentRepository appointmentRepository,
            UserManager<AppUser> userManager)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository= appointmentRepository;
            _userManager = userManager;
        }

        public async Task<List<DoctorInfo>> GetAllAsync()
        {
            return await _doctorRepository.GetAllAsync();
        }

        public async Task DeleteAsync(AppUser user)
        {
            await _appointmentRepository.DeleteByDoctorIdAsync(user.Id);
            await _userManager.DeleteAsync(user);
        }
    }
}
