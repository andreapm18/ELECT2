using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem.Services.Interfaces;

public interface IAppointmentService
{
    Task<(bool ok, string message, Appointment? appointment)> BookAsync(int patientId, int doctorId, DateTime dateTime);
    Task<(bool ok, string message)> CancelAsync(int appointmentId);
    Task<(bool ok, string message)> CompleteAsync(int appointmentId);
    Task<(bool ok, string message)> UpdateStatusAsync(int appointmentId, string status);
}