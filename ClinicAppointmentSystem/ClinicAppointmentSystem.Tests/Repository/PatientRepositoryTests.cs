using ClinicAppointmentSystem.Models;
using ClinicAppointmentSystem.Repositories;
using ClinicAppointmentSystem.Repository;
using ClinicAppointmentSystem.Tests.TestHelpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClinicAppointmentSystem.Tests.Repositories;

public class PatientRepositoryTests
{
    [Fact]
    public async Task AddPatient_ShouldInsertRecord()
    {
        var (ctx, conn) = DbFactory.Create();

        try
        {
            var repo = new PatientRepository(ctx);

            var patient = new Patient
            {
                PatientNumber = "P001",
                FirstName = "John",
                LastName = "Doe",
                Email = "john@test.com"
            };

            await repo.AddAsync(patient);

            Assert.True(patient.PatientId > 0);
            Assert.Equal(1, await ctx.Patients.CountAsync());
        }
        finally
        {
            conn.Dispose();
        }
    }
}