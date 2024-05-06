using MedicalCenter.Data.Entities;

namespace MedicalCenter.Business.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<List<DoctorInfo>> GetAllAsync();
    }
}
