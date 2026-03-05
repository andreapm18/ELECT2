using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem.Interfaces;

public interface IDoctorRepository
{
    Task<Doctor> AddAsync(Doctor doctor);
    Task<bool> UpdateAsync(Doctor doctor);
    Task<bool> DeleteAsync(int doctorId);

    Task<Doctor?> GetByIdAsync(int doctorId);
    Task<List<Doctor>> GetAllAsync();

    Task<bool> ExistsAsync(int doctorId);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> LicenseNumberExistsAsync(string licenseNumber);
}