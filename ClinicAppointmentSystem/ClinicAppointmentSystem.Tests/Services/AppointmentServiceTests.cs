using ClinicAppointmentSystem.Repositories;
using ClinicAppointmentSystem.Repository;
using ClinicAppointmentSystem.Services;
using ClinicAppointmentSystem.Tests.TestHelpers;
using Xunit;

namespace ClinicAppointmentSystem.Tests.Services;

public class AppointmentServiceTests
{
    [Fact]
    public async Task BookAppointment_ShouldFail_WhenDateIsPast()
    {
        var (ctx, conn) = DbFactory.Create();

        try
        {
            var service = new AppointmentService(
                new PatientRepository(ctx),
                new DoctorRepository(ctx),
                new AppointmentRepository(ctx));

            var result = await service.BookAsync(1, 1, DateTime.Now.AddMinutes(-10));

            Assert.False(result.ok);
        }
        finally
        {
            conn.Dispose();
        }
    }

    [Fact]
    public async Task CancelAppointment_ShouldSetStatusCancelled()
    {
        var (ctx, conn) = DbFactory.Create();

        try
        {
            ctx.Patients.Add(new ClinicAppointmentSystem.Models.Patient
            {
                PatientNumber = "P001",
                FirstName = "A",
                LastName = "B"
            });

            ctx.Doctors.Add(new ClinicAppointmentSystem.Models.Doctor
            {
                LicenseNumber = "LIC001",
                FirstName = "Doc",
                LastName = "One",
                Email = "doc@test.com"
            });

            await ctx.SaveChangesAsync();

            var service = new AppointmentService(
                new PatientRepository(ctx),
                new DoctorRepository(ctx),
                new AppointmentRepository(ctx));

            var booked = await service.BookAsync(1, 1, DateTime.Now.AddDays(1));

            Assert.True(booked.ok);

            var cancel = await service.CancelAsync(booked.appointment!.AppointmentId);

            Assert.True(cancel.ok);

            var updated = await ctx.Appointments.FindAsync(booked.appointment.AppointmentId);

            Assert.Equal("Cancelled", updated!.Status);
        }
        finally
        {
            conn.Dispose();
        }
    }
}