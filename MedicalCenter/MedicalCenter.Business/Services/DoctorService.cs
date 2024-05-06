using MedicalCenter.Business.Services.Interfaces;
using MedicalCenter.Data.Entities;
using MedicalCenter.Data.Repositories.Interfaces;

namespace MedicalCenter.Business.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<List<DoctorInfo>> GetAllAsync()
        {
            return await _doctorRepository.GetAllAsync();
        }
    }
}
