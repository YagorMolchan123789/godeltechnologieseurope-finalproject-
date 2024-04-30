using MedicalCenter.Domain;

namespace MedicalCenter.Business.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<List<DoctorInfo>> GetAllAsync();
    }
}
