using ClinicAppointmentSystem.Models;
using ClinicAppointmentSystem.Repositories.Interfaces;
using ClinicAppointmentSystem.Services.Interfaces;

namespace ClinicAppointmentSystem.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctors;
    private readonly IAppointmentRepository _appointments;

    public DoctorService(IDoctorRepository doctors, IAppointmentRepository appointments)
    {
        _doctors = doctors;
        _appointments = appointments;
    }

    public async Task<(bool ok, string message, Doctor? doctor)> CreateAsync(Doctor doctor)
    {
        if (string.IsNullOrWhiteSpace(doctor.LicenseNumber))
            return (false, "LicenseNumber is required.", null);
        if (string.IsNullOrWhiteSpace(doctor.Email))
            return (false, "Email is required.", null);

        if (await _doctors.LicenseNumberExistsAsync(doctor.LicenseNumber))
            return (false, "LicenseNumber already exists.", null);

        if (await _doctors.EmailExistsAsync(doctor.Email))
            return (false, "Email already exists.", null);

        var saved = await _doctors.AddAsync(doctor);
        return (true, "Doctor created.", saved);
    }

    public async Task<(bool ok, string message)> DeleteAsync(int doctorId)
    {
        if (!await _doctors.ExistsAsync(doctorId))
            return (false, "Doctor not found.");

        if (await _appointments.AnyForDoctorAsync(doctorId))
            return (false, "Cannot delete doctor with existing appointments.");

        await _doctors.DeleteAsync(doctorId);
        return (true, "Doctor deleted.");
    }
}