using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem.Services.Interfaces;

public interface IPatientService
{
    Task<(bool ok, string message, Patient? patient)> RegisterAsync(Patient patient);
    Task<(bool ok, string message)> DeleteAsync(int patientId);
}