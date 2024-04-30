using MedicalCenter.Data.Entities;
﻿using System.Reflection;
using MedicalCenter.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedicalCenter.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<DoctorInfo> DoctorInfos { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<TimeSlot> TimeSlots { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        private List<TimeSlot> GenerateTimeSlots()
        {
            const int timeSlotsCount = 8;
            const int timeMinutesInterval = 60;
            var startTime = new TimeOnly(8, 0);

            var timeSlots = new List<TimeSlot>();
            for (int i = 0; i < timeSlotsCount; i++)
            {
                var slotStartTime = startTime.AddMinutes(i * timeMinutesInterval);
                timeSlots.Add(new TimeSlot
                {
                    Id = i + 1,
                    StartTime = slotStartTime,
                    EndTime = slotStartTime.AddMinutes(timeMinutesInterval)
                });
            }
            return timeSlots;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<TimeSlot>().HasData(GenerateTimeSlots());

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
