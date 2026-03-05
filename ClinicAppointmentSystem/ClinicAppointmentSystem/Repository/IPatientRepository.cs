using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem.Interfaces;

public interface IPatientRepository
{
    Task<Patient> AddAsync(Patient patient);
    Task<bool> UpdateAsync(Patient patient);
    Task<bool> DeleteAsync(int patientId);

    Task<Patient?> GetByIdAsync(int patientId);
    Task<List<Patient>> GetAllAsync();

    Task<bool> ExistsAsync(int patientId);
    Task<bool> PatientNumberExistsAsync(string patientNumber);
}