using MedicalCenter.Data.Repositories.Interfaces;
using MedicalCenter.Domain;
using Microsoft.EntityFrameworkCore;

namespace MedicalCenter.Data.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DoctorRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<DoctorInfo>> GetAllAsync()
        {
            return await _dbContext.Set<DoctorInfo>().ToListAsync();
        }

        public async Task AddAsync(DoctorInfo doctor)
        {
            _dbContext.Set<DoctorInfo>().Add(doctor);
            await _dbContext.SaveChangesAsync();
        }
    }
}
