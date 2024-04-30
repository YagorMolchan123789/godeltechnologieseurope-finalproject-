using MedicalCenter.Data.Repositories.Interfaces;
using MedicalCenter.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalCenter.Data
{
    public static class DoctorSeedData
    {
        public static async Task CreateDoctorsAccountAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var scopedProvider = scope.ServiceProvider;

            var userManager = scopedProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scopedProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var _doctorRepository = scopedProvider.GetRequiredService<IDoctorRepository>();

            var role = "doctor";
            var password = "Doctor123!";
            var email = "doctor1@gmail.com";

            if (await userManager.FindByEmailAsync(email) != null)
            {
                return;
            }

            if (await roleManager.FindByNameAsync(role) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            var users = GetUsers();

            foreach (var user in users)
            {
                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }

            var doctors = GetDoctorsInfo();

            foreach (var doctor in doctors)
            {
                await _doctorRepository.AddAsync(doctor);
            }
        }

        private static List<AppUser> GetUsers()
        {
            return new List<AppUser>()
            {
                new AppUser
                {
                    Id = "1",
                    Email = "doctor1@gmail.com",
                    UserName = "doctor1@gmail.com",
                    FirstName = "Yelena",
                    LastName = "Snezhnaya"
                },
                new AppUser
                {
                    Id = "2",
                    Email = "doctor2@gmail.com",
                    UserName = "doctor2@gmail.com",
                    FirstName = "Emily",
                    LastName = "Carter"
                },
                new AppUser
                {
                    Id = "3",
                    Email = "doctor3@gmail.com",
                    UserName = "doctor3@gmail.com",
                    FirstName = "Nathan",
                    LastName = "Reynolds"
                },
                new AppUser
                {
                    Id = "4",
                    Email = "doctor4@gmail.com",
                    UserName = "doctor4@gmail.com",
                    FirstName = "Lisa",
                    LastName = "Morgan"
                },
                new AppUser
                {
                    Id = "5",
                    Email = "doctor5@gmail.com",
                    UserName = "doctor5@gmail.com",
                    FirstName = "Amelia",
                    LastName = "Warren"
                },
                new AppUser
                {
                    Id = "6",
                    Email = "doctor6@gmail.com",
                    UserName = "doctor6@gmail.com",
                    FirstName = "Michael",
                    LastName = "Johnson"
                },
                new AppUser
                {
                    Id = "7",
                    Email = "doctor7@gmail.com",
                    UserName = "doctor7@gmail.com",
                    FirstName = "Sarah",
                    LastName = "White"
                },
                new AppUser
                {
                    Id = "8",
                    Email = "doctor8@gmail.com",
                    UserName = "doctor8@gmail.com",
                    FirstName = "David",
                    LastName = "Parker"
                },
                new AppUser
                {
                    Id = "9",
                    Email = "doctor9@gmail.com",
                    UserName = "doctor9@gmail.com",
                    FirstName = "Olivia",
                    LastName = "Turner"
                },
                new AppUser
                {
                    Id = "10",
                    Email = "doctor10@gmail.com",
                    UserName = "doctor10@gmail.com",
                    FirstName = "James",
                    LastName = "Brooks"
                },
            };
        }

        private static List<DoctorInfo> GetDoctorsInfo()
        {
            return new List<DoctorInfo>
            {
                new DoctorInfo
                {
                    AppUserId = "1",
                    Specialization = "General Practice",
                    PracticeStartDate = 2000,
                    PhotoUrl = "Doctor_1.png"
                },
                new DoctorInfo
                {
                    AppUserId = "2",
                    Specialization = "General Practice",
                    PracticeStartDate = 2008,
                    PhotoUrl = "Doctor_2.png"
                },
                new DoctorInfo
                {
                    AppUserId = "3",
                    Specialization = "General Practice",
                    PracticeStartDate = 1996,
                    PhotoUrl = "Doctor_3.png"
                },
                new DoctorInfo
                {
                    AppUserId = "4",
                    Specialization = "General Practice",
                    PracticeStartDate = 2015,
                    PhotoUrl = "Doctor_4.png"

                },
                new DoctorInfo
                {
                    AppUserId = "5",
                    Specialization = "Pediatrics",
                    PracticeStartDate = 1999,
                    PhotoUrl = "Doctor_5.png"
                },
                new DoctorInfo
                {
                    AppUserId = "6",
                    Specialization = "Pediatrics",
                    PracticeStartDate = 2022,
                    PhotoUrl = "Doctor_6.png"
                },
                new DoctorInfo
                {
                    AppUserId = "7",
                    Specialization = "Pediatrics",
                    PracticeStartDate = 1990,
                    PhotoUrl = "Doctor_7.png"
                },
                new DoctorInfo
                {
                    AppUserId = "8",
                    Specialization = "Dermatology",
                    PracticeStartDate = 2015,
                    PhotoUrl = "Doctor_8.png"
                },
                new DoctorInfo
                {
                    AppUserId = "9",
                    Specialization = "Orthopedics",
                    PracticeStartDate = 2007,
                    PhotoUrl = "Doctor_9.png"
                },
                new DoctorInfo
                {
                    AppUserId = "10",
                    Specialization = "Dentist",
                    PracticeStartDate = 2017,
                    PhotoUrl = "Doctor_10.png"
                },
            };
        }
    }
}
