using MedicalCenter.Api.Models;
using MedicalCenter.Domain;
using Microsoft.AspNetCore.Identity;

namespace MedicalCenter.Api.Helpers.Mappers
{
    public class DoctorInfoDtoMapper
    {
        private readonly UserManager<AppUser> _userManager;

        public DoctorInfoDtoMapper(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<DoctorInfoDto> MapFromDomainAsync(DoctorInfo doctorInfo)
        {
            var firstName = (await _userManager.FindByIdAsync(doctorInfo.AppUserId))?.FirstName;
            var lastName = (await _userManager.FindByIdAsync(doctorInfo.AppUserId))?.LastName;

            return new DoctorInfoDto
            {
                AppUserId = doctorInfo.AppUserId,
                FirstName = firstName ?? string.Empty,
                LastName = lastName ?? string.Empty,
                PracticeStartDate = doctorInfo.PracticeStartDate,
                Specialization = doctorInfo.Specialization,
                PhotoUrl = doctorInfo.PhotoUrl
            };
        }

        public DoctorInfo MapToDomain(DoctorInfoDto doctorInfoDto)
        {
            return new DoctorInfo
            {
                AppUserId = doctorInfoDto.AppUserId,
                PracticeStartDate = doctorInfoDto.PracticeStartDate,
                Specialization = doctorInfoDto.Specialization,
                PhotoUrl = doctorInfoDto.PhotoUrl
            };
        }
    }
}
