using MedicalCenter.Business.Services.Interfaces;
using MedicalCenter.Data.Repositories.Interfaces;
using MedicalCenter.Domain;

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
