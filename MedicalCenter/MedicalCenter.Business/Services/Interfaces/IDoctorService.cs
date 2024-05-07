using MedicalCenter.Data.Entities;

namespace MedicalCenter.Business.Services.Interfaces
{
    public interface IDoctorService
    {
        Task DeleteAsync(AppUser user);
        Task<List<DoctorInfo>> GetAllAsync();
    }
}
