using MedicalCenter.Data.Entities;

namespace MedicalCenter.Data.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        Task AddAsync(DoctorInfo doctor);
        Task<List<DoctorInfo>> GetAllAsync();
    }
}
