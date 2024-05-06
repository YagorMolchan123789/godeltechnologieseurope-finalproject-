using MedicalCenter.Data.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MedicalCenter.Data.Entities;
using MedicalCenter.Api.Models.Interfaces;
using MedicalCenter.Api.Models;

namespace MedicalCenter.Api.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<AppUser> _userStore;
        private readonly EmailAddressAttribute _emailAddressAttribute = new();
        private readonly IDoctorRepository _doctorRepository;

        public UserService(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserStore<AppUser> userStore,
            IDoctorRepository doctorRepository
            )
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _userStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
            _doctorRepository = doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository));
        }

        public async Task<IActionResult> AddUserAsync(IRegisterRequest registration, string roleName)
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("Identity system requires a user store with email support.");
            }

            var emailStore = (IUserEmailStore<AppUser>)_userStore;
            var email = registration.Email;

            if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
            {
                return new StatusCodeResult(400);
            }

            var user = new AppUser();
            await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);

            user.FirstName = registration.FirstName;
            user.LastName = registration.LastName;

            await _userStore.UpdateAsync(user, CancellationToken.None);

            var result = await _userManager.CreateAsync(user, registration.Password);

            if (!result.Succeeded)
            {
                return new StatusCodeResult(400);
            }

            var isRoleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!isRoleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            await _userManager.AddToRoleAsync(user, roleName);

            if (roleName == "doctor")
            {
                var doctorRegistration = (RegisterDoctorRequest)registration;

                var doctorInfo = new DoctorInfo()
                {
                    AppUserId = user.Id,
                    PracticeStartDate = doctorRegistration.PracticeStartDate,
                    Specialization = doctorRegistration.Specialization
                };

                await _doctorRepository.AddAsync(doctorInfo);

                return new StatusCodeResult(201);
            }
            else
            {
                return new StatusCodeResult(201);
            }
        }
    }
}
