using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem.Services.Interfaces;

public interface IDoctorService
{
    Task<(bool ok, string message, Doctor? doctor)> CreateAsync(Doctor doctor);
    Task<(bool ok, string message)> DeleteAsync(int doctorId);
}