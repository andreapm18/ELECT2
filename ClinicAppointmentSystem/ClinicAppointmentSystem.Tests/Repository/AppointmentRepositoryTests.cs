using ClinicAppointmentSystem.Models;
using ClinicAppointmentSystem.Repositories;
using ClinicAppointmentSystem.Repository;
using ClinicAppointmentSystem.Tests.TestHelpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClinicAppointmentSystem.Tests.Repositories;

public class AppointmentRepositoryTests
{
    [Fact]
    public async Task DoctorCannotDoubleBook_SameDateTime()
    {
        var (ctx, conn) = DbFactory.Create();

        try
        {
            ctx.Patients.Add(new Patient { PatientNumber = "P001", FirstName = "A", LastName = "B" });
            ctx.Patients.Add(new Patient { PatientNumber = "P002", FirstName = "C", LastName = "D" });

            ctx.Doctors.Add(new Doctor
            {
                LicenseNumber = "LIC001",
                FirstName = "Doc",
                LastName = "One",
                Email = "doc@test.com"
            });

            await ctx.SaveChangesAsync();

            var repo = new AppointmentRepository(ctx);

            var when = DateTime.Now.AddDays(1);

            await repo.AddAsync(new Appointment
            {
                PatientId = 1,
                DoctorId = 1,
                AppointmentDateTime = when,
                Status = "Booked"
            });

            ctx.Appointments.Add(new Appointment
            {
                PatientId = 2,
                DoctorId = 1,
                AppointmentDateTime = when,
                Status = "Booked"
            });

            await Assert.ThrowsAsync<DbUpdateException>(() => ctx.SaveChangesAsync());
        }
        finally
        {
            conn.Dispose();
        }
    }
}